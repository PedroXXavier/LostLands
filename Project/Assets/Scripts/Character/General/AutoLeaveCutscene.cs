using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.Rendering;

public class AutoLeaveCutscene : MonoBehaviour
{
    PhotonView phView;
    public GameObject fade; public string scene;

    [SerializeField] int type;

    private void Awake()
    {
        phView = GetComponent<PhotonView>();
    }

    void Start() {
        if (type == 0)
            StartCoroutine("Fade");
        else if (type == 1)
            SceneManager.LoadScene("MainMenu");
    }

    IEnumerator Fade()
    {
        phView.RPC("FadeToPlay", RpcTarget.All);
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel(scene);
    }

    [PunRPC]
    private void FadeToPlay()
    {
        fade.SetActive(true);
    }
}
