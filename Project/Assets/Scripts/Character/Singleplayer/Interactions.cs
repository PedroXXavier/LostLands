using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Interactions : MonoBehaviour
{
    public int type;

    GameController sGC;

    Animator anim;
    //public AudioSource source;

    [Header("Pickaxe & Shovel")]
    public int blockLife;

    [Header("Hand")]
    public int handType;

    [Header("Paper")]
    public string paperContent;
    public int paperOn = 0;

    [Header("Chest")]
    public bool chest;

    private void Start()
    {
        sGC = FindObjectOfType(typeof(GameController)) as GameController;

        anim = GetComponent<Animator>();
        //source = GetComponent<AudioSource>();

        if (type == 0)
            gameObject.tag = ("HandInteract");
        else if (type == 1)
            gameObject.tag = ("PickaxeInteract");
        else if (type == 2)
            gameObject.tag = ("ShovelInteract");
        else if (type == 3)
            chest = true;
    }

    public void Shovel(GameObject winBg)
    {
        blockLife--;

        //botar a animação e som do bloco quebrando
        //botar partícula
        //coisa pra aparecer pra td mundo

        if (blockLife == 2)
        {
            anim.SetBool("Stage1", true);
        }
        if (blockLife == 1)
        {
            anim.SetBool("Stage2", true);

            anim.SetBool("Stage1", false);

        }
        if (blockLife == 0)
        {
            anim.SetBool("Stage3", true);

            anim.SetBool("Stage1", false);
            anim.SetBool("Stage2", false);

            winBg.SetActive(true);
            sGC.states = States.Pause;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Pickaxe()
    {
        blockLife--;

        if(blockLife<=0)
        {
            Destroy(gameObject);
        }
    }
}
