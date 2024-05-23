using Assets.Scripts.Inputs.Interface;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts.Inputs
{
    public class PlayerInputManager : MonoBehaviour, IPlayerInputManager
    {
        private ShipManager _selectedShip;

        private ILevelManager _levelManager;

        [Inject]
        private void Contructor(ILevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void SetShip(ShipManager ship)
        {
            _selectedShip = ship;
        }

        public void ResetShip(InputAction.CallbackContext callback)
        {
            if (_selectedShip == null) return;
            if (!callback.started) return;

            _selectedShip.DeselectShip();
            _selectedShip = null;
        }

        public void Moving(InputAction.CallbackContext callback)
        {
            if (_selectedShip == null) return;
            if (!callback.started) return;

            _selectedShip.MoveShip(callback.ReadValue<Vector2>());
        }

        public void Rotate(InputAction.CallbackContext callback)
        {
            if (_selectedShip == null) return;
            if (!callback.started) return;

            _selectedShip.RotateShip();
        }

        public void PlaceShip(InputAction.CallbackContext callback)
        {
            if (_selectedShip == null) return;
            if (!callback.started) return;
            if (!_levelManager.IsCanPlace(_selectedShip.Ship)) return;

            _selectedShip.PlacedShip();
            _selectedShip = null;
            _levelManager.EnableShip();
        }
    }
}
