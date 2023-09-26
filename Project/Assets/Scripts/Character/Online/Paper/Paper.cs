using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Paper : MonoBehaviour
{
    [Header("Content")] 
    public string content;

    void Start() {
        gameObject.tag = ("HandInteract"); }

}
