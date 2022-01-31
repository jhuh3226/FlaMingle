using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEvent : MonoBehaviour {
    // Start is called before the first frame update
    private ARAnimationStart ARAnimationStart;

    void Start () {
        ARAnimationStart = gameObject.GetComponent<ARAnimationStart> ();
    }

    // Update is called once per frame
    void Update () {
        // if (Input.touchCount > 0 && !aRAnimationStartScript.animationStartWaiting) {
        //     if (Input.GetTouch (0).phase == TouchPhase.Began) {
        //         if (EventSystem.current.IsPointerOverGameObject (Input.GetTouch (0).fingerId)) {
        //             isUITouch = true;
        //         }
        //     }
        //     if (Input.GetTouch (0).phase == TouchPhase.Ended && !arBackButtonPanelScript.isFade) {
        //         if (arBackButtonPanelScript.toggle) {
        //             arBackButtonPanelScript.FadeOut ();
        //         } else if (!arBackButtonPanelScript.toggle && !isUITouch) {
        //             arBackButtonPanelScript.FadeIn ();
        //         }
        //         isUITouch = false;
        //     } else if (Input.GetTouch (0).phase == TouchPhase.Ended) {
        //         isUITouch = false;
        //     }
        // }

    }
}