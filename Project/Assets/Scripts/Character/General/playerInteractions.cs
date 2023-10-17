using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Pun.Demo.Cockpit;

public class playerInteractions : MonoBehaviour
{
    GameController gc;

    public PhotonView phView;
    CanvasName nickNameTxt;

    private void Start()
    {
        gc = FindObjectOfType(typeof(GameController)) as GameController;

        nickNameTxt = GetComponentInChildren<CanvasName>();
        phView = GetComponent<PhotonView>();

        //phView.RPC("RPC_SetNickText", RpcTarget.All, PlayerPrefs.GetString("Nick"));
    }

    [PunRPC]
    void RPC_SetNickText(string nick)
    {
        nickNameTxt.SetNickText(nick);
    }
}
