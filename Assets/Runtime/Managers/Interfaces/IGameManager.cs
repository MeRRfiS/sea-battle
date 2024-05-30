using Assets.Scripts.Enums;
using Photon.Realtime;

namespace Assets.Scripts.Managers.Interfaces
{
	public interface IGameManager
	{
		public bool IsGameReady(); 
        public void ConnectPlayer(Player player);
		public PlayerManager GetPlayer();
        public PlayerManager GetPlayer(PlayerEnum type);
        public void CheckPlayersReady();
		public void MakeShoot();
        public void SetUpPlayerFild(CellManager[,] fild);

    } 
}
