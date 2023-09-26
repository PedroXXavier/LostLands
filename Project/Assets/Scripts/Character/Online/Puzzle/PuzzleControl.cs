using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class PuzzleControl : MonoBehaviour
{
    PhotonView phView;

    public bool[] sequenceNumber;
    public GameObject[] pieces;

    public GameObject door;

    void Start()
    {
        phView = GetComponent<PhotonView>();
    }


    private void Update()
    {
        phView.RPC("Check_RPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Check_RPC()
    {
        if (sequenceNumber[0]
            && sequenceNumber[1]
            && sequenceNumber[2]
            && sequenceNumber[3]
            && sequenceNumber[4]
            && sequenceNumber[5]
            && sequenceNumber[6]
            && sequenceNumber[7]
            && sequenceNumber[8])
        {
            door.GetComponent<SymbolDoor>().OpenDoor();

            pieces[0].gameObject.tag = "Nada";
            pieces[1].gameObject.tag = "Nada";
            pieces[2].gameObject.tag = "Nada";
            pieces[3].gameObject.tag = "Nada";
            pieces[4].gameObject.tag = "Nada";
            pieces[5].gameObject.tag = "Nada";
            pieces[6].gameObject.tag = "Nada";
            pieces[7].gameObject.tag = "Nada";
            pieces[8].gameObject.tag = "Nada";

            gameObject.GetComponent<PuzzleControl>().enabled = false;
        }
    }
}
