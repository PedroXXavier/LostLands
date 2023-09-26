using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Unity.VisualScripting;
using System;
using Photon.Realtime;
using Random = System.Random;
using TMPro;
using System.Linq;
using UnityEngine.UIElements;

public class NetworkController : MonoBehaviourPunCallbacks
{
    [Header("GO")]
    public GameObject loginGO;
    public GameObject partidasGO;
    public GameObject carregando;
    
    [Header("Player")]
    public TMP_InputField playerNameInput;
    string playerNameTemp;

    [Header("Room")]
    public TMP_InputField roomName;

    public string sceneName;

    void Start()
    {
        playerNameTemp = "Player: " + new Random().Next(1, 100) + "." + new Random().Next(1, 100);
        playerNameInput.text = playerNameTemp;

        roomName.text = "Room: " + new Random().Next(1, 100) + "." + new Random().Next(1, 100);

        PhotonNetwork.AutomaticallySyncScene = true;

        loginGO.SetActive(true);
        partidasGO.SetActive(false);
    }

    public void Login()
    {
        PhotonNetwork.ConnectUsingSettings();

        if (playerNameInput.text != "")
        {
            PhotonNetwork.NickName = playerNameInput.text;
        }
        else
        {
            PhotonNetwork.NickName = playerNameTemp;
        }
        loginGO.SetActive(false);
        carregando.SetActive(true);
    }

    public void BotaoBuscarPartidaRapida()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void BotaoCriarSala()
    {
        string roomTemp = "Room:" + new Random().Next(1, 1000) + "." + new Random().Next(1, 1000) + "." + new Random().Next(1, 1000);
        RoomOptions roomOptions = new RoomOptions() {MaxPlayers = 2};
        if (roomName.text != "")
        {
            roomTemp = roomName.text;
        }
        else
        {
            roomName.text = roomTemp;
        }
        PhotonNetwork.JoinOrCreateRoom(roomTemp, roomOptions, TypedLobby.Default);
    }
    public void BotaoVoltar()
    {
        PhotonNetwork.Disconnect();

        partidasGO.gameObject.SetActive(false);
        loginGO.gameObject.SetActive(true);
    }

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
        Debug.Log($"O Usuario: {PhotonNetwork.NickName} se conectou\n Ping: {PhotonNetwork.GetPing()}\n Server: {PhotonNetwork.CloudRegion}");
        partidasGO.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"O usuario: {PhotonNetwork.NickName} foi desconectado por causa: " + cause);

        PhotonNetwork.LoadLevel("MainMenu");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        Debug.Log($"O Master é: {PhotonNetwork.NickName} \nPing: {PhotonNetwork.GetPing()}");
        print("");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed");
        string roomTemp = "Room:" + new Random().Next(1, 1000) + "." + new Random().Next(1, 1000) + "." + new Random().Next(1, 1000);
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 2 };
        if (roomName.text != "")
        {
            roomTemp = roomName.text;
        }
        else
        {
            roomTemp = roomName.text;
        }
        
        PhotonNetwork.CreateRoom(roomTemp, roomOptions, TypedLobby.Default);
        Debug.Log(roomTemp);
    }

    public override void OnJoinedRoom()
    {
        //aqui é pra ter um if(host) recebe o kitgameplay 1 e se não o 2
        //fazer a aleatoriedade dos mapas qnd entra no lobby

        Debug.Log("OnJoinedRoom");
        PhotonNetwork.LoadLevel(sceneName);
        Debug.Log($"O Usuario: {PhotonNetwork.NickName} entrou na {PhotonNetwork.CurrentRoom}");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"O player: {newPlayer} entrou na sala {PhotonNetwork.CurrentRoom}");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log($"O player: {otherPlayer} saiu da sala {PhotonNetwork.CurrentRoom}");
    }
}