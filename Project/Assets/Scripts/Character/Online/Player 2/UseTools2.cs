using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public enum Tool2
{
    Hand, Shovel, MetalDet, Luneta
}

public class UseTools2 : MonoBehaviour
{
    GameController gc; SymbolControl symbol; PuzzleControl puzzle; FragmentControl fragments; NoteTrigger nt;
    PhotonView phView;

    public GameObject player;

    Tool2 tools;

    [Header("Amuleto")]
    public GameObject amu1; public GameObject amu2; public GameObject amu3;
    public GameObject silhueta; public GameObject notPreparedTxt;

    [Header("Hand")]
    public GameObject pressE;

    [Header("Paper")]
    public GameObject paperType1, paperType2; public TMP_Text paperText;

    [Header("Shovel")]
    public GameObject shovel; public GameObject handShovel;
    public Animator shovelAnim;

    public AudioSource hitShovelSFX;
    public AudioSource hitNothingSFX;

    [Header("Metal Detector")]
    public GameObject metalDet; public GameObject handMetalDet;

    [Header("Luneta")]
    public GameObject luneta; public GameObject hudLuneta; public GameObject zoomCam;
    public Animator lunetaAnim; int lunetaOnOff; public GameObject handLuneta;

    void Start() {
        phView = GetComponent<PhotonView>();

        gc = FindObjectOfType(typeof(GameController)) as GameController;
        nt = FindObjectOfType(typeof(NoteTrigger)) as NoteTrigger;
        symbol = FindObjectOfType(typeof(SymbolControl)) as SymbolControl;
        puzzle = FindObjectOfType(typeof(PuzzleControl)) as PuzzleControl;
        fragments = FindObjectOfType(typeof(FragmentControl)) as FragmentControl; }

    void Update()
    {
        if (phView.IsMine)
        {
            handMetalDet.SetActive(false);
            handLuneta.SetActive(false);
            handShovel.SetActive(false);
        }

        switch (gc.states) {
            case States.Play:
                TakeTools();
                UseTools();
                break;
            case States.Pause:
                ClosePaper();
                break; }

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

        else if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("FinalDoor"))
        {
            pressE.SetActive(true);

            if (Input.GetButtonDown("E"))
            {
                if (fragments.fragmentsCollected[0] && fragments.fragmentsCollected[1] && fragments.fragmentsCollected[2])
                {
                    ChangeToWin(); gc.Victory();
                }
                else
                    StartCoroutine("NotPreparedTxt");
            }
        } //Final door

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

                    if (hit.collider.gameObject.GetComponent<Chest>().type == 0)
                    {
                        hit.collider.gameObject.GetComponent<Chest>().fragmentActived = true;
                        fragments.CollectFragment(hit.collider.gameObject.GetComponent<Chest>().id);
                    }

                    else if (hit.collider.gameObject.GetComponent<Chest>().type == 1)
                        fragments.CollectGalinha();
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
                shovel.SetActive(false); luneta.SetActive(false); metalDet.SetActive(false);
                break;

            case Tool2.Shovel:
                shovel.SetActive(true); luneta.SetActive(false); 

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

            case Tool2.MetalDet:
                metalDet.SetActive(true);
                shovel.SetActive(false); luneta.SetActive(false);

                metalDet.GetComponent<MetalDet>().actived = true;
                break;

            case Tool2.Luneta:
                luneta.SetActive(true);
                shovel.SetActive(false); metalDet.SetActive(false);

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

    public void ChangeToWin()
    {
        gc.states = States.Win; gc.cursor = true;
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

                phView.RPC("Disable_Luneta_RPC", RpcTarget.AllBuffered);
                phView.RPC("Disable_MetalDet_RPC", RpcTarget.AllBuffered);
                tools = Tool2.Shovel;

                player.GetComponent<MetalDetTrigger>().distanceSounds[0].SetActive(false);
                player.GetComponent<MetalDetTrigger>().distanceSounds[1].SetActive(false);
                player.GetComponent<MetalDetTrigger>().distanceSounds[2].SetActive(false);
                player.GetComponent<MetalDetTrigger>().distanceSounds[3].SetActive(false);
            }
        }

        if (Input.GetButtonDown("Two")) //MetalDet
        {
            if (tools == Tool2.MetalDet)
            {
                tools = Tool2.Hand;

                metalDet.GetComponent<MetalDet>().actived = false;

                player.GetComponent<MetalDetTrigger>().distanceSounds[0].SetActive(false);
                player.GetComponent<MetalDetTrigger>().distanceSounds[1].SetActive(false);
                player.GetComponent<MetalDetTrigger>().distanceSounds[2].SetActive(false);
                player.GetComponent<MetalDetTrigger>().distanceSounds[3].SetActive(false);

                phView.RPC("Disable_MetalDet_RPC", RpcTarget.AllBuffered);
            }
            else
            {
                phView.RPC("Enable_MetalDet_RPC", RpcTarget.AllBuffered);

                phView.RPC("Disable_Shovel_RPC", RpcTarget.AllBuffered);
                phView.RPC("Disable_Luneta_RPC", RpcTarget.AllBuffered);
                tools = Tool2.MetalDet;
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
                phView.RPC("Enable_Luneta_RPC", RpcTarget.AllBuffered);

                phView.RPC("Disable_Shovel_RPC", RpcTarget.AllBuffered);
                phView.RPC("Disable_Compass_RPC", RpcTarget.AllBuffered);
                tools = Tool2.Luneta;

                player.GetComponent<MetalDetTrigger>().distanceSounds[0].SetActive(false);
                player.GetComponent<MetalDetTrigger>().distanceSounds[1].SetActive(false);
                player.GetComponent<MetalDetTrigger>().distanceSounds[2].SetActive(false);
                player.GetComponent<MetalDetTrigger>().distanceSounds[3].SetActive(false);
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
    private void Enable_MetalDet_RPC()
    {
        metalDet.SetActive(true);
    }
    [PunRPC]
    private void Disable_MetalDet_RPC()
    {
        metalDet.SetActive(false);
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

    IEnumerator Amuleto()
    {
        silhueta.SetActive(true);
        yield return new WaitForSeconds(5);
        silhueta.SetActive(false);
    }

    IEnumerator NotPreparedTxt()
    {
        notPreparedTxt.SetActive(true);
        yield return new WaitForSeconds(8);
        notPreparedTxt.SetActive(false);
    }
}
