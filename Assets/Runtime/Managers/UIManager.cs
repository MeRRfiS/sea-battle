using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Models;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Managers
{
	public class UIManager : MonoBehaviour, IUIManager
	{
        private IOpponentFildManager _opponentFild;

        [Header("Components")]
        [SerializeField] private Transform _fild;
        [SerializeField] private Transform _startPlace;

        [Header("Panels")]
        [SerializeField] private GameObject _waitPanel;
        [SerializeField] private GameObject _visualGrid;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;

        private VisualCellManager[,] _vCellsManager = new VisualCellManager[10,10];

        [Inject]
        private void Constructor(IOpponentFildManager opponentFild)
        {
            _opponentFild = opponentFild;

            var cells = _visualGrid.transform.GetComponentsInChildren<VisualCellManager>();
            foreach (var cell in cells)
            {
                _vCellsManager[cell.X, cell.Y] = cell;
                //cell.gameObject.SetActive(false);
            }
        }

        public void ChangeShipColor(List<Image> cellImages, Color newColor)
        {
            foreach (var image in cellImages)
            {
                image.color = newColor;
            }
        }

        public void DisableWaitPanel()
        {
            _waitPanel.SetActive(false);
        }

        public void EnableOpponentFild()
        {
            _opponentFild.EnableFild();
        }

        public void PlaceShipOnGrid(Transform shipTransform)
        {
            shipTransform.SetParent(_fild);
            shipTransform.DOMove(_startPlace.position, 0.2f);
            //shipTransform.position = _startPlace.position;
        }

        public void ReturnShipToList(Transform shipTransform, Transform shipList, Vector2 shipPos)
        {
            shipTransform.parent.SetParent(shipList);
            shipTransform.DOMove(shipPos, 0.2f);
            shipTransform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public void ShowVisualCell(int x, int y, CellModel.CellType type)
        {
            _vCellsManager[x, y].Cell = new CellModel(type);
            _vCellsManager[x, y].CellImage();

            _vCellsManager[x, y].gameObject.SetActive(true);
        }

        public void EnableWinPanel()
        {
            _winPanel.SetActive(true);
        }

        public void EnableLosePanel()
        {
            _losePanel.SetActive(true);
        }
    }
}
