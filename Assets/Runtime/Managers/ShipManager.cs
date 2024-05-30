using Assets.Scripts.Inputs.Interface;
using Assets.Scripts.Managers.Interfaces;
using Assets.Scripts.Models.Ships;
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
        [SerializeField] private Transform _shipList;
        [SerializeField] private Transform _shipPos;
        [SerializeField] private Ship.ShipType _type;
        [SerializeField] private Color _deselectColor;
        [SerializeField] private Color _selectColor;

        [Header("Components")]
        [SerializeField] private BoxCollider2D _boxCollider;
        [SerializeField] private List<Image> _cellImages;

        private ILevelManager _levelManager;
        private IPlayerInputManager _playerInputManager;
        private IUIManager _uiManager;

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
            Vector2 newPosition = new Vector2(transform.localPosition.x + 75 * moveSide.x,
                                              transform.localPosition.y + 75 * moveSide.y);
            if (!Ship.IsCanMove(newPosition)) return;

            _levelManager.ChangeTargetCell(moveSide);
            transform.localPosition = newPosition;
        }

        public void RotateShip()
        {
            Vector2 shipPos = transform.localPosition;
            Ship.CheckRotation(ref shipPos, _levelManager.ChangeTargetCell);
            transform.localPosition = shipPos;

            switch (Ship.ShipPosition)
            {
                case Ship.Position.Horizontal:
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                    Ship.ShipPosition = Ship.Position.Vertical;
                    break;
                case Ship.Position.Vertical:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    Ship.ShipPosition = Ship.Position.Horizontal;
                    break;
            }
        }

        public void DeselectShip()
        {
            _uiManager.ReturnShipToList(transform, _shipList, _shipPos.position);
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
    }
}
