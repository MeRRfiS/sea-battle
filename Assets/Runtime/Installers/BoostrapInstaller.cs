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
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private PlayerInputManager _playerInputManager;

        public override void InstallBindings()
        {
            BindLevelManager();
            BindUIManager();
            BingPlayerInputManager();
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
