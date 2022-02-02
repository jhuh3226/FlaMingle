using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent (typeof (ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour {
    public GameObject flamingoPack, eventSystem;
    // [SerializeField]
    private GameObject spawnedObject, singleFlamingo; // reference of the created object
    private GameObject[] singleFlamingos; // reference to the child of created object
    int i = 0;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition; // position of touch

    static List<ARRaycastHit> hits = new List<ARRaycastHit> (); // reference of raycast

    // Animation controll
    List<Animator> m_Animator; // has to be list
    public RuntimeAnimatorController FlamingoComeback;
    public bool firstAnimationDone, secondAnimationDone = false;

    private void Awake () {
        _arRaycastManager = GetComponent<ARRaycastManager> ();
        // m_Animator = new List<Animator> ();
    }

    bool TryGetTouchPosition (out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch (0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }
    void Start () {
        singleFlamingos = new GameObject[transform.childCount];
    }

    void Update () {
        /* if animator is detected play animPhase1 */
        if (m_Animator != null) AnimPhase1 ();

        /* if flamingo posture dected play animPhase2 */
        /* if flamingo bumps into mingle trigger zone, play animPhase3 */
        if (eventSystem.GetComponent<CanvasHolder> ().flamingoCombackInitiated) {
            Invoke ("AnimPhase2", 2f);
            AnimPhase3 ();
        }

        if (!TryGetTouchPosition (out Vector2 touchPosition)) return;
        if (_arRaycastManager.Raycast (touchPosition, hits, TrackableType.PlaneWithinPolygon)) {

            var hitPose = hits[0].pose; // get hitpoint

            // spawn object ready or not?
            if (spawnedObject == null) {
                spawnedObject = Instantiate (flamingoPack, hitPose.position, hitPose.rotation);

                foreach (Transform child in spawnedObject.transform) {
                    singleFlamingos[i] = child.gameObject;
                    i += 1;
                }
                // singleFlamingo = spawnedObject.transform.GetChild (0).gameObject; // get the firstchild objects of the flamingo packs

                // if there are singleflaminogs, put each animator to list
                if (singleFlamingos != null) {
                    m_Animator = new List<Animator> (); // initiate new list, for animator only list is possible

                    try {
                        for (int i = 0; i < singleFlamingos.Length; i++) {
                            m_Animator.Add (singleFlamingos[i].GetComponent<Animator> ());
                        }
                    } catch { }
                }

            } else {
                spawnedObject.transform.position = hitPose.position;
            }
        }
    }

    /* when the animation starts and go over 4 seconds, stop flamingos running by controlling animation */
    void AnimPhase1 () {
        if (m_Animator[0].GetCurrentAnimatorStateInfo (0).normalizedTime >= 4f) {
            // Debug.Log ("1st animation done");
            firstAnimationDone = true; // pass the bool to canvasHolder

            // stop all running flamingos
            try {
                foreach (Animator anim in m_Animator) {
                    anim.SetBool ("IsStopRunning", true);
                }
            } catch { }
        }
    }

    /* when the person poses flamingo posture, make flamingos to comeback */
    void AnimPhase2 () {
        // enable flamingos comback
        try {
            foreach (Animator anim in m_Animator) {
                anim.SetBool ("IsComeback", true);
            }
        } catch { }
    }

    /* If flamingo gets near the person(collides with the circle collider), make flamingos mingle */
    void AnimPhase3 () {
        // foreach (GameObject single in singleFlamingos) {
        //     if (single.GetComponent<FlamingoCollisionDetect> ().collided) {
        //         Debug.Log ("collided");
        //         foreach (Animator anim in m_Animator) {
        //             anim.SetBool ("IsMingle", true);
        //         }
        //     }
        // }

        // for all the flamingos, enable collider to detect trigger event
        for (int i = 0; i < singleFlamingos.Length; i++) {
            // singleFlamingos[i].GetComponent<FlamingoCollisionDetect>.enabled = true;
            // Debug.Log ("name each" + singleFlamingos[i]); // this returns 5
            // Debug.Log ("tag" + singleFlamingos[i].gameObject.tag);
            try {
                (singleFlamingos[i].GetComponent (typeof (CapsuleCollider)) as Collider).enabled = true;
            } catch { }
        }

        // when the flamingo enters trigger zone, set mingle animation true
        for (int i = 0; i < singleFlamingos.Length; i++) {
            // singleFlamingos[i].GetComponent<FlamingoCollisionDetect>.enabled = true;
            try {
                if (singleFlamingos[i].GetComponent<FlamingoCollisionDetect> ().collided) {
                    Debug.Log (singleFlamingos[i] + " collided");
                    singleFlamingos[i].GetComponent<Animator> ().SetBool ("IsMingle", true);
                } else { Debug.Log (singleFlamingos[i] + " didn't collided"); }
            } catch { }
        }

        secondAnimationDone = true; // send bool to canvas holder
    }

    // void  NotInUse() {
    //     // foreach (GameObject single in singleFlamingos) single.GetComponent<Animator> ().runtimeAnimatorController = FlamingoComeback as RuntimeAnimatorController;
    //     Debug.Log ("Flamingos going near the person");
    // }
}