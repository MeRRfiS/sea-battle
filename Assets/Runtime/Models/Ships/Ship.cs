using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models.Ships
{
	public class Ship
	{
		public enum Position
		{
            Horizontal = 1,
			Vertical = 2,
        }

		public enum ShipType
		{
			One,
			Two,
			Three,
			Four
		}

		public Position ShipPosition;
		public ShipType Type;
		public int CellCount;
		[HideInInspector] public bool IsPlaced = false;

		private float _horizontalMax = -139.6201f;
        private float _horizontalMin = -814.6201f;
		private float _verticalMax = 339.5399f;
		private float _verticalMin = -335.4601f;

		public bool IsCanMove(Vector2 newPosition) 
		{
            int newPositionXRound = (int)Math.Round(newPosition.x);
            int newPositionYRound = (int)Math.Round(newPosition.y);
			int horizontalMaxRound = (int)Math.Round(_horizontalMax);
			int horizontalMinRound = (int)Math.Round(_horizontalMin);
			int verticalMaxRound = (int)Math.Round(_verticalMax);
			int verticalMinRound = (int)Math.Round(_verticalMin);

            switch (ShipPosition)
			{
				case Position.Horizontal:
					if (newPositionYRound > verticalMaxRound ||
                        newPositionYRound < verticalMinRound) return false;

					if (newPositionXRound < horizontalMinRound) return false;

					if (newPositionXRound > (horizontalMaxRound - 75 * (CellCount - 1))) return false;
					break;
				case Position.Vertical:
                    if (newPositionXRound > horizontalMaxRound ||
                        newPositionXRound < horizontalMinRound) return false;

                    if (newPositionYRound > verticalMaxRound) return false;

                    if (newPositionYRound < (verticalMinRound + 75 * (CellCount - 1))) return false;

                    break;
			}

			return true;
		}

		public void CheckRotation(ref Vector2 shipPos, Action<int, int> ChangeTargetCell)
		{
            int positionXRound = (int)Math.Round(shipPos.x);
            int positionYRound = (int)Math.Round(shipPos.y);
            int horizontalMaxRound = (int)Math.Round(_horizontalMax);
            int verticalMinRound = (int)Math.Round(_verticalMin);

            switch (ShipPosition)
            {
                case Position.Horizontal:
                    if (positionYRound < (verticalMinRound + 75 * (CellCount - 1)))
					{
						shipPos = new Vector2(shipPos.x, _verticalMin + 75 * (CellCount - 1));
						ChangeTargetCell?.Invoke(-1, 9 - (CellCount - 1));
                    }
                    break;
                case Position.Vertical:
					if(positionXRound > (horizontalMaxRound - 75 * (CellCount - 1)))
					{
						shipPos = new Vector2(_horizontalMax - 75 * (CellCount - 1), shipPos.y);
                        ChangeTargetCell?.Invoke(9 - (CellCount - 1), -1);
                    }
                    break;
            }
        }

		public virtual byte[,] ShipData()
		{
			return new byte[0, 0];
		}
    }
}
