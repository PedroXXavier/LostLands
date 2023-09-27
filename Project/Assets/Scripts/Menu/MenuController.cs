using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{
    public GameObject fade;

    public Animator creditAnimator;

    private void Update()
    {
        if(Input.GetButtonDown("SkipFade"))
            fade.SetActive(false);
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
        SceneManager.LoadScene("ilha pequena");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
