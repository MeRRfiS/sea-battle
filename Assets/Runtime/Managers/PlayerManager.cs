using Assets.Scripts.Enums;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Models;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private IOpponentFildManager _opponentManager;

        public event Action OnMyTurn;
        public event Action OnOpponentTurn;
        public event Action<int, int, CellModel.CellType> OnShowCellStatus;
        public event Func<CellManager> OnGetCell;

        [HideInInspector] public PlayerEnum Type;
        [HideInInspector] public CellManager[,] PlayerFild;
        [HideInInspector] public bool IsReady = false;
        [HideInInspector] public bool IsMyTurn = false;

        [HideInInspector] public int Health = 20;

        public void ChangeTurn()
        {
            if (IsMyTurn)
            {
                OnMyTurn?.Invoke();
            }
            else
            {
                OnOpponentTurn?.Invoke();
            }
        }

        public bool IsSetDamage()
        {
            bool result = false;
            CellManager cell = OnGetCell?.Invoke();

            switch (cell.Cell.Type)
            {
                case CellModel.CellType.Nothing:
                    cell.Cell.Type = CellModel.CellType.Miss;
                    break;
                case CellModel.CellType.Ship:
                    cell.Cell.Type = CellModel.CellType.Damage;
                    Health--;
                    result = true;
                    break;
                case CellModel.CellType.CantPlace:
                    cell.Cell.Type = CellModel.CellType.Miss;
                    break;
            }

            cell.CellColor();

            return result;
        }

        public void GetDamage(int x, int y)
        {
            CellManager cell = PlayerFild[x, y];
            switch (cell.Cell.Type)
            {
                case CellModel.CellType.Ship:
                    Health--;
                    break;
            }
            OnShowCellStatus?.Invoke(cell.X, cell.Y, cell.Cell.Type);
        }
    } 
}
