using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDetTrigger : MonoBehaviour
{
    public GameObject metalDet;
    public GameObject[] distanceSounds;
    bool[] poggers;

    private void OnTriggerStay(Collider other)
    {
        if (metalDet.GetComponent<MetalDet>().actived)
        {
            if (other.gameObject.CompareTag("veryNearSFX"))
            {
                distanceSounds[0].SetActive(true);

/*                distanceSounds[1].SetActive(false);
                distanceSounds[2].SetActive(false);
                distanceSounds[3].SetActive(false);*/

            }

            else if (other.gameObject.CompareTag("nearSFX"))
            {
                distanceSounds[1].SetActive(true);
/*
                distanceSounds[0].SetActive(false);
                distanceSounds[2].SetActive(false);
                distanceSounds[3].SetActive(false);*/
            }

            else if (other.gameObject.CompareTag("farSFX"))
            {
                distanceSounds[2].SetActive(true);
/*
                distanceSounds[0].SetActive(false);
                distanceSounds[1].SetActive(false);
                distanceSounds[3].SetActive(false);*/
            }

            else if (other.gameObject.CompareTag("veryFarSFX"))
            {
                distanceSounds[3].SetActive(true);

/*                distanceSounds[0].SetActive(false);
                distanceSounds[1].SetActive(false);
                distanceSounds[2].SetActive(false);*/
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (metalDet.GetComponent<MetalDet>().actived)
        {
            if (other.gameObject.CompareTag("veryNearSFX"))
                distanceSounds[0].SetActive(false);

            else if (other.gameObject.CompareTag("nearSFX"))
                distanceSounds[1].SetActive(false);

            else if (other.gameObject.CompareTag("farSFX"))
                distanceSounds[2].SetActive(false);

            else if (other.gameObject.CompareTag("veryFarSFX"))
            distanceSounds[3].SetActive(false);
        }
    }
}
