using Assets.Scripts.Managers;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Handlers.Interfaces
{
	public interface IDataSender
	{
		public void SendPlayerReadyStatus(int type);
		public void SendPlayerTurn(bool isFirstPlayer = false, bool isSecondPlayer = false);
		public void SendPlayerFildData(CellManager[,] data);
		public void SendOpponentShoot(int x, int y);
		public void SendEndGameStatus(bool isWin);
    } 
}
