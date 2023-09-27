using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public enum Tool2
{
    Hand, Shovel, Compass, Luneta
}

public class UseTools2 : MonoBehaviour
{
    GameController gc; SymbolControl symbol; PuzzleControl puzzle; FragmentControl fragments;
    PhotonView phView;

    Tool2 tools;

    [Header("Hand")]
    public GameObject pressE;

    [Header("Paper")]
    public GameObject paperType1, paperType2; public TMP_Text paperText;

    [Header("Shovel")]
    public GameObject shovel; public GameObject handShovel;
    public Animator shovelAnim;

    public AudioSource hitShovelSFX;
    public AudioSource hitNothingSFX;

    [Header("Compass")]
    public GameObject compass; public GameObject hudCompass; public GameObject handCompass;

    [Header("Luneta")]
    public GameObject luneta; public GameObject hudLuneta; public GameObject zoomCam;
    public Animator lunetaAnim; int lunetaOnOff; public GameObject handLuneta;

    [Header("Win")]
    public GameObject winBg;


    void Start() {
        phView = GetComponent<PhotonView>();

        gc = FindObjectOfType(typeof(GameController)) as GameController;
        symbol = FindObjectOfType(typeof(SymbolControl)) as SymbolControl;
        puzzle = FindObjectOfType(typeof(PuzzleControl)) as PuzzleControl;
        fragments = FindObjectOfType(typeof(FragmentControl)) as FragmentControl; }

    void Update()
    {
        switch (gc.states) {
            case States.Play:
                TakeTools();
                UseTools();
                break;
            case States.Pause:
                ClosePaper();
                break; }
    }

    void UseTools()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("HandInteract"))
        {
            pressE.SetActive(true);

            if (Input.GetButtonDown("E"))
            {
                gc.cursor = true;
                gc.states = States.Pause;


                if (hit.collider.gameObject.GetComponent<Paper>().type == 1)
                {
                    paperType1.SetActive(true);                
                    paperText.text = hit.collider.gameObject.GetComponent<Paper>().content;
                }
                else if (hit.collider.gameObject.GetComponent<Paper>().type == 2)
                    paperType2.SetActive(true);
            }
        } //Paper

        else if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("ShovelInteract"))
        {
            if (hit.collider.gameObject.GetComponent<Chest>().life <= 0)
            {
                pressE.SetActive(true);

                if (Input.GetButtonDown("E"))
                {
                    hit.collider.SendMessage("OpenChest");

                    hit.collider.gameObject.GetComponent<Chest>().fragmentActived = true;
                    fragments.CollectFragment(hit.collider.gameObject.GetComponent<Chest>().id);
                }
            }
        }//Chest

        else if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("SymbolInteract"))
        {
            pressE.SetActive(true);

            if (Input.GetButtonDown("E"))
            {
                if (!hit.collider.GetComponent<Symbol>().isSelected)
                {
                    symbol.Sequence(hit.collider.GetComponent<Symbol>().id);
                    hit.collider.GetComponent<Symbol>().Press();
                }
            }
        } //Symbol

        else if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("PuzzleInteract"))
        {
            pressE.SetActive(true);

            if (Input.GetButtonDown("E"))
            {
                hit.collider.gameObject.GetComponent<PuzzlePiece>().Press();
                hit.collider.gameObject.GetComponent<PuzzlePiece>().Sequence();
            }
        } //Puzzle

        else
            pressE.SetActive(false);

        switch (tools)
        {
            case Tool2.Hand:
                shovel.SetActive(false); luneta.SetActive(false); compass.SetActive(false); hudCompass.SetActive(false);
                break;

            case Tool2.Shovel:
                shovel.SetActive(true);
                compass.SetActive(false); luneta.SetActive(false); hudCompass.SetActive(false);

                if (Input.GetButtonDown("Fire1"))
                {
                    shovelAnim.SetTrigger("Trigger");
                    hitNothingSFX.Play();
                }
                if (Input.GetButtonDown("Fire1") && Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("ShovelInteract"))
                {
                    hit.collider.SendMessage("Shovel");
                    hitShovelSFX.Play();
                }

                break;

            case Tool2.Compass:
                compass.SetActive(true); hudCompass.SetActive(true);
                shovel.SetActive(false); luneta.SetActive(false);
                break;

            case Tool2.Luneta:
                luneta.SetActive(true);
                shovel.SetActive(false); compass.SetActive(false); hudCompass.SetActive(false);

                if (Input.GetButtonDown("Fire2"))
                {
                    //hud com o bagulho da luneta, como se fosse mine
                    //coloca barulinho tb

                    if (lunetaOnOff == 0)
                    {
                        lunetaOnOff = 1;
                        hudLuneta.SetActive(true); zoomCam.SetActive(true);
                        lunetaAnim.SetTrigger("Trigger");
                    }
                    else
                    {
                        lunetaOnOff = 0;
                        hudLuneta.SetActive(false); zoomCam.SetActive(false);
                    }
                }
                break;
        }
    }
    void TakeTools()
    {
        if (Input.GetButtonDown("One")) //Shovel
        {
            if (tools == Tool2.Shovel)
            {
                tools = Tool2.Hand;
                phView.RPC("Disable_Shovel_RPC", RpcTarget.AllBuffered);
            }
            else
            {
                phView.RPC("Enable_Shovel_RPC", RpcTarget.AllBuffered);
                tools = Tool2.Shovel;
            }
        }

        if (Input.GetButtonDown("Two")) //Compass
        {
            if (tools == Tool2.Compass)
            {
                tools = Tool2.Hand;
                phView.RPC("Disable_Compass_RPC", RpcTarget.AllBuffered);
            }
            else
            {
                phView.RPC("Enable_Compass_RPC", RpcTarget.AllBuffered);
                tools = Tool2.Compass;
            }
        }

        if (Input.GetButtonDown("Three")) //Luneta
        {
            if (tools == Tool2.Luneta)
            {
                tools = Tool2.Hand;
                phView.RPC("Disable_Luneta_RPC", RpcTarget.AllBuffered);
            }
            else
            {
                phView.RPC("Disable_Luneta_RPC", RpcTarget.AllBuffered);
                tools = Tool2.Luneta;
            }
        }
    }

    [PunRPC]
    private void Enable_Shovel_RPC()
    {
        handShovel.SetActive(true);
    }
    [PunRPC]
    private void Disable_Shovel_RPC()
    {
        handShovel.SetActive(false);
    }


    [PunRPC]
    private void Enable_Luneta_RPC()
    {
        handLuneta.SetActive(true);
    }
    [PunRPC]
    private void Disable_Luneta_RPC()
    {
        handLuneta.SetActive(false);
    }


    [PunRPC]
    private void Enable_Compass_RPC()
    {
        handCompass.SetActive(true);
    }
    [PunRPC]
    private void Disable_Compass_RPC()
    {
        handCompass.SetActive(false);
    }

    void ClosePaper()
    {
        if (Input.GetButtonDown("E"))
        {
            paperType1.SetActive(false);
            paperType2.SetActive(false);

            gc.states = States.Play;
            gc.cursor = false;
        }
    }
}
