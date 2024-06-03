using Assets.Scripts.Enums;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Ships;
using System;
using System.Drawing;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private IOpponentFildManager _opponentManager;

        public event Action OnGetDamage;
        public event Action OnMyTurn;
        public event Action OnOpponentTurn;
        public event Action<int, int, CellModel.CellType> OnShowCellStatus;
        public event Func<CellManager> OnGetCell;
        public event Func<CellManager[,]> OnGetFild;

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

            if(cell.Cell.Type == CellModel.CellType.Damage)
                CheckDestroyShip(cell);

            return result;
        }

        public void GetDamage(int x, int y)
        {
            CellManager cell = PlayerFild[x, y];
            switch (cell.Cell.Type)
            {
                case CellModel.CellType.Ship:
                    Health--;
                    OnGetDamage?.Invoke();
                    break;
            }
            OnShowCellStatus?.Invoke(cell.X, cell.Y, cell.Cell.Type);
        }

        private void CheckDestroyShip(CellManager cell)
        {
            int startXPos = 0;
            int startYPos = 0;
            int shipSize = 1;
            Ship.Position position = Ship.Position.Horizontal;
            var shipModel = new Ship();
            var fild = OnGetFild?.Invoke();

            position = CheckShipPosition();
            FindStartShipPosition();
            shipSize = GetShipSize();

            switch (position)
            {
                case Ship.Position.Vertical:
                    for (int i = startXPos; i < startXPos + shipSize; i++)
                    {
                        if (fild[i, startYPos].Cell.Type == CellModel.CellType.Damage) continue;
                        return;
                    }
                    break;
                case Ship.Position.Horizontal:
                    for (int i = startYPos; i < startYPos + shipSize; i++)
                    {
                        if (fild[startXPos, i].Cell.Type == CellModel.CellType.Damage) continue;
                        return;
                    }
                    break;
            }

            switch (shipSize)
            {
                case 1:
                    shipModel = new OneCellShip();
                    break;
                case 2:
                    shipModel = new TwoCellShip();
                    break;
                case 3:
                    shipModel = new ThreeCellShip();
                    break;
                case 4:
                    shipModel = new FourCellShip();
                    break;
            }
            shipModel.ShipPosition = position;

            UpdateFild(shipModel);

            Ship.Position CheckShipPosition()
            {
                if (cell.X >= 0 && cell.X < 9)
                {
                    var checkCell = fild[cell.X + 1, cell.Y].Cell.Type;
                    if (checkCell == CellModel.CellType.Ship || checkCell == CellModel.CellType.Damage) return Ship.Position.Vertical;
                }
                if (cell.X > 0 && cell.X <= 9)
                {
                    var checkCell = fild[cell.X - 1, cell.Y].Cell.Type;
                    if (checkCell == CellModel.CellType.Ship || checkCell == CellModel.CellType.Damage) return Ship.Position.Vertical;
                }
                if (cell.Y >= 0 && cell.Y < 9)
                {
                    var checkCell = fild[cell.X, cell.Y + 1].Cell.Type;
                    if (checkCell == CellModel.CellType.Ship || checkCell == CellModel.CellType.Damage) return Ship.Position.Horizontal;
                }
                if (cell.Y > 0 && cell.Y <= 9)
                {
                    var checkCell = fild[cell.X, cell.Y - 1].Cell.Type;
                    if (checkCell == CellModel.CellType.Ship || checkCell == CellModel.CellType.Damage) return Ship.Position.Horizontal;
                }

                return Ship.Position.Horizontal;
            }
            void FindStartShipPosition()
            {
                switch (position)
                {
                    case Ship.Position.Vertical:
                        startYPos = cell.Y;
                        if (cell.X > 0 && cell.X <= 9)
                        {
                            startXPos = cell.X;
                            while (fild[startXPos - 1, startYPos].Cell.Type != CellModel.CellType.CantPlace &&
                                   fild[startXPos - 1, startYPos].Cell.Type != CellModel.CellType.Miss)
                            {
                                startXPos--;
                                if (startXPos == 0) break;
                            }
                        }
                        else
                        {
                            startXPos = 0;
                        }
                        break;
                    case Ship.Position.Horizontal:
                        startXPos = cell.X;
                        if (cell.Y > 0 && cell.Y <= 9)
                        {
                            startYPos = cell.Y;
                            while (fild[startXPos, startYPos - 1].Cell.Type != CellModel.CellType.CantPlace &&
                                   fild[startXPos, startYPos - 1].Cell.Type != CellModel.CellType.Miss)
                            {
                                startYPos--;
                                if (startYPos == 0) break;
                            }
                        }
                        else
                        {
                            startYPos = 0;
                        }
                        break;
                }
            }
            int GetShipSize()
            {
                switch (position)
                {
                    case Ship.Position.Vertical:
                        int xPos = startXPos;
                        if (xPos == 9) break;
                        while (fild[xPos + 1, startYPos].Cell.Type != CellModel.CellType.CantPlace &&
                               fild[xPos + 1, startYPos].Cell.Type != CellModel.CellType.Miss)
                        {
                            shipSize++;
                            xPos++;
                            if (xPos == 9) break;
                        }
                        break;
                    case Ship.Position.Horizontal:
                        int yPos = startYPos;
                        if (yPos == 9) break;
                        while (fild[startXPos, yPos + 1].Cell.Type != CellModel.CellType.CantPlace &&
                               fild[startXPos, yPos + 1].Cell.Type != CellModel.CellType.Miss)
                        {
                            shipSize++;
                            yPos++;
                            if (yPos == 9) break;
                        }
                        break;
                }

                return shipSize;
            }
            void UpdateFild(Ship ship)
            {
                byte[,] data = ship.ShipData();

                int maxDataX = data.GetLength(0);
                int maxDataY = data.GetLength(1);

                int startDataY = startYPos == 0 ? 1 : 0;
                int endDataY = maxDataY;
                int startDataX = startXPos == 0 ? 1 : 0;
                int endDataX = maxDataX;

                switch (ship.ShipPosition)
                {
                    case Ship.Position.Horizontal:
                        endDataY = startYPos == (10 - 1) - (ship.CellCount - 1) ?
                                   maxDataY - 1 : maxDataY;

                        endDataX = startXPos == (10 - 1) ? maxDataX - 1 : maxDataX;
                        break;
                    case Ship.Position.Vertical:
                        endDataY = startYPos == (10 - 1) ? maxDataY - 1 : maxDataY;

                        endDataX = startXPos == (10 - 1) - (ship.CellCount - 1) ?
                                       maxDataX - 1 : maxDataX;
                        break;
                }
                byte[,] formedData = ToFormData(data, startDataY, endDataY, startDataX, endDataX);
                UpdateCellsColor(formedData);
            }
            void UpdateCellsColor(byte[,] formedData)
            {
                int startX = startXPos == 0 ? startXPos : startXPos - 1;
                int startY = startYPos == 0 ? startYPos : startYPos - 1;

                for (int i = 0; i < formedData.GetLength(0); i++)
                {
                    for (int j = 0; j < formedData.GetLength(1); j++)
                    {
                        if (formedData[i, j] == 2)
                        {
                            fild[startX + i, startY + j].Cell.Type = CellModel.CellType.Miss;
                            fild[startX + i, startY + j].CellColor();
                        }
                    }
                }
            }
            byte[,] ToFormData(byte[,] data, int startDataY, int endDataY, int startDataX, int endDataX)
            {
                byte[,] formedData = new byte[endDataX - startDataX, endDataY - startDataY];
                for (int i = 0; i < endDataX - startDataX; i++)
                {
                    for (int j = 0; j < endDataY - startDataY; j++)
                    {
                        formedData[i, j] = data[startDataX + i, startDataY + j];
                    }
                }

                return formedData;
            }
        }
    }
}
