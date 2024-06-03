using Assets.Scripts.Enums;
using Assets.Scripts.Handlers;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Models;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
	public class GameManager : MonoBehaviour, IGameManager
	{
        private IUIManager _uiManager;
        private IOpponentFildManager _opponentManager;

        [SerializeField] private GameObject _player;

        [Header("Handlers")]
        [SerializeField] private DataSender _sender;
        [SerializeField] private DataReceiver _receiver;

        private bool _isGameReady = false;
		private Dictionary<PlayerEnum, PlayerManager> _playersList = new Dictionary<PlayerEnum, PlayerManager> ();

        [Inject]
        private void Constructor(IUIManager uiManager,
                                 IOpponentFildManager opponentManager)
        {
            _uiManager = uiManager;
            _opponentManager = opponentManager;

            _receiver.OnUpdatePlayerReadyStatus += UpdatePlayerReadyStatus;
            _receiver.OnUpdatePlayerTurn += ChangePlayerTurn;
            _receiver.OnSetFildData += SetUpOpponentFild;
            _receiver.OnSetShootResult += ShowDamageOnFild;
            _receiver.OnShowGameResult += EndGame;
        }

        public bool IsGameReady()
        {
            return _isGameReady;
        }

        public void ConnectPlayer(Player player)
        {
            PlayerEnum type;
            if (player.IsMasterClient)
            {
                type = PlayerEnum.FirstPlayer;
            }
            else
            {
                type = PlayerEnum.SecondPlayer;
            }
            if (_playersList.ContainsKey(type)) return;

            var newPlayer = PhotonNetwork.Instantiate(_player.name, Vector2.zero, Quaternion.identity);
            PlayerManager playerManager = newPlayer.GetComponent<PlayerManager>();

            playerManager.Type = type;
            _playersList.Add(type, playerManager);
            Debug.Log($"{type} is connected");
        }

        public void SetUpPlayerFild(CellManager[,] fild)
        {
            GetPlayer().PlayerFild = fild;
            _sender.SendPlayerFildData(fild);
        }

        public PlayerManager GetPlayer()
        {
            PlayerEnum type;
            if (PhotonNetwork.IsMasterClient)
            {
                type = PlayerEnum.FirstPlayer;
            }
            else
            {
                type = PlayerEnum.SecondPlayer;
            }

            return _playersList[type];
        }

        public PlayerManager GetPlayer(PlayerEnum type)
        {
            return _playersList[type];
        }

        public void CheckPlayersReady()
        {
            if (_playersList[PlayerEnum.FirstPlayer].IsReady)
            {
                Debug.Log("First player is ready!");
            }

            if (_playersList[PlayerEnum.SecondPlayer].IsReady)
            {
                Debug.Log("Second players is ready!");
            }

            if (_playersList[PlayerEnum.FirstPlayer].IsReady && _playersList[PlayerEnum.SecondPlayer].IsReady)
            {
                Debug.Log("Players are ready!");

                foreach (var player in _playersList.Values)
                {
                    player.OnMyTurn += _opponentManager.SetSelectedCell;
                    player.OnOpponentTurn += _opponentManager.RemoveSelectedCell;
                    player.OnShowCellStatus += _uiManager.ShowVisualCell;
                    player.OnGetCell += _opponentManager.ReturnCell;
                    player.OnGetFild += _opponentManager.ReturnFild;
                    player.OnGetDamage += _uiManager.ShowGetDamage;
                }

                if (PhotonNetwork.IsMasterClient)
                {
                    int randPlayer = Random.Range(1, 3);
                    switch ((PlayerEnum)randPlayer)
                    {
                        case PlayerEnum.FirstPlayer:
                            ChangePlayerTurn(isFirstPlayer: true);
                            _sender.SendPlayerTurn(isFirstPlayer: true);
                            break;
                        case PlayerEnum.SecondPlayer:
                            ChangePlayerTurn(isSecondPlayer: true);
                            _sender.SendPlayerTurn(isSecondPlayer: true);
                            break;
                    }
                    //ChangePlayerTurn(isFirstPlayer: true);
                    //_sender.SendPlayerTurn(isFirstPlayer: true);
                }

                _uiManager.EnableOpponentFild();
                _isGameReady = true;
            }
        }

        public void MakeShoot()
        {
            _sender.SendOpponentShoot(_opponentManager.GetX(), _opponentManager.GetY());

            if (GetOpponent().IsSetDamage())
            {
                if (GetOpponent().Health <= 0)
                {
                    EndGame(true);
                    _sender.SendEndGameStatus(false);
                }
                _opponentManager.ShowShoot(null);
                return;
            }

            _opponentManager.ShowShoot(SwitchPlayer);

            void SwitchPlayer()
            {
                bool isFirstPlayerTurn = GetPlayer().IsMyTurn;
                bool isSecondPlayerTurn = GetOpponent().IsMyTurn;
                if (PhotonNetwork.IsMasterClient)
                {
                    this.ChangePlayerTurn(!isFirstPlayerTurn, !isSecondPlayerTurn);
                    _sender.SendPlayerTurn(!isFirstPlayerTurn, !isSecondPlayerTurn);
                }
                else
                {
                    this.ChangePlayerTurn(!isSecondPlayerTurn, !isFirstPlayerTurn);
                    _sender.SendPlayerTurn(!isSecondPlayerTurn, !isFirstPlayerTurn);
                }
            }
        }

        private void UpdatePlayerReadyStatus(PlayerEnum type)
        {
            GetPlayer(type).IsReady = true;
            CheckPlayersReady();
        }

        private void ChangePlayerTurn(bool isFirstPlayer = false, bool isSecondPlayer = false)
        {
            _playersList[PlayerEnum.FirstPlayer].IsMyTurn = isFirstPlayer;
            _playersList[PlayerEnum.SecondPlayer].IsMyTurn = isSecondPlayer;

            if (PhotonNetwork.IsMasterClient)
            {
                _playersList[PlayerEnum.FirstPlayer].ChangeTurn();
            }
            else
            {
                _playersList[PlayerEnum.SecondPlayer].ChangeTurn();
            }
        }

        private void SetUpOpponentFild(byte[,] data)
        {
            CellModel[,] cellsInfo = new CellModel[data.GetLength(0), data.GetLength(1)];
            for (int i = 0; i < cellsInfo.GetLength(0); i++)
            {
                for (int j = 0; j < cellsInfo.GetLength(1); j++)
                {
                    cellsInfo[i, j] = new CellModel((CellModel.CellType)data[i, j]);
                }
            }
            _opponentManager.SetCellsInfo(cellsInfo);
        }

        private PlayerManager GetOpponent()
        {
            PlayerEnum type;
            if (PhotonNetwork.IsMasterClient)
            {
                type = PlayerEnum.SecondPlayer;
            }
            else
            {
                type = PlayerEnum.FirstPlayer;
            }

            return _playersList[type];
        }

        private void ShowDamageOnFild(int x, int y)
        {
            GetPlayer().GetDamage(x, y);
        }

        private void EndGame(bool isWin)
        {
            _isGameReady = false;
            if (isWin)
            {
                _uiManager.EnableWinPanel();
            }
            else
            {
                _uiManager.EnableLosePanel();
            }
        }

        private void OnDestroy()
        {
            _receiver.OnUpdatePlayerReadyStatus -= UpdatePlayerReadyStatus;
            _receiver.OnUpdatePlayerTurn -= ChangePlayerTurn;
            _receiver.OnSetFildData -= SetUpOpponentFild;
            _receiver.OnSetShootResult -= ShowDamageOnFild;
            _receiver.OnShowGameResult -= EndGame;
        }

        public void PlayerLeaveGame()
        {
            if(_isGameReady) _sender.SendEndGameStatus(true);

            PhotonNetwork.JoinLobby();
            PhotonNetwork.LoadLevel("Menu");
        }
    } 
}
