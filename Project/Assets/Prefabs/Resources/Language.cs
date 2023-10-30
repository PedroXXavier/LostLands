using Assets.SimpleLocalization;
using Assets.SimpleLocalization.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language : MonoBehaviour
{
    private void Awake()
    {
        LocalizationManager.Read();

        LocalizationManager.Language = "English";
    }
}
