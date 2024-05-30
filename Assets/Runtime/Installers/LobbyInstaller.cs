using Assets.Scripts.Managers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class LobbyInstaller : MonoInstaller
    {
        [SerializeField] private LobbyManager _lobbyManager;

        public override void InstallBindings()
        {
            BindLobbyManager();
        }

        private void BindLobbyManager()
        {
            Container.Bind<ILobbyManager>()
                     .FromInstance(_lobbyManager)
                     .AsSingle();
        }
    } 
}
