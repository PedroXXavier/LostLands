using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{
    [Header("Inicial Fade")]

    public GameObject inicialFade, historiaFade;
    [SerializeField] bool menu;
    public Animator creditAnimator;

    public GameObject skipText;
    bool canSkip;

    [Header("Online Skip")]

    public GameObject showChose;
    int playerChose;

    private void Update()
    {
        if(Input.GetButtonDown("SkipFade") && menu)
            inicialFade.SetActive(false);

        else if(Input.GetButtonDown("SkipFade") && !menu && !canSkip)
            StartCoroutine("ActiveSkip");

        else if (Input.GetButtonDown("SkipFade") && !menu && canSkip)
            StartCoroutine("FadeToMenu");


        if (playerChose == 2)
        {
            //fechar cutscene pros dois
        }
    }

    public void Page1()
    {
        creditAnimator.SetTrigger("Page1");
    }

    public void Page2()
    {
        creditAnimator.SetTrigger("Page2");
    }

    public void SinglePlayer()
    {
        StartCoroutine("FadeToPlay");
    }

    IEnumerator FadeToPlay()
    {
        historiaFade.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("cutcine");
    }

    IEnumerator ActiveSkip()
    {
        skipText.SetActive(true); canSkip= true;
        yield return new WaitForSeconds(4);
        skipText.SetActive(false); canSkip= false;
    }

    IEnumerator FadeToMenu()
    {
        historiaFade.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
    }


    public void SkipButton()
    {
        StartCoroutine("ShowChose");

        if (playerChose == 0)
            playerChose = 1;
        else if (playerChose == 1)
            playerChose = 2;
    }

    IEnumerator ShowChose()
    {
        showChose.SetActive(true);
        yield return new WaitForSeconds(4);
        showChose.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
