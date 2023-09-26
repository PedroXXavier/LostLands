using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpTrigger : MonoBehaviour
{
    [SerializeField] int type;
    public Transform tpIn, tpOut;

    private void OnCollisionEnter(Collision collision)
    {
        if(type== 0) //In
            if(collision.gameObject.CompareTag("Player"))
            {
                GameObject player = collision.gameObject;
                player.transform.position = tpIn.position;
            }


        if (type == 1) //Out
            if (collision.gameObject.CompareTag("Player"))
            {
                GameObject player = collision.gameObject;
                player.transform.position = tpOut.position;
            }
    }
}
