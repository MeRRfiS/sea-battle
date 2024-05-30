using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class CellManager : MonoBehaviour
    {
        [SerializeField] private Image _cellImage;

        [field: Header("Position")]
        [field: SerializeField] public int X { get; private set; }
        [field: SerializeField] public int Y { get; private set; }

        [Header("Colors")]
        [SerializeField] private Color _missColor;
        [SerializeField] private Color _damageColor;

        public CellModel Cell;

        private void Awake()
        {
            if(Cell == null) Cell = new CellModel(CellModel.CellType.Nothing);
        }

        public void CellColor()
        {
            switch (Cell.Type)
            {
                case CellModel.CellType.Miss:
                    _cellImage.color = _missColor;
                    break;
                case CellModel.CellType.Damage:
                    _cellImage.color = _damageColor;
                    break;
            }
        }
    }
}
