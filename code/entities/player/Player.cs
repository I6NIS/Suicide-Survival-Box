using Sandbox;
using SuicideSurvival.systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuicideSurvival.entities.player
{
	public partial class Player : Player
	{
		public int kills { get; private set; }
		public Team team { get; private set; }
	}
}
