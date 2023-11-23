using Assets.SimpleLocalization;
using Assets.SimpleLocalization.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MultiLanguage : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        LocalizationManager.Read();

        switch(Application.systemLanguage) 
        { 
            case SystemLanguage.English:
                LocalizationManager.Language = "English";
                break;
            case SystemLanguage.Portuguese:
                LocalizationManager.Language = "Portuguese";
                break;
        }
    }

    public void ChangeLanguage(string language)
    {
        LocalizationManager.Language = language;
    }
}
