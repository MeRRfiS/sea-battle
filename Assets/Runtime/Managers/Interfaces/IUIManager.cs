using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers.Interfaces
{
	public interface IUIManager
	{
        public void PlaceShipOnGrid(Transform shipTransform);
        public void ReturnShipToList(Transform shipTransform, Vector2 shipPos);
        public void ChangeShipColor(List<Image> cellImages, Color newColor);
        public void DisableWaitPanel();
        public void EnableOpponentFild();
        public void ShowVisualCell(int x, int y, CellModel.CellType type);
        public void EnableWinPanel();
        public void EnableLosePanel();
        public void ShowGetDamage();
        public void SwitchInteractable(bool status);
        public void PlayerReadyButton();
        public void OpenMenuButton();
        public void CloseMenuButton();
    } 
}
