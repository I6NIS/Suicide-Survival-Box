using Sandbox;
using SuicideSurvival.systems;

namespace SuicideSurvival.entities.player
{
	public partial class Player
	{
		[Net, Change] public Team Team { get; private set; }

		public void SetTeam( Team team )
		{
			Team = team;
			Client.SetInt( "team", (int)team );
		}
		
		protected virtual void OnTeamChanged( Team oldTeam, Team newTeam )
		{
			// noop
		}
	}
}
