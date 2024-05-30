using Assets.Scripts.Enums;
using Assets.Scripts.Handlers.Interfaces;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Handlers
{
    public class DataReceiver : MonoBehaviourPun, IDataReceiver
    {
        public event Action<PlayerEnum> OnUpdatePlayerReadyStatus;
        public event Action<bool, bool> OnUpdatePlayerTurn;
        public event Action<byte[,]> OnSetFildData;
        public event Action<int, int> OnSetShootResult;
        public event Action<bool> OnShowGameResult;

        [PunRPC]
        public void ReceiveEndGameStatus(bool isWin)
        {
            OnShowGameResult?.Invoke(isWin);
        }

        [PunRPC]
        public void ReceiveOpponentShoot(int x, int y)
        {
            OnSetShootResult?.Invoke(x, y);
        }

        [PunRPC]
        public void ReceivePlayerFildData(byte[] serializedData, int rows, int cols)
        {
            var data = DeserializeByteMatrix(serializedData, rows, cols);
            OnSetFildData?.Invoke(data);
        }

        [PunRPC]
        public void ReceivePlayerReadyStatus(int type)
        {
            OnUpdatePlayerReadyStatus?.Invoke((PlayerEnum)type);
        }

        [PunRPC]
        public void ReceivePlayerTurn(bool isFirstPlayer, bool isSecondPlayer)
        {
            OnUpdatePlayerTurn?.Invoke(isFirstPlayer, isSecondPlayer);
        }
        private byte[,] DeserializeByteMatrix(byte[] serializedMatrix, int rows, int cols)
        {
            byte[,] data = new byte[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] = serializedMatrix[i * cols + j];
                }
            }

            return data;
        }

    }
}
