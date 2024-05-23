using Assets.Scripts.Managers.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Managers
{
	public class UIManager : MonoBehaviour, IUIManager
	{
        [Header("Components")]
        [SerializeField] private Transform _fild;
        [SerializeField] private Transform _startPlace;

        public void ChangeShipColor(List<Image> cellImages, Color newColor)
        {
            foreach (var image in cellImages)
            {
                image.color = newColor;
            }
        }

        public void PlaceShipOnGrid(Transform shipTransform)
        {
            shipTransform.SetParent(_fild);
            shipTransform.position = _startPlace.position;
        }

        public void ReturnShipToList(Transform shipTransform, Transform shipList, Vector2 shipPos)
        {
            shipTransform.parent.SetParent(shipList);
            shipTransform.position = shipPos;
            shipTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
