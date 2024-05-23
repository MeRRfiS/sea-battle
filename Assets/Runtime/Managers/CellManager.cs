using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CellManager : MonoBehaviour
    {
        [field: Header("Position")]
        [field: SerializeField] public int X { get; private set; }
        [field: SerializeField] public int Y { get; private set; }

        public CellModel Cell;

        private void Awake()
        {
            Cell = new CellModel(CellModel.CellType.Nothing);
        }
    } 
}
