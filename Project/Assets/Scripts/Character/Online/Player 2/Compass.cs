using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform north;
    Vector3 dir;

    void Update()
    {
        dir.z = north.transform.eulerAngles.y;
        transform.localEulerAngles = dir;
    }
}
