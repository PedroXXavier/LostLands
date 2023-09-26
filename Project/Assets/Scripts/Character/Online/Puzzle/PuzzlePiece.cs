using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using Unity.VisualScripting;

public class PuzzlePiece : MonoBehaviour
{
    public bool[] angulo;

    PhotonView phView; Animator anim;
    PuzzleControl puzzle;
    public int id;

    public int value = 1;
    public int correctId;

    void Start() {
        puzzle = FindObjectOfType(typeof(PuzzleControl)) as PuzzleControl;

        phView= GetComponent<PhotonView>();
        anim = GetComponent<Animator>();

        if (angulo[0])
        {
            anim.SetBool("0�", true);

            anim.SetBool("90�", false);
            anim.SetBool("180�", false);
            anim.SetBool("270�", false);
        }
        else if (angulo[1])
        {
            anim.SetBool("0�", false);

            anim.SetBool("90�", true);

            anim.SetBool("180�", false);
            anim.SetBool("270�", false);
        }
        else if (angulo[2])
        {
            anim.SetBool("0�", false);
            anim.SetBool("90�", false);

            anim.SetBool("180�", true);

            anim.SetBool("270�", false);

        }
        else if (angulo[3])
        {
            anim.SetBool("0�", false);
            anim.SetBool("90�", false);
            anim.SetBool("180�", false);

            anim.SetBool("270�", true);
        }
    }

    public void Sequence()
    {
        phView.RPC("Sequence_RPC", RpcTarget.AllBuffered);
    }
    
    [PunRPC]
    void Sequence_RPC()
    {
        if (value == 1)
        {
            value = 2;
        }

        else if (value == 2)
        {
            value = 3;
        }

        else if (value == 3)
        {
            value = 4;
        }

        else if (value == 4)
        {
            value = 1;
        }
    }

    public void Update()
    {
        phView.RPC("MakeTrue", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void MakeTrue()
    {
        if (value == correctId)
        {
            puzzle.sequenceNumber[id] = true;
        }
        else
            puzzle.sequenceNumber[id] = false;
    }

    public void Press()  {
        phView.RPC("Press_RPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Press_RPC()
    {
        if(anim.GetBool("0�")) 
        {
            anim.SetBool("0�", false); anim.SetBool("90�", true);
        }
        else if (anim.GetBool("90�"))
        {
            anim.SetBool("90�", false); anim.SetBool("180�", true);
        }
        else if (anim.GetBool("180�"))
        {
            anim.SetBool("180�", false); anim.SetBool("270�", true);
        }
        else if (anim.GetBool("270�"))
        {
            anim.SetBool("270�", false); anim.SetBool("0�", true);
        }
    }
}
