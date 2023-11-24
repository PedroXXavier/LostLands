using Assets.SimpleLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using System.Diagnostics;
using Assets.SimpleLocalization.Scripts;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public enum States {
    Play, Pause, Book, Win}

public class GameController : MonoBehaviour
{
    PhotonView phView;
    NoteTrigger noteTrigger; FragmentControl frag;

    public States states; 
    
    public bool cursor;

    [Header("Pause")]
    public GameObject pause;
    public bool pauseOn;

    [Header("Notebook")]
    public GameObject BrNotebook; public GameObject EnNotebook;
    GameObject atualBook; public bool noteOn;
    public AudioSource openBookSFX;

    [Header("Final")]
    public GameObject win;

    private void Start() {
        noteTrigger = FindObjectOfType(typeof(NoteTrigger)) as NoteTrigger;
        frag = FindObjectOfType(typeof(FragmentControl)) as FragmentControl;

        phView = GetComponent<PhotonView>();

        if(!phView.IsMine)
            gameObject.SetActive(false);
    }

    void Update()
    {
        if (LocalizationManager.Language == "English")
            atualBook = EnNotebook;
        else if(LocalizationManager.Language == "Portuguese")
            atualBook = BrNotebook;

        OtherPlayerDisconnect();

        if (cursor) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; }
        else if (!cursor) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; }

        if (!cursor && Input.GetButtonDown("Left Alt") && states == States.Play)
            cursor = true;
        else if (cursor && Input.GetButtonDown("Left Alt") && states == States.Play)
            cursor = false;

        switch (states)
        {
            case States.Play:
                Pause(); Notebook();
                break;
            case States.Pause:
                Pause();
                break;
            case States.Book:
                Notebook();
                break;
        }
    }

    public void Pause()
    {
        if (Input.GetButtonDown("P") && !pauseOn)
        {
            states = States.Pause;
            pause.SetActive(true);
            pauseOn = true;

            cursor = true;
        }
        else if (Input.GetButtonDown("P") && pauseOn)
        {
            states = States.Play;
            pause.SetActive(false);
            pauseOn = false;

            cursor = false;
        }
    }

    public void Notebook()
    {
        if (Input.GetButtonDown("B") && !noteOn)
        {
            openBookSFX.Play();

            states = States.Book;
            atualBook.SetActive(true);
            noteOn = true;

            cursor = true;
        }
        else if (Input.GetButtonDown("B") && noteOn)
        {
            atualBook.SetActive(false);
            noteOn = false; cursor = false;

            states = States.Play;

            if (noteTrigger.notificationOn)
                noteTrigger.notificationOn = false;
        }
    }

    private void OtherPlayerDisconnect()
    {
        if (PhotonNetwork.PlayerList.Length <= 1)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("MainMenu");
        }
    }

    public void CursorOff()
    {
        cursor = false;
    }

    public void Victory()
    {
        phView.RPC("Victory_RPC", RpcTarget.All);
    }
    [PunRPC]
    void Victory_RPC()
    {
        win.SetActive(true); cursor = true; states = States.Win;
    }

    public void FinalVoteYes()
    {
        frag.VoteYesButton();
    }

    public void FinalVoteCancel()
    {
        frag.VoteCancelButton();
    }
}
