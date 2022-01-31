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
    public GameObject HumanBodyTracker, ArCamera, Flamingo;

    bool pointGuideInitiated, footGuideInitiated, poseGuideInitiated, flamingoCombackInitiated = false;

    Animator m_Animator;
    public RuntimeAnimatorController FlamingoTest, FlamingoComeback;

    // Start is called before the first frame update
    void Start () {
        cvPointPerson.enabled = true;
        cvFootGuide.enabled = false;
        cvFootText.enabled = false;
        cvPoseGuide.enabled = false;
        cvMessage.enabled = false;

        m_Animator = Flamingo.GetComponent<Animator> ();
    }

    // Update is called once per frame
    void FixedUpdate () {
        Debug.Log (ArCamera.GetComponent<ScreenSpaceJointVisualizer> ().flamingoPose);

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

            AnimateFlamingoFlea (); // play flamingo flea animation

            // hide raycast manger and stop people to create multiple packs of flamingos
        }

        /* 04. When the animation finishes, enable cv guiding people with to make flamingo posture (mingle and get part of it) */
        else if (m_Animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f && !poseGuideInitiated) {
            Debug.Log ("AnimationFinished");
            ControlPoseGuide ();
            poseGuideInitiated = !poseGuideInitiated;
        }

        /* 05. when assumed the right pose*/
        /* *what if posture is incorrect?*/
        else if (ArCamera.GetComponent<ScreenSpaceJointVisualizer> ().flamingoPose && cvPoseGuide.enabled) {
            Debug.Log ("The pose is correct");
            AnimateFlamingoComback ();
            flamingoCombackInitiated = true;
        }

        /* 06. When all flamingos come near, final message pops up*/
        else if (m_Animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f && flamingoCombackInitiated) {
            ControlMessage ();
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
    }

    void ControlMessage () {
        cvPoseGuide.enabled = false;
        cvMessage.enabled = true;
    }

    void AnimateFlamingoFlea () {
        Flamingo.GetComponent<Animator> ().runtimeAnimatorController = FlamingoTest as RuntimeAnimatorController;
    }

    void AnimateFlamingoComback () {
        Flamingo.GetComponent<Animator> ().runtimeAnimatorController = FlamingoComeback as RuntimeAnimatorController;
    }
}