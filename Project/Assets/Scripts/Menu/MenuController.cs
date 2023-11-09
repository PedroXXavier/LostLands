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

    public void Page1()
    {
        creditAnimator.SetTrigger("Page1");
    }

    public void Page2()
    {
        creditAnimator.SetTrigger("Page2");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
