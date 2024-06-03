using Assets.Scripts.Models.Ships;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
	public interface ILevelManager
	{
        public void PlacingShip(ShipManager shipManager);
        public void DisableShip();
        public void EnableShip();
        public void ChangeTargetCell(Vector2 position);
        public void ChangeTargetCell(int x, int y);
        public void UpdateFild(Ship ship);
        public void ShowFildType();
        public bool IsCanPlace(Ship ship);
        public void PlayerReadyButton();
        public void ResetShipButton();
    } 
}
