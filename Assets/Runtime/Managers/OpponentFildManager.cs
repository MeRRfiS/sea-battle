using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Models;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class OpponentFildManager : MonoBehaviour, IOpponentFildManager
    {
        private const int X_SIZE = 10;
        private const int Y_SIZE = 10;

        private CellManager[,] _cells = new CellManager[X_SIZE, Y_SIZE];

        [SerializeField] private GameObject _waitText;

        [Header("Prefabs")]
        [SerializeField] private GameObject _selectedCellPrefab;

        private GameObject _selectedCell;

        private bool _isCanMove = true;
        private int X = 0;
        private int Y = 0;
        private float _horizontalMax = 811.3401f;
        private float _horizontalMin = 136.3401f;
        private float _verticalMax = 339.5f;
        private float _verticalMin = -335.5f;

        public void MoveSelectedCell(Vector2 moveSide)
        {
            if (_selectedCell == null) return;
            if (!_isCanMove) return;

            Vector2 newPosition = new Vector2(_selectedCell.transform.localPosition.x + 75 * moveSide.x,
                                              _selectedCell.transform.localPosition.y + 75 * moveSide.y);
            if (!IsCanMove(newPosition)) return;

            _isCanMove = false;
            X = X - (int)moveSide.y;
            Y = Y + (int)moveSide.x;
            _selectedCell.transform.DOLocalMove(newPosition, 0.1f).OnComplete(() => { _isCanMove = true; });
        }

        public void EnableFild()
        {
            _waitText.SetActive(false);
            gameObject.SetActive(true);
        }

        public void SetSelectedCell()
        {
            X = 0; 
            Y = 0;

            _selectedCell = Instantiate(_selectedCellPrefab, transform);
            _selectedCell.transform.localPosition = _selectedCellPrefab.transform.position;
        }

        public void RemoveSelectedCell()
        {
            Destroy(_selectedCell);
        }

        private bool IsCanMove(Vector2 newPosition)
        {
            int newPositionXRound = (int)Math.Round(newPosition.x);
            int newPositionYRound = (int)Math.Round(newPosition.y);
            int horizontalMaxRound = (int)Math.Round(_horizontalMax);
            int horizontalMinRound = (int)Math.Round(_horizontalMin);
            int verticalMaxRound = (int)Math.Round(_verticalMax);
            int verticalMinRound = (int)Math.Round(_verticalMin);

            if (newPositionYRound > verticalMaxRound ||
                newPositionYRound < verticalMinRound) return false;

            if (newPositionXRound < horizontalMinRound ||
                newPositionXRound > horizontalMaxRound) return false;

            return true;
        }

        public int GetX()
        {
            return X;
        }

        public int GetY()
        {
            return Y;
        }

        public CellManager ReturnCell()
        {
            return _cells[X, Y];
        }

        public CellManager[,] ReturnFild()
        {
            return _cells;
        }

        public void SetCellsInfo(CellModel[,] cellsInfo)
        {
            var cells = transform.GetComponentsInChildren<CellManager>();
            foreach (var cell in cells)
            {
                _cells[cell.X, cell.Y] = cell;
            }

            for (int i = 0; i < _cells.GetLength(0); i++)
            {
                for (int j = 0; j < _cells.GetLength(1); j++)
                {
                    _cells[i, j].Cell = cellsInfo[i, j];
                }
            }
        }

        public void ShowShoot(Action shootResult)
        {
            DOTween.Sequence()
                .Append(_selectedCell.GetComponent<Image>().DOFade(0.1f, 0.25f))
                .Append(_selectedCell.GetComponent<Image>().DOFade(1f, 0.25f))
                .OnComplete(() => { shootResult?.Invoke(); });
        }
    } 
}
