using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using Photon.Realtime;
using System;

namespace Assets.Scripts.Handlers.Interfaces
{
	public interface IDataReceiver
	{
        public event Action<PlayerEnum> OnUpdatePlayerReadyStatus;
        public event Action<bool, bool> OnUpdatePlayerTurn;
        public event Action<byte[,]> OnSetFildData;
        public event Action<int, int> OnSetShootResult;
        public event Action<bool> OnShowGameResult;
        public void ReceivePlayerReadyStatus(int type);
        public void ReceivePlayerTurn(bool isFirstPlayer, bool isSecondPlayer);
        public void ReceivePlayerFildData(byte[] serializedData, int rows, int cols);
        public void ReceiveOpponentShoot(int x, int y);
        public void ReceiveEndGameStatus(bool isWin);
    } 
}
