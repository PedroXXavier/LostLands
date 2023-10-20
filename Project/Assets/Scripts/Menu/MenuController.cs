using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{
    public GameObject inicialFade, historiaFade;
    float timer, maxTimer = 2; bool timerb;

    public Animator creditAnimator;

    private void Update()
    {
        if(Input.GetButtonDown("SkipFade"))
            inicialFade.SetActive(false);

        if(timerb)
        {
            timer += Time.deltaTime;

            if(timer >= maxTimer) 
            {
                SceneManager.LoadScene("cutcine");
                timerb = false;
            }
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
        historiaFade.SetActive(true);
        timerb = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
