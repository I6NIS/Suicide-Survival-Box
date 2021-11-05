using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using SuicideSurvival.entities.player;
using SuicideSurvival.systems;
using Player = SuicideSurvival.entities.player.Player;

namespace SuicideSurvival.ui
{
	public class Ammo : Panel
	{
		public readonly Label Label;

		public Ammo()
		{
			Label = Add.Label( "", "ammo" );
		}

		public override void Tick()
		{
			var player = Local.Pawn;
			if ( player == null || ((Player)player).Team != Team.Survivor ) return;
			
			Survivor survivor = player as Survivor;
			
			Label.Text = $"{survivor?.Ammo.ToString()}/{survivor?.MaxAmmo.ToString()}";
		}
	}
}
