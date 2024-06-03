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
        [SerializeField] private Transform _shipList;
        [SerializeField] private Transform _startPlace;
        [SerializeField] private Button _readyButton;

        [Header("Panels")]
        [SerializeField] private GameObject _waitPanel;
        [SerializeField] private GameObject _visualGrid;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private Image _backround;
        [SerializeField] private GameObject _menu;

        private VisualCellManager[,] _vCellsManager = new VisualCellManager[10,10];

        [Inject]
        private void Constructor(IOpponentFildManager opponentFild)
        {
            _opponentFild = opponentFild;

            var cells = _visualGrid.transform.GetComponentsInChildren<VisualCellManager>();
            foreach (var cell in cells)
            {
                _vCellsManager[cell.X, cell.Y] = cell;
            }
        }

        public void ChangeShipColor(List<Image> cellImages, Color newColor)
        {
            foreach (var image in cellImages)
            {
                image.DOColor(newColor, 0.25f);
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
            shipTransform.DOMove(_startPlace.position, 0.2f)
                .OnComplete(() =>
                {
                    shipTransform.GetComponent<ShipManager>().IsCanMoving = true;
                });
        }

        public void ReturnShipToList(Transform shipTransform, Vector2 shipPos)
        {
            shipTransform.SetParent(_shipList);
            shipTransform.GetComponent<ShipManager>().IsCanMoving = false;
            shipTransform.DOMove(shipPos, 0.2f);
            shipTransform.DORotate(Vector3.zero, 0.2f);
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

        public void ShowGetDamage()
        {
            DOTween.Sequence()
                .Append(_backround.DOFade(0.1f, 0.25f))
                .Append(_backround.DOFade(1, 0.25f));
        }

        public void SwitchInteractable(bool status)
        {
            _readyButton.interactable = status;
        }

        public void PlayerReadyButton()
        {
            _shipList.gameObject.SetActive(false);
        }

        public void OpenMenuButton()
        {
            _menu.SetActive(true);
        }

        public void CloseMenuButton()
        {
            _menu.SetActive(false);
        }
    }
}
