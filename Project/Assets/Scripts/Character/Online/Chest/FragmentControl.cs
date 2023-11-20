using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class FragmentControl : MonoBehaviour
{
    PhotonView phView;
    public bool[] fragmentsCollected;

    [SerializeField] bool galinhaCollected, indoAliCollected;

    public bool allFrag;

    private void Start() {
        phView = GetComponent<PhotonView>();
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
