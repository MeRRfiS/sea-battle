using UnityEngine;

namespace Assets.Scripts.Models.Ships
{
    public class OneCellShip : Ship
    {
        public OneCellShip()
        {
            CellCount = 1;
        }

        public override byte[,] ShipData()
        {
            byte[,] data =
            {
                { 2,2,2 },
                { 2,1,2 },
                { 2,2,2 }
            };

            return data;
        }
    } 
}
