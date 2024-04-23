using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLauncher : MonoBehaviourPunCallbacks
{
    private const string gameVersion = "1";
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailer. Create new room");
        PhotonNetwork.CreateRoom("", new Photon.Realtime.RoomOptions { MaxPlayers = 2});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");
        SceneManager.LoadScene(1);
    }
}
