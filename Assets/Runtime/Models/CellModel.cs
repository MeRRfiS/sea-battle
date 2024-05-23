using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class CellModel
    {
        public enum CellType
        {
            Nothing = 0,
            Ship = 1,
            CantPlace = 2,
            Miss = 3,
            Damage = 4
        }

        public CellType Type;

        public CellModel(CellType type)
        {
            Type = type;
        }
    } 
}
