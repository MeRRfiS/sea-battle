using Assets.Scripts.Handlers;
using Assets.Scripts.Handlers.Interfaces;
using Assets.Scripts.Inputs;
using Assets.Scripts.Inputs.Interface;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class BoostrapInstaller : MonoInstaller
    {
        [Header("Managers")]
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private PlayerInputManager _playerInputManager;
        [SerializeField] private NetworkManager _networkManager;
        [SerializeField] private GameManager _gameManager;

        [Header("Handlers")]
        [SerializeField] private DataReceiver _receiver;
        [SerializeField] private DataSender _sender;

        public override void InstallBindings()
        {
            BindNetworkManager();
            BindUIManager();
            BingPlayerInputManager();
            BindLevelManager();
            BindDataSender();
            BindDataReceiver();
            BindGameManager();
        }

        private void BindGameManager()
        {
            Container.Bind<IGameManager>()
                            .FromInstance(_gameManager)
                            .AsSingle();
        }

        private void BindDataSender()
        {
            Container.Bind<IDataSender>()
                     .FromInstance(_sender)
                     .AsSingle();
        }

        private void BindDataReceiver()
        {
            Container.Bind<IDataReceiver>()
                     .FromInstance(_receiver)
                     .AsSingle();
        }

        private void BindNetworkManager()
        {
            Container.Bind<INetworkManager>()
                     .FromInstance(_networkManager)
                     .AsSingle();
        }

        private void BingPlayerInputManager()
        {
            Container.Bind<IPlayerInputManager>()
                     .FromInstance(_playerInputManager)
                     .AsSingle();
        }

        private void BindUIManager()
        {
            Container.Bind<IUIManager>()
                     .FromInstance(_uiManager)
                     .AsSingle();
        }

        private void BindLevelManager()
        {
            Container.Bind<ILevelManager>()
                     .FromInstance(_levelManager)
                     .AsSingle();
        }
    } 
}
