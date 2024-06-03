using Assets.Scripts.Enums;
using Assets.Scripts.Handlers;
using Assets.Scripts.Handlers.Interfaces;
using Assets.Scripts.Managers.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class NetworkManager : MonoBehaviourPunCallbacks, INetworkManager
    {
        [SerializeField] private GameObject _player;

        private ILevelManager _levelManager;
        private IUIManager _uiManager;
        private IGameManager _gameManager;

        [Inject]
        private void Constructor(ILevelManager levelManager,
                                 IUIManager uiManager,
                                 IGameManager gameManager)
        {
            _levelManager = levelManager;
            _uiManager = uiManager;
            _gameManager = gameManager;

            //PhotonNetwork.ConnectUsingSettings();
        }

        //public override void OnConnectedToMaster()
        //{
        //    PhotonNetwork.JoinOrCreateRoom("Game Room 1", new Photon.Realtime.RoomOptions { MaxPlayers = 2 }, null);
        //}

        public override void OnJoinedRoom()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                _gameManager.ConnectPlayer(player);
            }
            StartCoroutine(WaitPlayer());
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _gameManager.ConnectPlayer(newPlayer);
        }

        private IEnumerator WaitPlayer()
        {
            while (PhotonNetwork.CurrentRoom.PlayerCount != 2)
            {
                yield return new WaitForSeconds(1);
            }
            //yield return new WaitForSeconds(0.1f);
            _uiManager.DisableWaitPanel();
            _levelManager.EnableShip();
        }
    } 
}
