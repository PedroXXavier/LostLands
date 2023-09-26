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
            anim.SetBool("0º", true);

            anim.SetBool("90º", false);
            anim.SetBool("180º", false);
            anim.SetBool("270º", false);
        }
        else if (angulo[1])
        {
            anim.SetBool("0º", false);

            anim.SetBool("90º", true);

            anim.SetBool("180º", false);
            anim.SetBool("270º", false);
        }
        else if (angulo[2])
        {
            anim.SetBool("0º", false);
            anim.SetBool("90º", false);

            anim.SetBool("180º", true);

            anim.SetBool("270º", false);

        }
        else if (angulo[3])
        {
            anim.SetBool("0º", false);
            anim.SetBool("90º", false);
            anim.SetBool("180º", false);

            anim.SetBool("270º", true);
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
        if(anim.GetBool("0º")) 
        {
            anim.SetBool("0º", false); anim.SetBool("90º", true);
        }
        else if (anim.GetBool("90º"))
        {
            anim.SetBool("90º", false); anim.SetBool("180º", true);
        }
        else if (anim.GetBool("180º"))
        {
            anim.SetBool("180º", false); anim.SetBool("270º", true);
        }
        else if (anim.GetBool("270º"))
        {
            anim.SetBool("270º", false); anim.SetBool("0º", true);
        }
    }
}
