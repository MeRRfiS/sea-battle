using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Ships;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        private IUIManager _uiManager;

        private const int X_SIZE = 10;
        private const int Y_SIZE = 10;

        private List<BoxCollider2D> _shipsCollider = new List<BoxCollider2D>();
        private CellManager _targetCell;

        public CellManager[,] Cells = new CellManager[X_SIZE, Y_SIZE];

        [Inject]
        private void Contructor(IUIManager uiManager)
        {
            _uiManager = uiManager;

            foreach (var ship in FindObjectsOfType<ShipManager>())
            {
                _shipsCollider.Add(ship.GetComponent<BoxCollider2D>());
            }

            foreach (var cell in FindObjectsOfType<CellManager>())
            {
                Cells[cell.X, cell.Y] = cell;
            }
        } 

        public void PlacingShip(ShipManager shipManager)
        {
            _targetCell = Cells[0, 0];
            _uiManager.PlaceShipOnGrid(shipManager.transform);
        }

        public void DisableShip()
        {
            foreach (var ship in _shipsCollider)
            {
                ship.enabled = false;
            }
        }

        public void EnableShip()
        {
            foreach (var ship in _shipsCollider)
            {
                if (ship.GetComponent<ShipManager>().Ship.IsPlaced) continue;

                ship.enabled = true;
            }
        }

        public void ChangeTargetCell(Vector2 position)
        {
            int newY = _targetCell.Y + (int)position.x;
            int newX = _targetCell.X - (int)position.y;

            int x = newX < 0 || newX > (X_SIZE - 1) ? _targetCell.X : newX;
            int y = newY < 0 || newY > (Y_SIZE - 1) ? _targetCell.Y : newY;

            _targetCell = Cells[x, y];
        }

        public void ChangeTargetCell(int x, int y)
        {
            int newX = y == -1 ? _targetCell.X : y;
            int newY = x == -1 ? _targetCell.Y : x;

            _targetCell = Cells[newX, newY];
        }

        public void UpdateFild(Ship ship)
        {
            byte[,] data = ship.ShipData();

            int maxDataX = data.GetLength(0);
            int maxDataY = data.GetLength(1);

            int startDataY = _targetCell.Y == 0 ? 1 : 0;
            int endDataY = maxDataY;
            int startDataX = _targetCell.X == 0 ? 1 : 0;
            int endDataX = maxDataX;

            switch (ship.ShipPosition)
            {
                case Ship.Position.Horizontal:
                    endDataY = _targetCell.Y == (Y_SIZE - 1) - (ship.CellCount - 1) ?
                               maxDataY - 1 : maxDataY;

                    endDataX = _targetCell.X == (X_SIZE - 1) ? maxDataX - 1 : maxDataX;
                    break;
                case Ship.Position.Vertical:
                    endDataY = _targetCell.Y == (Y_SIZE - 1) ? maxDataY - 1 : maxDataY;

                    endDataX = _targetCell.X == (X_SIZE - 1) - (ship.CellCount - 1) ?
                                   maxDataX - 1 : maxDataX;
                    break;
            }
            byte[,] formedData = ToFormData(data, startDataY, endDataY, startDataX, endDataX);
            UpdateCellsType(formedData);
        }

        private void UpdateCellsType(byte[,] formedData)
        {
            int startX = _targetCell.X == 0 ? _targetCell.X : _targetCell.X - 1;
            int startY = _targetCell.Y == 0 ? _targetCell.Y : _targetCell.Y - 1;

            for (int i = 0; i < formedData.GetLength(0); i++)
            {
                for (int j = 0; j < formedData.GetLength(1); j++)
                {
                    Cells[startX + i, startY + j].Cell.Type = (CellModel.CellType)formedData[i, j];
                }
            }
        }

        private byte[,] ToFormData(byte[,] data, int startDataY, int endDataY, int startDataX, int endDataX)
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

        [ContextMenu("Show Fild")]
        public void ShowFildType()
        {
            string fild = "";
            for (int i = 0; i < X_SIZE; i++)
            {
                for (int j = 0; j < Y_SIZE; j++)
                {
                    fild += $"{(int)Cells[i, j].Cell.Type}\t";
                }
                fild += "\n";
            }

            Debug.Log(fild);
        }

        public bool IsCanPlace(Ship ship)
        {
            switch (ship.ShipPosition)
            {
                case Ship.Position.Horizontal:
                    for (int i = _targetCell.Y; i < _targetCell.Y + ship.CellCount; i++)
                    {
                        if (Cells[_targetCell.X, i].Cell.Type == CellModel.CellType.Nothing) continue;

                        Debug.LogWarning($"You can't place ship on [{_targetCell.X}, {i}]");
                        return false;
                    }
                    break;
                case Ship.Position.Vertical:
                    for (int i = _targetCell.X; i < _targetCell.X + ship.CellCount; i++)
                    {
                        if (Cells[i, _targetCell.Y].Cell.Type == CellModel.CellType.Nothing) continue;

                        Debug.LogWarning($"You can't place ship on [{i}, {_targetCell.Y}]");
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}
