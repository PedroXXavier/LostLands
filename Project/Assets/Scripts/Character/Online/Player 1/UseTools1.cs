using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public enum Tool1
{
    Hand, Pickaxe, Compass, Map
}

public class UseTools1 : MonoBehaviour
{
    GameController gc; SymbolControl symbol; PuzzleControl puzzle; FragmentControl fragments; NoteTrigger nt;
    PhotonView phView;

    Tool1 tools;

    [Header("Amuleto")]
    public GameObject amu1; public GameObject amu2; public GameObject amu3;
    public GameObject silhueta;

    [Header("Hand")]
    public GameObject pressE;

    [Header("Paper")]
    public GameObject paperType1, paperType2; public TMP_Text paperText;

    [Header("Pickaxe")]
    public GameObject pickaxe; public GameObject handPickaxe;
    public Animator pickaxeAnim;

    [Header("Compass")]
    public GameObject compass; public GameObject hudCompass; public GameObject handCompass;

    public AudioSource hitPickaxeSFX;
    public AudioSource hitNothingSFX;

    [Header("Map")]
    public GameObject map; public AudioSource openMapSFX;

    void Start() {
        phView = GetComponent<PhotonView>();

        gc = FindObjectOfType(typeof(GameController)) as GameController;
        nt = FindObjectOfType(typeof(NoteTrigger)) as NoteTrigger;
        symbol = FindObjectOfType(typeof(SymbolControl)) as SymbolControl;
        puzzle = FindObjectOfType(typeof(PuzzleControl)) as PuzzleControl;
        fragments = FindObjectOfType(typeof(FragmentControl)) as FragmentControl; }

    void Update()
    {
        if(phView.IsMine) 
            handPickaxe.SetActive(false);

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

        if (fragments.fragmentsCollected[0])
        {
            amu1.SetActive(true);
        }
        if (fragments.fragmentsCollected[1])
        {
            amu2.SetActive(true);
        }
        if (fragments.fragmentsCollected[2])
        {
            amu3.SetActive(true);
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

                if (hit.collider.gameObject.GetComponent<Paper>().type == 1)
                {
                    paperType1.SetActive(true);
                    paperText.text = hit.collider.gameObject.GetComponent<Paper>().content;
                }
                if (hit.collider.gameObject.GetComponent<Paper>().type == 2)
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

                    StartCoroutine("Amuleto");

                    nt.activedContent[hit.collider.gameObject.GetComponent<NoteId>().id] = true;
                    if (!hit.collider.gameObject.GetComponent<NoteId>().actived)
                    {
                        nt.notificationOn = true;

                        nt.notificationSound.Play();
                        hit.collider.gameObject.GetComponent<NoteId>().actived = true;
                    }

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
                pickaxe.SetActive(false); map.SetActive(false); compass.SetActive(false); hudCompass.SetActive(false);
                break;

            case Tool1.Pickaxe:
                pickaxe.SetActive(true); map.SetActive(false); compass.SetActive(false); hudCompass.SetActive(false);

                if (Input.GetButtonDown("Fire1")) {
                    pickaxeAnim.SetTrigger("Trigger");
                    hitNothingSFX.Play(); }
                if (Input.GetButtonDown("Fire1") && Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("PickaxeInteract")) {
                    hit.collider.GetComponent<Rock>().Pickaxe();
                    hitPickaxeSFX.Play(); }
                break;

            case Tool1.Compass:
                compass.SetActive(true); hudCompass.SetActive(true);
                pickaxe.SetActive(false); map.SetActive(false);
                break;

            case Tool1.Map:
                map.SetActive(true); pickaxe.SetActive(false); compass.SetActive(false); hudCompass.SetActive(false);
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

        if (Input.GetButtonDown("Two")) //Compass
        {
            if (tools == Tool1.Compass)
            {
                tools = Tool1.Hand;
                phView.RPC("Disable_Compass_RPC", RpcTarget.AllBuffered);
            }
            else
            {
                phView.RPC("Enable_Compass_RPC", RpcTarget.AllBuffered);

                phView.RPC("Disable_Pickaxe_RPC", RpcTarget.AllBuffered);
                tools = Tool1.Compass;
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
        if(Input.GetButtonDown("E"))
        {
            paperType1.SetActive(false);
            paperType2.SetActive(false);

            gc.states = States.Play;
            gc.cursor = false;
        }
    }

    IEnumerator Amuleto()
    {
        silhueta.SetActive(true);
        yield return new WaitForSeconds(5);
        silhueta.SetActive(false);
    }
}
