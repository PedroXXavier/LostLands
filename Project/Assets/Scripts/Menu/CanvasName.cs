using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class CanvasName : MonoBehaviour
{
    public PhotonView phView;
    GameObject cameraScene;
    TMP_Text nickTxt;

    private void Awake()
    {
        nickTxt=GetComponentInChildren<TMP_Text>();
        cameraScene = Camera.main.gameObject;
    }

    void Update()
    {
        Vector3 newRot = new Vector3(transform.position.x - cameraScene.transform.position.x, 0, transform.position.z - cameraScene.transform.position.z);
        transform.rotation = Quaternion.LookRotation(newRot);
    }

    public void SetNickText(string nick)
    {
        phView.Controller.NickName= nick;
        nickTxt.text = nick;
    }
}
