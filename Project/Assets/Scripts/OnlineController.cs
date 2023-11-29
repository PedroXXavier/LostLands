using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class OnlineController : MonoBehaviourPunCallbacks
{
    [Header("Online")]
    public GameObject kitGameplay1;
    public GameObject kitGameplay2;

    public Transform playerSpawner1;
    public Transform playerSpawner2;

    public GameObject fragment;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate(kitGameplay1.name, playerSpawner1.position, playerSpawner1.transform.rotation, 0);
        else
            PhotonNetwork.Instantiate(kitGameplay2.name, playerSpawner2.position, playerSpawner2.transform.rotation, 0);
    }

    void OnDisconnected()
    {
        Destroy(fragment);
        Destroy(gameObject);
    }
}
