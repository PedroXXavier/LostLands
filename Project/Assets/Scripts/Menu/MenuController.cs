using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void SinglePlayer()
    {
        SceneManager.LoadScene("ilha pequena");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
