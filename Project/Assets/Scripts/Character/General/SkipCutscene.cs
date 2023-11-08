using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class SkipCutscene : MonoBehaviour
{
    PhotonView phview;

    [Header("Online Skip")]

    public GameObject numberText; 
    public GameObject skipButton;
    public GameObject showChose; 
    public GameObject fade; 

    [SerializeField] int playerChose; 
    
    [SerializeField] bool canSpace, canSkip;

    void Start() {
        phview= GetComponent<PhotonView>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }

    void Update()
    {
        if (Input.GetButtonUp("Skip") && canSpace && !canSkip)
            StartCoroutine("CursorLol");

        if (playerChose == 2)
            StartCoroutine("FadeToPlay");

        if (Input.GetButtonUp("Skip") && canSpace && !canSkip)
            StartCoroutine("CanSkipTrue");
    }

    public void SkipButton()
    {
        if(canSkip) 
            StartCoroutine("ShowChose");
    }

    IEnumerator CanSkipTrue()
    {
        skipButton.SetActive(true);
        canSpace = false; canSkip= true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        yield return new WaitForSeconds(8);

        skipButton.SetActive(false);
        canSpace = true; canSkip = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator ShowChose()
    {
        phview.RPC("OpenNumber", RpcTarget.AllBuffered);

        skipButton.SetActive(false);
        canSkip = false; canSpace= false;

        yield return new WaitForSeconds(10);

        phview.RPC("CloseNumber", RpcTarget.AllBuffered);
        phview.RPC("ZeroNumber", RpcTarget.AllBuffered);
        canSpace = true;
    }

    IEnumerator FadeToPlay()
    {
        phview.RPC("Fade_RPC", RpcTarget.All);
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("mapa");
    }


    [PunRPC]
    private void Fade_RPC()
    {
        fade.SetActive(true);
    }

    [PunRPC]
    private void ZeroNumber()
    {
        playerChose = 0;
    }

    [PunRPC]
    private void OpenNumber()
    {
        showChose.SetActive(true);

        if (playerChose == 0)
        {
            playerChose = 1; numberText.GetComponent<TextMeshProUGUI>().text = "1/2";
        }
        else if (playerChose == 1)
        {
            playerChose = 2; numberText.GetComponent<TextMeshProUGUI>().text = "2/2";
        }
    }

    [PunRPC]
    private void CloseNumber()
    {
        showChose.SetActive(false);
    }

    public void BackMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    IEnumerator CursorLol()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        yield return new WaitForSeconds(8);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void FadeToMenuButton()
    {
        StartCoroutine("FadeToMenu");
    }

    IEnumerator FadeToMenu()
    {
        fade.SetActive(true);
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("MainMenu");
    }
}
