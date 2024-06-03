using Assets.Scripts.Inputs.Interface;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Models.Ships;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class ShipManager : MonoBehaviour
    {
        [HideInInspector] public Ship Ship;

        [Header("Settings")]
        [SerializeField] private Transform _shipPos;
        [SerializeField] private Ship.ShipType _type;
        [SerializeField] private Color _deselectColor;
        [SerializeField] private Color _selectColor;
        [SerializeField] private Color _cantPlaceColor;

        [Header("Components")]
        [SerializeField] private BoxCollider2D _boxCollider;
        [SerializeField] private List<Image> _cellImages;

        private ILevelManager _levelManager;
        private IPlayerInputManager _playerInputManager;
        private IUIManager _uiManager;

        private bool _isCanMoving = false;

        public bool IsCanMoving
        {
            set { _isCanMoving = value; }
        }

        [Inject]
        private void Contructor(ILevelManager levelManager,
                                IPlayerInputManager playerInputManager,
                                IUIManager uiManager)
        {
            _levelManager = levelManager;
            _playerInputManager = playerInputManager;
            _uiManager = uiManager;

            switch (_type)
            {
                case Ship.ShipType.One:
                    Ship = new OneCellShip();
                    break;
                case Ship.ShipType.Two:
                    Ship = new TwoCellShip();
                    break;
                case Ship.ShipType.Three:
                    Ship = new ThreeCellShip();
                    break;
                case Ship.ShipType.Four:
                    Ship = new FourCellShip();
                    break;
            }
        }

        private void OnMouseDown()
        {
            Ship.ShipPosition = Ship.Position.Horizontal;

            _levelManager.PlacingShip(this);
            _levelManager.DisableShip();
            _playerInputManager.SetShip(this);
            _uiManager.ChangeShipColor(_cellImages, _selectColor);
        }

        public void MoveShip(Vector2 moveSide)
        {
            if (!_isCanMoving) return;

            Vector2 newPosition = new Vector2(transform.localPosition.x + 75 * moveSide.x,
                                              transform.localPosition.y + 75 * moveSide.y);
            if (!Ship.IsCanMove(newPosition)) return;

            _isCanMoving = false;
            _levelManager.ChangeTargetCell(moveSide);
            transform.DOLocalMove(newPosition, 0.1f).OnComplete(() => { _isCanMoving = true; });
        }

        public void RotateShip()
        {
            if (!_isCanMoving) return;

            _isCanMoving = false;
            Vector2 shipPos = transform.localPosition;
            Ship.CheckRotation(ref shipPos, _levelManager.ChangeTargetCell);
            transform.DOLocalMove(shipPos,0.1f);

            switch (Ship.ShipPosition)
            {
                case Ship.Position.Horizontal:
                    transform.DORotate(new Vector3(0, 0, -90), 0.1f).OnComplete(() => { _isCanMoving = true; });
                    Ship.ShipPosition = Ship.Position.Vertical;
                    break;
                case Ship.Position.Vertical:
                    transform.DORotate(new Vector3(0, 0, 0), 0.1f).OnComplete(() => { _isCanMoving = true; });
                    Ship.ShipPosition = Ship.Position.Horizontal;
                    break;
            }
        }

        public void DeselectShip()
        {
            _uiManager.ReturnShipToList(transform, _shipPos.position);
            _uiManager.ChangeShipColor(_cellImages, _deselectColor);
            Ship.ShipPosition = Ship.Position.Horizontal;
            _levelManager.EnableShip();
        }

        public void PlacedShip()
        {
            Ship.IsPlaced = true;
            _uiManager.ChangeShipColor(_cellImages, _deselectColor);
            _levelManager.UpdateFild(Ship);
        }

        private void Update()
        {
            if(Ship.IsPlaced) _uiManager.ChangeShipColor(_cellImages, _deselectColor);
        }

        public IEnumerator CantPlace()
        {
            _isCanMoving = false;
            _uiManager.ChangeShipColor(_cellImages, _cantPlaceColor);

            yield return new WaitForSeconds(0.3f);

            _uiManager.ChangeShipColor(_cellImages, _selectColor);
            _isCanMoving = true;
        }
    }
}
