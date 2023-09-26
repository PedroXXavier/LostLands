using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class ButtonController : MonoBehaviour
{
    public GameObject player;

    GameController gc;
    PhotonView phView;

    private void Start() {
        gc = FindObjectOfType(typeof(GameController)) as GameController; 
        phView = GetComponent<PhotonView>(); }

    public void StatesPlay() {
        gc.states = States.Play; }

    public void StatesPause() {
        gc.states = States.Pause; }

    //esse é pra qunado estiver dentro da cena do jogo

    public void BackMenu() {
        player.SetActive(false); //referencia do kit gameplay
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    public void Quit() {
        player.SetActive(false); //referencia do kit gameplay
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        Application.Quit();
        print("saiu"); }
}
