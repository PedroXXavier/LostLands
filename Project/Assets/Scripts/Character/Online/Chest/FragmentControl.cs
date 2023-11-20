using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class FragmentControl : MonoBehaviour
{
    PhotonView phView;
    public bool[] fragmentsCollected;

    public bool allFrag;

    private void Start() {
        phView = GetComponent<PhotonView>();
    }

    private void Update()
    {
/*        if (fragmentsCollected[0] && fragmentsCollected[1] )
        {
            phView.RPC("Win", RpcTarget.AllBuffered);
        }*/
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

    [PunRPC]
    private void Win()
    {
        allFrag= true;
    }
}
