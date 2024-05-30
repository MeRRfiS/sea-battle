using Assets.Scripts.Handlers.Interfaces;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Handlers
{
    public class DataSender : MonoBehaviourPun, IDataSender
    {
        public void SendEndGameStatus(bool isWin)
        {
            photonView.RPC("ReceiveEndGameStatus", FindOpponentPlayer(), isWin);
        }

        public void SendOpponentShoot(int x, int y)
        {
            photonView.RPC("ReceiveOpponentShoot", FindOpponentPlayer(), x, y);
        }

        public void SendPlayerFildData(CellManager[,] cells)
        {
            byte[,] data = new byte[cells.GetLength(0), cells.GetLength(1)];
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j] = (byte)cells[i, j].Cell.Type;
                }
            }

            byte[] serializedData = SerializeByteMatrix(data);
            photonView.RPC("ReceivePlayerFildData", FindOpponentPlayer(), serializedData, data.GetLength(0), data.GetLength(1));
        }

        public void SendPlayerReadyStatus(int type)
        {
            photonView.RPC("ReceivePlayerReadyStatus", FindOpponentPlayer(), type);
        }

        public void SendPlayerTurn(bool isFirstPlayer = false, bool isSecondPlayer = false)
        {
            photonView.RPC("ReceivePlayerTurn", FindOpponentPlayer(), isFirstPlayer, isSecondPlayer);
        }

        private Player FindOpponentPlayer()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player != PhotonNetwork.LocalPlayer)
                {
                    return player;
                }
            }

            return null;
        }

        private byte[] SerializeByteMatrix(byte[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            byte[] serialized = new byte[rows * cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    serialized[i * cols + j] = matrix[i, j];
                }
            }

            return serialized;
        }
    }
}
