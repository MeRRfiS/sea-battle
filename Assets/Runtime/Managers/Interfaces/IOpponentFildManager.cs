using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
	public interface IOpponentFildManager
	{
        public int GetX();
        public int GetY();
        public void EnableFild();
        public void SetSelectedCell();
        public void RemoveSelectedCell();
        public void MoveSelectedCell(Vector2 moveSide);
        public CellManager ReturnCell();
        public void SetCellsInfo(CellModel[,] cellsInfo);
        public CellManager[,] ReturnFild();
        public void ShowShoot(Action shootResult);
    } 
}
