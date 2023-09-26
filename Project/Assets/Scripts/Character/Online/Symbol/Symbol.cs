using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class Symbol : MonoBehaviour
{
    public Animator anim;

    PhotonView phView;

    public bool isSelected;
    public int id;

    //public bool canInteract;

    private void Start() {
        anim = GetComponent<Animator>();
        phView = GetComponent<PhotonView>();
    }

    public void Press() {
        phView.RPC("Press_RPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Press_RPC()
    {
        isSelected = true;
        anim.SetBool("Pressed", isSelected);
    }
}