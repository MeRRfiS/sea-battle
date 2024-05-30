using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Installers
{
	public class GameInstaller : MonoInstaller
	{
        [Header("Managers")]
        [SerializeField] private OpponentFildManager _opponentManager;

        public override void InstallBindings()
        {
            BindOpponentManager();
        }

        private void BindOpponentManager()
        {
            Container.Bind<IOpponentFildManager>()
                                 .FromInstance(_opponentManager)
                                 .AsSingle();
        }
    } 
}
