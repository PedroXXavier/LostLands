using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

public class AutoLeaveCutscene : MonoBehaviour
{
    PhotonView phView;
    public GameObject fade;

    void Start() {
        StartCoroutine("Fade");
        phView= GetComponent<PhotonView>(); 
    }

    IEnumerator Fade()
    {
        phView.RPC("FadeToPlay", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("mapa");
    }

    [PunRPC]
    private void FadeToPlay()
    {
        fade.SetActive(true);
    }
}
