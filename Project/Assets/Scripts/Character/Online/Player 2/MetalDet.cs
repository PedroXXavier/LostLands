using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDet : MonoBehaviour
{
    public bool actived;

    public AudioSource[] distanceSounds;

    private void OnTriggerStay(Collider other)
    {
        if (actived)
        {
            if (other.gameObject.CompareTag("veryNearSFX"))
                distanceSounds[0].Play();

            else if (other.gameObject.CompareTag("nearSFX"))
                distanceSounds[1].Play();

            else if (other.gameObject.CompareTag("farSFX"))
                distanceSounds[2].Play();

            else if (other.gameObject.CompareTag("veryFarSFX"))
                distanceSounds[3].Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (actived)
        {
            if (other.gameObject.CompareTag("veryNearSFX"))
                distanceSounds[0].Stop();

            else if (other.gameObject.CompareTag("nearSFX"))
                distanceSounds[1].Stop();

            else if (other.gameObject.CompareTag("farSFX"))
                distanceSounds[2].Stop();

            else if (other.gameObject.CompareTag("veryFarSFX"))
                distanceSounds[3].Stop();
        }
    }
}