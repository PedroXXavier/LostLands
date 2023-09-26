using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class HudOnline : MonoBehaviour
{
    PhotonView phview;

    private void Start() {
        phview = GetComponent<PhotonView>();

        if(!phview.IsMine)
            gameObject.SetActive(false); }
}