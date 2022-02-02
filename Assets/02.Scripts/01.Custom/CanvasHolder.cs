using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARFoundation.Samples;
using UnityEngine.XR.ARSubsystems;

public class CanvasHolder : MonoBehaviour {
    public Canvas cvPointPerson, cvFootGuide, cvFootText, cvPoseGuide, cvMessage;
    public GameObject poseGuideText1, poseGuideText2;
    public GameObject messageText1, messageText2, messageImg;
    public GameObject HumanBodyTracker, ArCamera, ArSessionOrigin; // the flamingo here is spawned packs of flamingo

    public bool pointGuideInitiated, footGuideInitiated, poseGuideInitiated, flamingoCombackInitiated = false;

    Animator m_Animator;
    public RuntimeAnimatorController FlamingoTest, FlamingoComeback;

    int textCounter = 0;

    // Start is called before the first frame update
    void Start () {
        cvPointPerson.enabled = true;
        cvFootGuide.enabled = false;
        cvFootText.enabled = false;
        cvPoseGuide.enabled = false;
        cvMessage.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate () {
        /* 01. cv saying, point toward person */

        /* 02. get the position of the person's foot and enable the guide cv(world-view) and cv text(overlay) */
        // if person is recognized
        if (HumanBodyTracker.GetComponent<HumanBodyTracker> ().foundPerson && !pointGuideInitiated) {
            ControlFootGuide ();
            pointGuideInitiated = !pointGuideInitiated;
            // Debug.Log ("found person");
        }

        /* 03. person initiates flamingo and floor guide gets deactivated. And flamingo anymation plays */
        /* *what to do when person retouches the surface? */
        else if (Input.touchCount > 0 && cvFootGuide.enabled && !footGuideInitiated) {
            Debug.Log ("Flamingo initiated");
            cvFootGuide.enabled = false;
            cvFootText.enabled = false;
            footGuideInitiated = !footGuideInitiated;
            // AnimateFlamingoFlea (); // play flamingo flea animation

            // hide raycast manger and stop people to create multiple packs of flamingos
        }

        /* 04. When the animation finishes, enable cv guiding people with to make flamingo posture (mingle and get part of it) */
        /* get the animation finish information from ARTapToPlaceObject */
        else if (ArSessionOrigin.GetComponent<ARTapToPlaceObject> ().firstAnimationDone && !poseGuideInitiated) {
            Debug.Log ("AnimationFinished");
            ControlPoseGuide ();
            poseGuideInitiated = !poseGuideInitiated;
        }

        /* 05. when assumed the right pose*/
        /* *what if posture is incorrect?*/
        else if (ArCamera.GetComponent<ScreenSpaceJointVisualizer> ().flamingoPose && cvPoseGuide.enabled) {
            Debug.Log ("The pose is correct");
            flamingoCombackInitiated = true;
        }

        /* 06. When all flamingos come near, final message pops up*/
        else if (ArSessionOrigin.GetComponent<ARTapToPlaceObject> ().secondAnimationDone && flamingoCombackInitiated) {
            Invoke ("ControlMessage", 1f);
        }

    }

    void ControlFootGuide () {
        cvPointPerson.enabled = false;
        cvFootGuide.enabled = true;
        cvFootText.enabled = true;
    }

    void ControlPoseGuide () {
        cvFootGuide.enabled = false;
        cvFootText.enabled = false;
        cvPoseGuide.enabled = true;
        if (ArCamera.GetComponent<ScreenSpaceJointVisualizer> ().posing) {
            poseGuideText1.SetActive (false);
            poseGuideText2.SetActive (true);
        }
    }

    void ControlMessage () {
        cvPoseGuide.enabled = false;
        cvMessage.enabled = true;
        messageImg.transform.Rotate (Vector3.up * Time.deltaTime * 100);
        messageText1.transform.Rotate (Vector3.up * Time.deltaTime * 100);
        messageText2.transform.Rotate (Vector3.up * Time.deltaTime * 100);

        textCounter++;
        if (textCounter == 200) {
            textCounter = 0;
        }

        // jump between two sentences
        if (textCounter < 100) {
            messageText1.SetActive (true);
            messageText2.SetActive (false);
        } else if (textCounter >= 100 & textCounter <= 200) {
            messageText1.SetActive (false);
            messageText2.SetActive (true);
        }
    }

    void FindFlamingo () {
        // FlamingoPack = (GameObject) Instantiate (FlamingoPackPrefab);
        // FlamingoPack = GameObject.FindWithTag ("ArSessionOrigin").GetComponent<ARTapToPlaceObject> ().spawnedObject;
        // Invoke ("FindSingleFlamingo", 2);
        // SingleFlamingo = FlamingoPack.transform.GetChild (0).gameObject;
        // Debug.Log ($"Name: {SingleFlamingo}");
        // m_Animator = SingleFlamingo.GetComponent<Animator> ();
    }
}