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
    Animator m_Animator;
    public RuntimeAnimatorController FlamingoComeback;
    public bool firstAnimationDone, secondAnimationDone = false;

    private void Awake () {
        _arRaycastManager = GetComponent<ARRaycastManager> ();
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
        if (m_Animator != null) CheckFirstAnimation (); // animation passing to canvas holder
        if (eventSystem.GetComponent<CanvasHolder> ().flamingoCombackInitiated) {
            Invoke("CheckSecondAnimation", 2f);
            AnimateFlamingoComeback ();
        }

        if (!TryGetTouchPosition (out Vector2 touchPosition)) return;
        if (_arRaycastManager.Raycast (touchPosition, hits, TrackableType.PlaneWithinPolygon)) {
            // get hitpoint
            var hitPose = hits[0].pose;

            // spawn object ready or not?
            if (spawnedObject == null) {
                spawnedObject = Instantiate (flamingoPack, hitPose.position, hitPose.rotation);

                foreach (Transform child in spawnedObject.transform) {
                    singleFlamingos[i] = child.gameObject;
                    // Debug.Log (singleFlamingos[i]);
                    i += 1;
                }
                // singleFlamingo = spawnedObject.transform.GetChild (0).gameObject; // get the firstchild objects of the flamingo packs
                m_Animator = singleFlamingos[0].GetComponent<Animator> ();

            } else {
                spawnedObject.transform.position = hitPose.position;
            }
        }
    }

    void CheckFirstAnimation () {
        if (m_Animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 4.5f) {
            Debug.Log ("1st animation done");
            firstAnimationDone = true;
        }
    }

    void CheckSecondAnimation () {
        if (m_Animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
            Debug.Log ("2nd animation done");
            secondAnimationDone = true;
        }
    }

    void AnimateFlamingoComeback () {
        foreach (GameObject single in singleFlamingos) single.GetComponent<Animator> ().runtimeAnimatorController = FlamingoComeback as RuntimeAnimatorController;
        Debug.Log ("Flamingos going near the person");
    }
}