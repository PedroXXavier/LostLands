using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Tool
{
    Hand, Pickaxe, Shovel, Compass, Luneta, Map
}

public class UseTools : MonoBehaviour
{
    GameController gc;

    Tool tools;

    [Header("Hand")]
    public GameObject pressE;

    [Header("Paper")]
    public GameObject paperHud; public TMP_Text paperText;
    int paperOn;

    [Header("Pickaxe")]
    public GameObject pickaxe; //public GameObject handPickaxe;
    public Animator pickaxeAnim;

    [Header("Shovel")]
    public GameObject shovel; //public GameObject handShovel;
    public Animator shovelAnim;

    [Header("Compass")]
    public GameObject compass; public GameObject hudCompass;

    [Header("Luneta")]
    public GameObject luneta; public GameObject hudLuneta;
    public Animator lunetaAnim; int lunetaOnOff;

    [Header("Map")]
    public GameObject map;

    [Header("Win")]
    public GameObject winBg;

    void Start()
    {
        gc = FindObjectOfType(typeof(GameController)) as GameController;
    }

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

        if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("HandInteract")) //Paper
        {
            pressE.SetActive(true);

            if (Input.GetButtonDown("E") && hit.collider.GetComponent<Interactions>().handType == 1 && hit.collider.GetComponent<Interactions>().paperOn == 0)
            {
                paperText.text = hit.collider.GetComponent<Interactions>().paperContent;
                paperHud.SetActive(true);

                gc.states = States.Pause;
            }
        }
        else
        {
            pressE.SetActive(false);
        }

        switch (tools)
        {
            case Tool.Hand: //Hand
                pickaxe.SetActive(false); shovel.SetActive(false); compass.SetActive(false); hudCompass.SetActive(false); map.SetActive(false); hudLuneta.SetActive(false); luneta.SetActive(false);
                break;

            case Tool.Pickaxe: //Pickaxe
                pickaxe.SetActive(true); 
                map.SetActive(false); compass.SetActive(false); luneta.SetActive(false); hudCompass.SetActive(false); hudLuneta.SetActive(false); shovel.SetActive(false);

                if (Input.GetButtonDown("Fire1"))
                    pickaxeAnim.SetTrigger("Trigger");
                if (Input.GetButtonDown("Fire1") && Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("PickaxeInteract"))
                {
                    hit.collider.SendMessage("Pickaxe");

                    /*hit.collider.GetComponent<Interactions>().blockLife--;

                    if (hit.collider.gameObject.GetComponent<Interactions>().blockLife <= 0)
                        Pickaxe(hit.collider);*/
                }
                break;

            case Tool.Shovel:
                shovel.SetActive(true);
                pickaxe.SetActive(false); compass.SetActive(false); luneta.SetActive(false); hudCompass.SetActive(false); hudLuneta.SetActive(false); map.SetActive(false);

                if (Input.GetButtonDown("Fire1"))
                    shovelAnim.SetTrigger("Trigger");
                if (Input.GetButtonDown("Fire1") && Physics.Raycast(transform.position, transform.forward, out hit, 2.5f) && hit.collider.CompareTag("ShovelInteract"))
                {
                    hit.collider.SendMessage("Shovel", winBg);

                    /*hit.collider.GetComponent<Interactions>().blockLife--;

                    if (hit.collider.gameObject.GetComponent<Interactions>().blockLife <= 0)
                        Pickaxe(hit.collider);*/
                }
                break;

            case Tool.Compass:
                compass.SetActive(true); hudCompass.SetActive(true);
                shovel.SetActive(false); luneta.SetActive(false); pickaxe.SetActive(false); map.SetActive(false); hudLuneta.SetActive(false);
                break;

            case Tool.Luneta: //Luneta
                luneta.SetActive(true);
                shovel.SetActive(false); compass.SetActive(false); hudCompass.SetActive(false); pickaxe.SetActive(false); map.SetActive(false);

                if (Input.GetButtonDown("Fire2"))
                {
                    //hud com o bagulho da luneta, como se fosse mine
                    //coloca barulinho tb

                    if (lunetaOnOff == 0)
                    {
                        lunetaOnOff = 1;
                        hudLuneta.SetActive(true);
                        lunetaAnim.SetTrigger("Trigger");
                    }
                    else
                    {
                        lunetaOnOff = 0;
                        hudLuneta.SetActive(false);
                    }
                }
                break;

            case Tool.Map:
                map.SetActive(true); 
                pickaxe.SetActive(false); hudLuneta.SetActive(false); compass.SetActive(false); luneta.SetActive(false); hudCompass.SetActive(false); shovel.SetActive(false);
                break;
        }
    }

    void TakeTools()
    {
        if (Input.GetButtonDown("One")) //Pickaxe
        {
            if (tools == Tool.Pickaxe)
                tools = Tool.Hand;
            else
                tools = Tool.Pickaxe;
        }

        if (Input.GetButtonDown("Two")) //Shovel
        {
            if (tools == Tool.Shovel)
                tools = Tool.Hand;
            else
                tools = Tool.Shovel;
        }

        if (Input.GetButtonDown("Three")) //Map
        {
            if (tools == Tool.Map)
                tools = Tool.Hand;
            else
                tools = Tool.Map;
        }

        if (Input.GetButtonDown("Four")) //Compass
        {
            if (tools == Tool.Compass)
                tools = Tool.Hand;
            else
                tools = Tool.Compass;
        }

        if (Input.GetButtonDown("Five")) //Luneta
        {
            if (tools == Tool.Luneta)
                tools = Tool.Hand;
            else
                tools = Tool.Luneta;
        }
    }

    void ClosePaper()
    {
        if (Input.GetButtonDown("Esc"))
        {
            paperText.text = "";
            paperHud.SetActive(false);

            gc.states = States.Play;
        }
    }
}
