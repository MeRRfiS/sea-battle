using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models.Ships
{
    public class TwoCellShip : Ship
	{
        public TwoCellShip()
        {
            CellCount = 2;
        }

        public override byte[,] ShipData()
        {
            byte[,] data = base.ShipData();

            switch (ShipPosition)
            {
                case Position.Horizontal:
                    data = new byte[,]
                    {
                        { 2,2,2,2 },
                        { 2,1,1,2 },
                        { 2,2,2,2 }
                    };
                    break;
                case Position.Vertical:
                    data = new byte[,]
                    {
                        { 2,2,2 },
                        { 2,1,2 },
                        { 2,1,2 },
                        { 2,2,2 }
                    };
                    break;
            }

            return data;
        }
    }
}
