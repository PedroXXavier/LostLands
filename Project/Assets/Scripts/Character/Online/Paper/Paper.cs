using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Paper : MonoBehaviour
{
    [Header("Content")] 
    public string content;

    public int type;
    public TMP_Text localizedText;

    void Start() {
        gameObject.tag = ("HandInteract");
    }

    private void Update()
    {
        content = localizedText.text;
    }
}
