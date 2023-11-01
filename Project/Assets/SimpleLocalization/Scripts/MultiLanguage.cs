using Assets.SimpleLocalization;
using Assets.SimpleLocalization.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLanguage : MonoBehaviour
{
    private void Awake()
    {
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
