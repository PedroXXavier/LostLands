using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Rock : MonoBehaviour
{
    PhotonView phView;

    public GameObject particle;

    public AudioSource destroyRockSFX;
    public int life = 3;

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
        if (life <= 1)
        {
            destroyRockSFX.Play();
            Destroy(gameObject);
        }

        life--;

        particle.transform.position = gameObject.transform.position;
        particle.SetActive(true);
    }
}
