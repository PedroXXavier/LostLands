using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class FragmentControl : MonoBehaviour
{
    PhotonView phView;

    [Header("Fragments")]
    public bool[] fragmentsCollected;

    [Header("Achieviments")]
    [SerializeField] bool galinhaCollected, indoAliCollected;

    [Header("Win")]
    [SerializeField] int playersChose; public GameObject playerNumberText;
    public bool openWin; bool canChose;

    private void Start() {
        phView = GetComponent<PhotonView>();
    }

    public void VoteYesButton()
    {
        if(canChose)
        {
            if (playersChose == 0)
                playersChose = 1;
            else if (playersChose == 1)
                playersChose = 2;

            StartCoroutine("PlayersNumber");
        }
    }

    IEnumerator PlayerNumber()
    {

        yield return new WaitForSeconds(8);
    }

    public void CollectGalinha()
    {
        phView.RPC("CollectGalinha_RPC", RpcTarget.AllBuffered);
    }

    public void CollectIndoAli()
    {
        phView.RPC("CollectIndoAli_RPC", RpcTarget.AllBuffered);
    }

    public void CollectFragment(int id)
    {
        phView.RPC("CollectFragments_RPC", RpcTarget.AllBuffered, id);
    }

    [PunRPC]
    private void CollectGalinha_RPC(int id)
    {
        galinhaCollected= true;
    }

    [PunRPC]
    private void CollectIndoAli_RPC(int id)
    {
        indoAliCollected= true;
    }

    [PunRPC]
    private void CollectFragments_RPC(int id)
    {
        fragmentsCollected[id] = true;
    }
}
