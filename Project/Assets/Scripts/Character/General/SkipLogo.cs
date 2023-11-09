using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SkipLogo : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Skip"))
            SceneManager.LoadScene("MainMenu");
    }
}
