using Assets.Scripts.Managers.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks, ILobbyManager
{
    [Header("Menus")]
    [SerializeField] private GameObject _createServerMenu;

    [Header("UI Component")]
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private Transform _serverList;

    [Header("Prefab")]
    [SerializeField] private GameObject _server;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CloseCreateServerMenu()
    {
        _createServerMenu.SetActive(false);
    }

    public void CreateServer()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinOrCreateRoom(_input.text, new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public void OpenCreateServerMenu()
    {
        _createServerMenu.SetActive(true);
    }

    public void OpenServer(string name)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinOrCreateRoom(name, new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (!room.RemovedFromList)
            {
                GameObject server = Instantiate(_server, _serverList);
                server.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
                server.GetComponent<Button>().onClick.AddListener(() => OpenServer(room.Name));
            }
        }
    }
}
