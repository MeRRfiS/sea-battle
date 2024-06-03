using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers.Interfaces
{
	public interface ILobbyManager
	{
		public void OpenCreateServerMenu();
		public void CloseCreateServerMenu();
		public void CreateServer();
		public void OpenServer(string name);
		public void OpenInstructionButton();
        public void CloseInstructionButton();
        public void LeaveGame();
    } 
}
