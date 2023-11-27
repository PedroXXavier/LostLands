using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    public AudioSource notificationSound;
    PhotonView view;

    public GameObject[] contentPTBr, contentEN;
    public bool[] activedContent;

    public GameObject notification;
    public bool notificationOn;

    private void Start()
    {
        activedContent= new bool[contentPTBr.Length];

        view= GetComponent<PhotonView>();

        if(!view.IsMine) 
            gameObject.GetComponent<NoteTrigger>().enabled = false;
    }

    private void Update()
    {
        if(notificationOn)
            notification.SetActive(true);
        else
            notification.SetActive(false);

        if (activedContent[0]) //0
        {
            contentPTBr[0].SetActive(true); contentEN[0].SetActive(true);
        }

        if (activedContent[1]) //1
        {
            contentPTBr[1].SetActive(true); contentEN[1].SetActive(true);
        }

        if (activedContent[2]) //2
        {
            contentPTBr[2].SetActive(true); contentEN[2].SetActive(true);
        }

        if (activedContent[3]) //3
        {
            contentPTBr[3].SetActive(true); contentEN[3].SetActive(true);
        }

        if (activedContent[4]) //4
        {
            contentPTBr[4].SetActive(true); contentEN[4].SetActive(true);
        }

        if (activedContent[5]) //5
        {
            contentPTBr[5].SetActive(true); contentEN[5].SetActive(true);
        }

        if (activedContent[6]) //6
        {
            contentPTBr[6].SetActive(true); contentEN[6].SetActive(true);
        }

        if (activedContent[7]) //7
        {
            contentPTBr[7].SetActive(true); contentEN[7].SetActive(true);
        }

        if (activedContent[8]) //8
        {
            contentPTBr[8].SetActive(true); contentEN[8].SetActive(true);
        }

        if (activedContent[9]) //9
        {
            contentPTBr[9].SetActive(true); contentEN[9].SetActive(true);
        }

        if (activedContent[10]) //10
        {
            contentPTBr[10].SetActive(true); contentEN[10].SetActive(true);
        }

        if (activedContent[11]) //10
        {
            contentPTBr[11].SetActive(true); contentEN[11].SetActive(true);
        }

        if (activedContent[12]) //10
        {
            contentPTBr[12].SetActive(true); contentEN[12].SetActive(true);
        }

        if (activedContent[13]) //10
        {
            contentPTBr[13].SetActive(true); contentEN[13].SetActive(true);
        }

        if (activedContent[14]) //10
        {
            contentPTBr[14].SetActive(true); contentEN[14].SetActive(true);
        }

        if (activedContent[15]) //10
        {
            contentPTBr[15].SetActive(true); contentEN[15].SetActive(true);
        }

        if (activedContent[16]) //10
        {
            contentPTBr[16].SetActive(true); contentEN[16].SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("NoteTrigger"))
        {
            activedContent[other.gameObject.GetComponent<NoteId>().id] = true;

            if (!other.gameObject.GetComponent<NoteId>().actived)
            {
                notificationOn = true;

                notificationSound.Play();
                other.gameObject.GetComponent<NoteId>().actived = true;
            }
        }
    }
}
