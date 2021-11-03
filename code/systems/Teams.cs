using Sandbox;
using SuicideSurvival.entities.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Player = SuicideSurvival.entities.player.Player;

namespace SuicideSurvival.systems
{
	public enum Team
	{
		None,
		Suicider,
		Survivor
	}

	public static class TeamExtensions
	{
		public static string GetName(this Team team )
		{
			if (team == Team.Suicider )
			{
				return "Suiciders";
			}
			else if (team == Team.Survivor )
			{
				return "Survivors";
			}
			else
			{
				return "None";
			}
		}

		public static To GetTo( this Team team )
		{
			return To.Multiple( team.GetAll().Select( e => e.Client ) );
		}

		public static IEnumerable<Player> GetAll(this Team team )
		{
			return Entity.All.OfType<Player>().Where( e => e.Team == team );
		}

		public static int GetCount(this Team team )
		{
			return Entity.All.OfType<Player>().Where( e => e.Team == team ).Count();
		}
	}
}
