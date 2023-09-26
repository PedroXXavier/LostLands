using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;


    void Start() {
        anim= GetComponent<Animator>();
    }

    public void OpenDoor() {
        anim.SetTrigger("OpenDoor");
    }
}
