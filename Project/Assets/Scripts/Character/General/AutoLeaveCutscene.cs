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

    [SerializeField] int type;

    void Start() {
        phView= GetComponent<PhotonView>();

        if (type == 0)
            StartCoroutine("Fade");
        else if (type == 1)
            SceneManager.LoadScene("MainMenu");
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
