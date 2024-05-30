using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class VisualCellManager : MonoBehaviour
    {
        [SerializeField] private Image _cellImage;

        [field: Header("Position")]
        [field: SerializeField] public int X { get; private set; }
        [field: SerializeField] public int Y { get; private set; }

        [Header("Sprites")]
        [SerializeField] private Sprite _missSprite;
        [SerializeField] private Sprite _damageSprite;

        public CellModel Cell;

        private void Awake()
        {
            if (Cell == null) 
            {
                Cell = new CellModel(CellModel.CellType.Nothing);
                _cellImage.color = new Color(1, 1, 1, 0);
            }
        }

        public void CellImage()
        {
            _cellImage.color = new Color(1, 1, 1, 1);

            switch (Cell.Type)
            {
                case CellModel.CellType.Nothing:
                case CellModel.CellType.CantPlace:
                    _cellImage.sprite = _missSprite;
                    break;
                case CellModel.CellType.Ship:
                    _cellImage.sprite = _damageSprite;
                    break;
            }
        }
    } 
}
