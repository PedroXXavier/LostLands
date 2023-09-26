using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    public AudioSource notificationSound;

    public GameObject[] content;
    [SerializeField] bool[] activedContent;

    public GameObject notification;
    public bool notificationOn;

    private void Start()
    {
        activedContent= new bool[content.Length];
    }

    private void Update()
    {
        if(notificationOn)
            notification.SetActive(true);
        else
            notification.SetActive(false);

        if (activedContent[0]) //0
            content[0].SetActive(true);
        
        if (activedContent[1]) //1
            content[1].SetActive(true);

        if (activedContent[2]) //2
            content[2].SetActive(true);

        if (activedContent[3]) //3
            content[3].SetActive(true);

        if (activedContent[4]) //4
            content[4].SetActive(true);

        if (activedContent[5]) //5
            content[5].SetActive(true);

        if (activedContent[6]) //6
            content[6].SetActive(true);

        if (activedContent[7]) //7
            content[7].SetActive(true);

        if (activedContent[8]) //8
            content[8].SetActive(true);

        if (activedContent[9]) //9
            content[9].SetActive(true);

        if (activedContent[10]) //10
            content[10].SetActive(true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("NoteTrigger"))
        {
            if (!other.gameObject.GetComponent<NoteId>().actived)
            {
                activedContent[other.gameObject.GetComponent<NoteId>().id] = true;
                notificationOn = true;

                notificationSound.Play();
                other.gameObject.GetComponent<NoteId>().actived = true;
            }

        }
    }
}
