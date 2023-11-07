using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class AutoLeaveCutscene : MonoBehaviour
{
    public GameObject fade;

    void Start() {
        StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        fade.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
    }
}
