using Sandbox;
using SuicideSurvival.systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuicideSurvival.entities.player
{
	public partial class Player
	{
		[Net, Change] public Team Team { get; private set; }

		public void SetTeam(Team team )
		{
			Team = team;
			Client.SetInt( "team", (int)team );
		}
	}
}
