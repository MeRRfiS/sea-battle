using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers.Interfaces
{
	public interface IUIManager
	{
        public void PlaceShipOnGrid(Transform shipTransform);
        public void ReturnShipToList(Transform shipTransform, Transform shipList, Vector2 shipPos);
        public void ChangeShipColor(List<Image> cellImages, Color newColor);
    } 
}
