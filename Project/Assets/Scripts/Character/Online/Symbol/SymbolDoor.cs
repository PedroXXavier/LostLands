using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolDoor : MonoBehaviour
{
    Animator anim;


    void Start() {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor() {
        anim.SetTrigger("OpenDoor");
    }
}
