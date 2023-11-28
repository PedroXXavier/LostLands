using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class FinalCutscene : MonoBehaviour
{
    FragmentControl frag;
    public GameObject indoAli, galinha;
    public TMP_Text collectedNumber;

    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();  
        frag = FindObjectOfType(typeof(FragmentControl)) as FragmentControl;
    }

    void Update()
    {
        if (frag.indoAliCollected)
            view.RPC("IndoAli", RpcTarget.All);

        if (frag.galinhaCollected)
            view.RPC("Galinha", RpcTarget.All);

        ChangeText();
    }

    [PunRPC]
    private void ChangeText()
    {
        if (frag.indoAliCollected && frag.galinhaCollected)
            collectedNumber.text = "2/2";
        else if(frag.indoAliCollected && !frag.galinhaCollected)
            collectedNumber.text = "1/2";
    }

    [PunRPC]
    private void IndoAli()
    {
        indoAli.SetActive(true);
    }

    [PunRPC]
    private void Galinha()
    {
        galinha.SetActive(true);
    }
}
