using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class Chest : MonoBehaviour
{
    PhotonView phView;
    Animator anim;

    public AudioSource openChestSFX, grabFragment;

    public bool fragmentActived;

    public int life = 3;
    public int id;

    private void Start()
    {
        gameObject.tag = "ShovelInteract";

        phView = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
    }

    public void OpenChest()
    {
        phView.RPC("OpenChestRPC", RpcTarget.AllBuffered);
    }

    public void Shovel()
    {
        phView.RPC("ShovelRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ShovelRPC()
    {
        life--;

        //botar partícula

        if (life == 2)
        {
            anim.SetTrigger("1");
        }
        if (life == 1)
        {
            anim.SetTrigger("2");
        }
        if (life == 0)
        {
            anim.SetTrigger("3");
        }
    }

    [PunRPC]
    public void OpenChestRPC()
    {
        anim.SetTrigger("Openned");
        life--;

        openChestSFX.Play();
        grabFragment.Play();
    }
}
