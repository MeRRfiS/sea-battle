using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Inputs.Interface
{
	public interface IPlayerInputManager
	{
		public void SetShip(ShipManager ship);
		public void Moving(InputAction.CallbackContext callback);
		public void MovingOnOpponent(InputAction.CallbackContext callback);
        public void Rotate(InputAction.CallbackContext callback);
        public void ResetShip(InputAction.CallbackContext callback);
		public void PlaceShip(InputAction.CallbackContext callback);
		public void Attack(InputAction.CallbackContext callback);
    } 
}
