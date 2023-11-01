using Assets.SimpleLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using System.Diagnostics;
using Assets.SimpleLocalization.Scripts;

public enum States {
    Play, Pause, Book, Win }

public class GameController : MonoBehaviour
{
    PhotonView phView;

    NoteTrigger noteTrigger;

    FragmentControl fragmentControl;
    public GameObject win;

    public AudioSource openBookSFX;

    public States states;

    [Header("Pause")]
    public GameObject pauseBg;
    public bool pauseOn;

    [Header("Notebook")]
    public GameObject BrNotebook; public GameObject EnNotebook;
    GameObject atualBook;
    public bool noteOn;

    public bool cursor;

    private void Start() {
        noteTrigger = FindObjectOfType(typeof(NoteTrigger)) as NoteTrigger;
        fragmentControl = FindObjectOfType(typeof(FragmentControl)) as FragmentControl;

        phView = GetComponent<PhotonView>();

        if (!phView.IsMine)
            gameObject.SetActive(false);
    }

    void Update()
    {
        if (LocalizationManager.Language == "English")
            atualBook = EnNotebook;
        else if(LocalizationManager.Language == "Portuguese")
            atualBook = BrNotebook;

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
            case States.Win:
                break;
        }

        ActicedWin();

        if (cursor) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; }
        else if (!cursor) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; }

        if (!cursor && Input.GetButtonDown("Left Alt") && states == States.Play)
        {
            cursor = true;
        }
        else if (cursor && Input.GetButtonDown("Left Alt") && states == States.Play)
        {
            cursor = false;
        }

        if(fragmentControl.allFrag)
        {
            cursor= true;
            states = States.Pause;
        }
    }

    private void ActicedWin()
    {
        if (fragmentControl.allFrag)
        {
            cursor = true;
            states = States.Win;

            win.SetActive(true);
        }
    }

    public void Pause()
    {
        if (Input.GetButtonDown("P") && !pauseOn)
        {
            states = States.Pause;
            pauseBg.SetActive(true);
            pauseOn = true;

            cursor = true;
        }
        else if (Input.GetButtonDown("P") && pauseOn)
        {
            states = States.Play;
            pauseBg.SetActive(false);
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
            states = States.Play;
            atualBook.SetActive(false);
            noteOn = false;

            cursor = false;

            if(noteTrigger.notificationOn)
                noteTrigger.notificationOn = false;
        }
    }

    public void CursorOff()
    {
        cursor = false;
    }
}
