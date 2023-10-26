using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Notebook : MonoBehaviour
{
    public AudioSource turnPageSFX;

    [SerializeField] float pageSpeed = 0.5f;
    [SerializeField] List<Transform> pages;
    int index = -1; bool rotation;

    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject previousButton;

    GameController gc;

    private void Start()
    {
        InitialState();

        gc = FindObjectOfType(typeof(GameController)) as GameController;
    }
    public void InitialState()
    {
        for(int i=0; i < pages.Count; i++) 
        {
            pages[i].transform.rotation=Quaternion.identity;
        }
        pages[0].SetAsLastSibling();
        previousButton.SetActive(false);
    }

    public void NextPage()
    {
        if (rotation) { return; }
        index++;
        float angle = 180;
        NextButtonActions();
        pages[index].SetAsLastSibling();
        StartCoroutine(Rotate(angle, true));

        StartCoroutine("CantMove");

        turnPageSFX.Play();
    }

    void NextButtonActions () 
    {
        if (!previousButton.activeInHierarchy)
        {
            previousButton.SetActive(true);
        }
        if (index == pages.Count - 1)
        {
            nextButton.SetActive(false);
        }
    }

    public void PreviousPage()
    {
        if(rotation) { return; }
        float angle = 0;
        PreviousButtonActions();
        pages[index].SetAsLastSibling();
        StartCoroutine(Rotate(angle, false));

        StartCoroutine("CantMove");

        turnPageSFX.Play();
    }

    void PreviousButtonActions()
    {
        if (!nextButton.activeInHierarchy)
        {
            nextButton.SetActive(true);
        }
        if (index - 1 == -1)
        {
            previousButton.SetActive(false);
        }
    }

    IEnumerator Rotate(float angle, bool foward)
    {
        float value = 0f;

        while(true)
        {
            rotation = true;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            value += Time.deltaTime * pageSpeed;
            pages[index].rotation = Quaternion.Slerp(pages[index].rotation, targetRotation, value);
            float angle1 = Quaternion.Angle(pages[index].rotation, targetRotation);
            if(angle1 < 0.1f)
            {
                if (foward == false)
                {
                    index--;
                }
                rotation= false;
                break;
            }
            yield return null;
        }
    }

    IEnumerator CantMove()
    {
        gc.states = States.Pause;
        yield return new WaitForSeconds(0.5f);
        gc.states = States.Book;
    }
}
