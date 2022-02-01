using UnityEngine;
using System.Collections;

public class Flamingo : MonoBehaviour {
    Animator flamingo;
    private IEnumerator coroutine;
    // Use this for initialization
    void Start() {
        flamingo = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            flamingo.SetBool("idle", true);
            flamingo.SetBool("run", false);
            flamingo.SetBool("walk", false);
            flamingo.SetBool("eat", false);
            flamingo.SetBool("sitdown", false);
            flamingo.SetBool("standup", false);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            flamingo.SetBool("eat", true);
            flamingo.SetBool("run", false);
            flamingo.SetBool("walk", false);
            flamingo.SetBool("idle", false);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            flamingo.SetBool("sitdown", true);
            flamingo.SetBool("run", false);
            flamingo.SetBool("walk", false);
            flamingo.SetBool("idle", false);
            flamingo.SetBool("standup", false);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            flamingo.SetBool("standup", true);
            flamingo.SetBool("run", false);
            flamingo.SetBool("walk", false);
            flamingo.SetBool("idle", false);
            flamingo.SetBool("sitdown", false);
        }
        if (Input.GetKey("up"))
        {
            flamingo.SetBool("walk", true);
            flamingo.SetBool("run", false);
            flamingo.SetBool("eat", false);
            flamingo.SetBool("idle", false);
        }
        if (Input.GetKey("left"))
        {
            flamingo.SetBool("turnleft", true);
            flamingo.SetBool("walk", false);
            StartCoroutine("walk");
            walk();
        }
        if (Input.GetKey("right"))
        {
            flamingo.SetBool("turnright", true);
            flamingo.SetBool("walk", false);
            StartCoroutine("walk");
            walk();
        }
        if (Input.GetKey(KeyCode.Keypad5))
        {
            flamingo.SetBool("run", true);
            flamingo.SetBool("walk", false);
            flamingo.SetBool("eat", false);
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            flamingo.SetBool("runleft", true);
            flamingo.SetBool("run", false);
            StartCoroutine("run");
            run();
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            flamingo.SetBool("runright", true);
            flamingo.SetBool("run", false);
            StartCoroutine("run");
            run();
        }
    }
    IEnumerator walk()
    {
        yield return new WaitForSeconds(0.7f);
        flamingo.SetBool("turnleft", false);
        flamingo.SetBool("turnright", false);
        flamingo.SetBool("walk", true);
    }
    IEnumerator run()
    {
        yield return new WaitForSeconds(0.4f);
        flamingo.SetBool("runleft", false);
        flamingo.SetBool("runright", false);
        flamingo.SetBool("run", true);
    }

}
