using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent (typeof (ARRaycastManager), typeof (AudioSource), typeof (ARPlaneManager))]
public class ARTapToPlaceObject : MonoBehaviour {
    public GameObject flamingoPack, eventSystem;
    // [SerializeField]
    private GameObject spawnedObject, singleFlamingo; // reference of the created object
    List<GameObject> singleFlamingos;
    // private GameObject[] singleFlamingos; // reference to the child of created object
    int i = 0;

    /* Raycast control */
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition; // position of touch

    static List<ARRaycastHit> hits = new List<ARRaycastHit> (); // reference of raycast

    /* Animation control */
    List<Animator> m_Animator; // has to be list
    public RuntimeAnimatorController FlamingoComeback;
    public bool firstAnimationDone, secondAnimationDone = false;

    /* Audio control */
    AudioSource m_audio;
    public AudioClip frightenedFlamingo, calmFlamingo;
    bool played, calmPlayed = false;

    private void Awake () {
        _arRaycastManager = GetComponent<ARRaycastManager> ();
        m_ARPlaneManager = GetComponent<ARPlaneManager> (); // toggle plane visibility
    }

    bool TryGetTouchPosition (out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch (0).position;

            // Block UI
            bool isOverUI = touchPosition.IsPointOverUIObject ();

            if (isOverUI) {
                Debug.Log ("touch over UI");
            }
            return true;
        }

        touchPosition = default;
        return false;
    }
    void Start () {
        // singleFlamingos = new GameObject[transform.childCount];
        singleFlamingos = new List<GameObject> ();
    }

    void Update () {
        /* if animator is detected play animPhase0 */
        if (m_Animator != null) AnimPhase0 ();

        /* if animator is detected play animPhase1 */
        if (m_Animator != null) AnimPhase1 ();

        /* if flamingo posture dected play animPhase2 */
        /* if flamingo bumps into mingle trigger zone, play animPhase3 */
        if (eventSystem.GetComponent<CanvasHolder> ().flamingoCombackInitiated) {
            Debug.Log("testing flamingo comeback");
            Invoke ("AnimPhase2", 2f);
            AnimPhase3 ();
        }

        if (!TryGetTouchPosition (out Vector2 touchPosition)) return;
        if (_arRaycastManager.Raycast (touchPosition, hits, TrackableType.PlaneWithinPolygon)) {

            var hitPose = hits[0].pose; // get hitpoint
            TogglePlaneDetection ();

            // spawn object ready or not?
            if (spawnedObject == null) {
                spawnedObject = Instantiate (flamingoPack, hitPose.position, hitPose.rotation);

                foreach (Transform child in spawnedObject.transform) {
                    try {
                        singleFlamingos.Add (child.gameObject);
                        Debug.Log (singleFlamingos);
                    } catch { }
                }

                // foreach (Transform child in spawnedObject.transform) {
                //     try {
                //         singleFlamingos[i] = child.gameObject;
                //         Debug.Log (singleFlamingos[i]);
                //         i += 1;
                //     } catch { }
                // }

                // try {
                //     for (int i = 0; i < 7; i++) {
                //         singleFlamingos[i] = spawnedObject.transform.GetChild(i).gameObject;
                //         Debug.Log (singleFlamingos[i]);
                //     }
                // } catch { }

                // singleFlamingo = spawnedObject.transform.GetChild (0).gameObject; // get the firstchild objects of the flamingo packs

                // if there are singleflaminogs, put each animator to list
                if (singleFlamingos != null) {
                    Invoke ("AddAnimatorToList", 1f);
                }

            } else {
                spawnedObject.transform.position = hitPose.position;
            }
        }
    }

    void AddAnimatorToList () {
        m_Animator = new List<Animator> (); // initiate new list, for animator only list is possible

        try {
            for (int i = 0; i < singleFlamingos.Count; i++) {
                // for (int i = 0; i < singleFlamingos.Length; i++) {
                Debug.Log (singleFlamingos.Count);
                m_Animator.Add (singleFlamingos[i].GetComponent<Animator> ());
            }
        } catch { }
    }
    void AnimPhase0 () {
        if (m_Animator[0].GetCurrentAnimatorStateInfo (0).IsName ("turnright")) {
            // if (m_Animator[0].GetCurrentAnimatorStateInfo (0).normalizedTime >= 1f) {
            Debug.Log ("Flamingo Started running");
            m_audio = spawnedObject.GetComponent<AudioSource> ();
            if (!played) {
                m_audio.clip = frightenedFlamingo;
                m_audio.Play ();
                played = true;
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
        m_audio = spawnedObject.GetComponent<AudioSource> ();
        m_audio.clip = calmFlamingo;
        if (!calmPlayed) {
            Debug.Log ("change audio to calm");
            m_audio.Play ();
            calmPlayed = true;
        }

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
        for (int i = 0; i < singleFlamingos.Count; i++) {
            // singleFlamingos[i].GetComponent<FlamingoCollisionDetect>.enabled = true;
            // Debug.Log ("name each" + singleFlamingos[i]); // this returns 5
            // Debug.Log ("tag" + singleFlamingos[i].gameObject.tag);
            try {
                (singleFlamingos[i].GetComponent (typeof (CapsuleCollider)) as Collider).enabled = true;
            } catch { }
        }

        // when the flamingo enters trigger zone, set mingle animation true
        for (int i = 0; i < singleFlamingos.Count; i++) {
            // singleFlamingos[i].GetComponent<FlamingoCollisionDetect>.enabled = true;
            try {
                if (singleFlamingos[i].GetComponent<FlamingoCollisionDetect> ().collided) {
                    // Debug.Log (singleFlamingos[i] + " collided");
                    singleFlamingos[i].GetComponent<Animator> ().SetBool ("IsMingle", true);

                    secondAnimationDone = true; // send bool to canvas holder
                } else { Debug.Log (singleFlamingos[i] + " didn't collided"); }
            } catch { }
        }
    }

    // void  NotInUse() {
    //     // foreach (GameObject single in singleFlamingos) single.GetComponent<Animator> ().runtimeAnimatorController = FlamingoComeback as RuntimeAnimatorController;
    //     Debug.Log ("Flamingos going near the person");
    // }

    /* Toggles plane detection and the visualization of the planes. */
    void TogglePlaneDetection () {
        m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;

        if (m_ARPlaneManager.enabled) {
            SetAllPlanesActive (true);
        } else {
            SetAllPlanesActive (false);
        }
    }

    /// <summary>
    /// Iterates over all the existing planes and activates
    /// or deactivates their <c>GameObject</c>s'.
    /// </summary>
    /// <param name="value">Each planes' GameObject is SetActive with this value.</param>
    void SetAllPlanesActive (bool value) {
        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive (value);
    }

    ARPlaneManager m_ARPlaneManager;
}