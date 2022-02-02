using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingoCollisionDetect : MonoBehaviour {
    // Start is called before the first frame update
    public bool collided = false;
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }

    // collider should be activated only after flamingos ran way from the person
    void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "StopSphere") {
            collided = true;
            // Debug.Log ("collided with stop collider");
        }
    }
}