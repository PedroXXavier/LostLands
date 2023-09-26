using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class Rock : MonoBehaviour
{
    PhotonView phView;

    public AudioSource destroyRockSFX;

    [SerializeField] int life = 3;

    void Awake()
    {
        phView= GetComponent<PhotonView>();
        gameObject.tag = "PickaxeInteract";
    }

    public void Pickaxe()
    {
        phView.RPC("PickaxeRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void PickaxeRPC()
    {
        life--;

        if (life <= 0)
        {
            Destroy(gameObject);
            destroyRockSFX.Play();
        }
    }
}
