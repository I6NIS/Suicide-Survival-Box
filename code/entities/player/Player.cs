using Sandbox;
using System;

namespace SuicideSurvival.entities.player
{
	public partial class Player : Sandbox.Player
	{
		public int Kills { get; private set; }

		public override void OnKilled()
		{
			// Also debug code, randomizes player "team"
			Client client = this.Client;
			
			Player player = new Random().Next( 2 ) > 0 ? new Suicider() : new Survivor();
			client.Pawn = player;
			
			base.OnKilled();
			player.Respawn();
			Delete();
		}
	}
}
