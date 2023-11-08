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

    private void Update()
    {
        if(Input.GetButtonDown("Skip") && menu)
            inicialFade.SetActive(false);
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

    public void Quit()
    {
        Application.Quit();
    }
}
