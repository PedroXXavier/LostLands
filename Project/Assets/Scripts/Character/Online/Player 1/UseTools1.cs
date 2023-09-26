using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public enum Tool1
{
    Hand, Pickaxe, Map
}

public class UseTools1 : MonoBehaviour
{
    GameController gc; SymbolControl symbol; PuzzleControl puzzle; FragmentControl fragments;
    PhotonView phView;

    Tool1 tools;

    [Header("Hand")]
    public GameObject pressE;

    [Header("Paper")]
    public GameObject paperHud; public TMP_Text paperText;

    [Header("Pickaxe")]
    public GameObject pickaxe; public GameObject handPickaxe;
    public Animator pickaxeAnim;

    public AudioSource hitPickaxeSFX;

    [Header("Map")]
    public GameObject map; public AudioSource openMapSFX;

    void Start() {
        phView = GetComponent<PhotonView>();

        gc = FindObjectOfType(typeof(GameController)) as GameController;
        symbol = FindObjectOfType(typeof(SymbolControl)) as SymbolControl;
        puzzle = FindObjectOfType(typeof(PuzzleControl)) as PuzzleControl;
        fragments = FindObjectOfType(typeof(FragmentControl)) as FragmentControl; }

    void Update()
    {
        switch (gc.states)
        {
            case States.Play:
                TakeTools();
                UseTool();
                break;
            case States.Pause:
                ClosePaper();
                break;
        }
    }

    void UseTool()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("HandInteract"))
        {
            pressE.SetActive(true);

            if (Input.GetButtonDown("E"))
            {
                gc.cursor = true;
                gc.states = States.Pause;

                paperHud.SetActive(true);
                paperText.text = hit.collider.gameObject.GetComponent<Paper>().content;
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
            case Tool1.Hand:
                pickaxe.SetActive(false); map.SetActive(false);
                break;

            case Tool1.Pickaxe:
                pickaxe.SetActive(true); map.SetActive(false);

                if (Input.GetButtonDown("Fire1"))
                    pickaxeAnim.SetTrigger("Trigger");
                if (Input.GetButtonDown("Fire1") && Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("PickaxeInteract"))
                {
                    hit.collider.SendMessage("Pickaxe");
                    hitPickaxeSFX.Play();
                }
                break;

            case Tool1.Map:
                map.SetActive(true); pickaxe.SetActive(false);
                break;
        }
    }

    void TakeTools()
    {
        if (Input.GetButtonDown("One")) //Pickaxe
        {
            if (tools == Tool1.Pickaxe)
            {
                tools = Tool1.Hand;
                phView.RPC("Disable_Pickaxe_RPC", RpcTarget.AllBuffered);
            }
            else
            {
                tools = Tool1.Pickaxe;
                phView.RPC("Enable_Pickaxe_RPC", RpcTarget.AllBuffered);
            }
        }
        if (Input.GetButtonDown("Three")) //Map
        {
            if (tools == Tool1.Map)
                tools = Tool1.Hand;
            else
            {
                tools = Tool1.Map;
                openMapSFX.Play();
            }
        }
    }

    [PunRPC]
    private void Enable_Pickaxe_RPC()
    {
        handPickaxe.SetActive(true);
    }

    [PunRPC]
    private void Disable_Pickaxe_RPC()
    {
        handPickaxe.SetActive(false);
    }

    void ClosePaper()
    {
        if(Input.GetButtonDown("E"))
        {
            paperHud.SetActive(false);

            gc.states = States.Play;
            gc.cursor = false;
        }
    }
}
