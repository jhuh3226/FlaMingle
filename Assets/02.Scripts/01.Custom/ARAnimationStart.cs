using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARAnimationStart : MonoBehaviour {
    public GameObject HitPointfab;
    public GameObject animationPrefab;
    protected new GameObject animation = null;
    [HideInInspector]
    public bool isAnimation = false;

    public CanvasGroup uiElement;
    public bool isARScene;
    [HideInInspector]
    public bool animationStartWaiting = false;

    private Vector3 hitPoint = Vector3.zero;

    private void Start () { }

    // private void Update () {
    //     if (animation != null) {
    //         if (isARScene) {
    //             if (animation.GetComponent<AnimationRePlay> ().isRePlay) {
    //                 animation.SetActive (false);
    //                 animation = null;
    //                 isAnimation = false;
    //             }
    //         }
    //     }
    // }

    // public void AnimationStart (Collision collision) {
    //     animationStartWaiting = true;
    //     StartCoroutine (FadeCanvasGroup (collision, 0.8f));
    // }

    // public IEnumerator FadeCanvasGroup (Collision collision, float lerpTime = 2) {
    //     uiElement.gameObject.SetActive (true);

    //     float _timeStartedLerping = Time.time;
    //     float timeSinceStarted = Time.time - _timeStartedLerping;
    //     float percentageComplete = timeSinceStarted / lerpTime;

    //     while (true) {
    //         timeSinceStarted = Time.time - _timeStartedLerping;
    //         percentageComplete = timeSinceStarted / lerpTime;

    //         float currentValue = Mathf.Lerp (0, 1, percentageComplete);

    //         uiElement.alpha = currentValue;

    //         if (percentageComplete >= 1) break;

    //         yield return new WaitForSeconds (0.03f);
    //     }

    //     if (isARScene) {
    //         yield return new WaitForSeconds (3f);
    //     } else {
    //         yield return new WaitForSeconds (2f);
    //     }

    //     _timeStartedLerping = Time.time;
    //     timeSinceStarted = Time.time - _timeStartedLerping;
    //     percentageComplete = timeSinceStarted / lerpTime;

    //     while (true) {
    //         timeSinceStarted = Time.time - _timeStartedLerping;
    //         percentageComplete = timeSinceStarted / lerpTime;

    //         float currentValue = Mathf.Lerp (1, 0, percentageComplete);

    //         uiElement.alpha = currentValue;

    //         if (percentageComplete >= 1) break;

    //         yield return new WaitForSeconds (0.03f);
    //     }

    //     uiElement.gameObject.SetActive (false);
    //     animationStartWaiting = false;

    //     if (hitPoint == Vector3.zero) {
    //         hitPoint = collision.contacts[0].point;
    //     }
    //     ShowPlane (false);

    //     //Quaternion rotation = Quaternion.Euler(0, 330, 0);      // 2x2x2 정위치
    //     ///////////////////////////////////////////////////////////////////////////////////////////////////////
    //     //Quaternion rotation = Quaternion.Euler(0, 295, 0);      //
    //     Quaternion rotation = Quaternion.Euler (0, 290, 0); //
    //     animation = Instantiate (animationPrefab, hitPoint, rotation);
    //     animation.SetActive (true);

    //     Instantiate (HitPointfab, hitPoint, rotation);
    // }

    // public ARPlaneManager arPlane;

    // public void ShowPlane (bool b) {
    //     arPlane = gameObject.GetComponent<ARPlaneManager> ();
    //     foreach (var plane in arPlane.trackables)
    //         plane.gameObject.SetActive (b);
    // }
}