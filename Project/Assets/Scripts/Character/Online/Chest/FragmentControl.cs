using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Newtonsoft.Json.Bson;
using Unity.VisualScripting;

public class FragmentControl : MonoBehaviour
{
    PhotonView phView;

    [Header("Fragments")]
    public bool[] fragmentsCollected;

    [Header("Achieviments")]
    public bool indoAliCollected; public bool galinhaCollected;

    [Header("Win")]
    [SerializeField] int playersChose; public GameObject playerNumber;
    bool canChose = true; public TMP_Text txt;

    public GameObject fade;

    private void Start() {
        phView = GetComponent<PhotonView>();
        DontDestroyOnLoad(gameObject);
    }

    public void VoteYesButton()
    {
        if(canChose)
            StartCoroutine("ShowNumber");
    }

    public void VoteCancelButton()
    {
        StartCoroutine("CancelNumber");
    }

    IEnumerator ShowNumber()
    {
        phView.RPC("OpenNumber", RpcTarget.AllBuffered);
        canChose= false;
        yield return new WaitForSeconds(12);
        phView.RPC("CloseNumber", RpcTarget.AllBuffered);
        phView.RPC("ZeroNumber", RpcTarget.AllBuffered);
        canChose = true;
    }

    IEnumerator CancelNumber()
    {
        StopCoroutine("ShowNumber");

        phView.RPC("CloseNumber", RpcTarget.AllBuffered);
        phView.RPC("ZeroNumber", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(0.1f);
        canChose = true;
    }

    [PunRPC]
    private void OpenNumber()
    {
        playerNumber.SetActive(true);

        if (playersChose == 0) {
            playersChose = 1; txt.text = "1/2";
        }
        else if (playersChose == 1) {
            indoAliCollected= true;
            playersChose = 2; txt.text = "2/2";
            StartCoroutine("FadeToCutscene");
        }
    }

    [PunRPC]
    private void CancelNumber_RPC()
    {
        StartCoroutine("CancelNumber");

        if(playersChose == 1)
        {
            playersChose = 0; txt.text = "0/2";
        }
    }
    
    private void OnDisconnectedFromServer()
    {
        Destroy(gameObject);
    }

    [PunRPC]
    private void CloseNumber()
    {
        playerNumber.SetActive(false);
    }

    [PunRPC]
    private void ZeroNumber()
    {
        playersChose = 0;
    }

    IEnumerator FadeToCutscene()
    {
        phView.RPC("Fade_RPC", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("FinalCutscene");
    }

    [PunRPC]
    private void Fade_RPC()
    {
        fade.SetActive(true);
    }


    private void OnDisconnectedFromMasterServer()
    {
        Destroy(gameObject);
    }


    public void CollectGalinha()
    {
        phView.RPC("CollectGalinha_RPC", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void CollectGalinha_RPC()
    {
        galinhaCollected = true;
    }

    public void CollectFragment(int id)
    {
        phView.RPC("CollectFragments_RPC", RpcTarget.AllBuffered, id);
    }
    [PunRPC]
    private void CollectFragments_RPC(int id)
    {
        fragmentsCollected[id] = true;
    }
}
