using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class SymbolControl : MonoBehaviour
{
    PhotonView phView;

    public int[] sequenceNumber, correctSequence;
    List<int> randomCorrectSquence;

    public GameObject[] symbols;
    public GameObject[] spotters;
    public GameObject door;

    bool activeTimer;

    private void Start() {
        phView= GetComponent<PhotonView>();
    }

    [PunRPC]
    private void WhiteBack_RPC()
    {
        activeTimer = false;

        symbols[0].gameObject.GetComponent<Symbol>().isSelected = false;
        symbols[1].gameObject.GetComponent<Symbol>().isSelected = false;
        symbols[2].gameObject.GetComponent<Symbol>().isSelected = false;
        symbols[3].gameObject.GetComponent<Symbol>().isSelected = false;
        symbols[4].gameObject.GetComponent<Symbol>().isSelected = false;
        symbols[5].gameObject.GetComponent<Symbol>().isSelected = false;

        symbols[0].gameObject.GetComponent<Symbol>().anim.SetBool("Pressed", false);
        symbols[1].gameObject.GetComponent<Symbol>().anim.SetBool("Pressed", false);
        symbols[2].gameObject.GetComponent<Symbol>().anim.SetBool("Pressed", false);
        symbols[3].gameObject.GetComponent<Symbol>().anim.SetBool("Pressed", false);
        symbols[4].gameObject.GetComponent<Symbol>().anim.SetBool("Pressed", false);
        symbols[5].gameObject.GetComponent<Symbol>().anim.SetBool("Pressed", false);


        spotters[0].GetComponent<Renderer>().material.color = Color.white;
        spotters[1].GetComponent<Renderer>().material.color = Color.white;
        spotters[2].GetComponent<Renderer>().material.color = Color.white;
        spotters[3].GetComponent<Renderer>().material.color = Color.white;
    }

    public void Sequence(int id)
    {
        if(!activeTimer) 
            phView.RPC("Sequence_RPC", RpcTarget.AllBuffered, id);
    }

    [PunRPC]
    public void Sequence_RPC(int id)
    {
        if (sequenceNumber[0]==0) 
        {
            sequenceNumber[0] = id;

            spotters[0].GetComponent<Renderer>().material.color= Color.gray;
        }
        else if (sequenceNumber[1] == 0)
        {
            sequenceNumber[1] = id;

            spotters[1].GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (sequenceNumber[2] == 0)
        {
            sequenceNumber[2] = id;

            spotters[2].GetComponent<Renderer>().material.color = Color.gray;
        }
        else if (sequenceNumber[3] == 0)
        {
            sequenceNumber[3] = id;

            spotters[3].GetComponent<Renderer>().material.color = Color.gray;
            phView.RPC("Check", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void Check()
    {
        if (sequenceNumber[0]== correctSequence[0]
            && sequenceNumber[1]== correctSequence[1]
            && sequenceNumber[2] == correctSequence[2]
            && sequenceNumber[3] == correctSequence[3]) 
        {
            symbols[0].gameObject.GetComponent<Symbol>().gameObject.tag = "Nada";
            symbols[1].gameObject.GetComponent<Symbol>().gameObject.tag = "Nada";
            symbols[2].gameObject.GetComponent<Symbol>().gameObject.tag = "Nada";
            symbols[3].gameObject.GetComponent<Symbol>().gameObject.tag = "Nada";
            symbols[4].gameObject.GetComponent<Symbol>().gameObject.tag = "Nada";
            symbols[5].gameObject.GetComponent<Symbol>().gameObject.tag = "Nada";

            door.GetComponent<SymbolDoor>().OpenDoor();

            gameObject.GetComponent<SymbolControl>().enabled = false;

            spotters[0].GetComponent<Renderer>().material.color = Color.green;
            spotters[1].GetComponent<Renderer>().material.color = Color.green;
            spotters[2].GetComponent<Renderer>().material.color = Color.green;
            spotters[3].GetComponent<Renderer>().material.color = Color.green;
        }

        else 
        {
            sequenceNumber[0] = 0;
            sequenceNumber[1] = 0;
            sequenceNumber[2] = 0;
            sequenceNumber[3] = 0;

            //mostra q erro

            spotters[0].GetComponent<Renderer>().material.color = Color.red;
            spotters[1].GetComponent<Renderer>().material.color = Color.red;
            spotters[2].GetComponent<Renderer>().material.color = Color.red;
            spotters[3].GetComponent<Renderer>().material.color = Color.red;

            StartCoroutine("Redone");
            activeTimer = true;
        }
    }

    IEnumerator Redone()
    {
        yield return new WaitForSeconds(2);
        phView.RPC("WhiteBack_RPC", RpcTarget.AllBuffered);
    }
}
