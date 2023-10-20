using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{
    public GameObject inicialFade, historiaFade;
    [SerializeField] bool menu;
    public Animator creditAnimator;

    public GameObject skipText;
    bool canSkip;

    private void Update()
    {
        if(Input.GetButtonDown("SkipFade") && menu)
            inicialFade.SetActive(false);

        else if(Input.GetButtonDown("SkipFade") && !menu && !canSkip)
            StartCoroutine("ActiveSkip");

        else if (Input.GetButtonDown("SkipFade") && !menu && canSkip)
            StartCoroutine("FadeToMenu");
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
        yield return new WaitForSeconds(5);
        skipText.SetActive(false); canSkip= false;
    }

    IEnumerator FadeToMenu()
    {
        historiaFade.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
