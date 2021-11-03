using Sandbox;
using SuicideSurvival.systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuicideSurvival.entities.map
{
	[Library( "ss_spawnpoint")]
	[Hammer.EditorModel("models/editor/playerstart.vmdl", FixedBounds = true)]
	[Hammer.EntityTool( "Player Spawnpoint", "Suicide Survival", "Defines a point where players on a team can spawn")]
	
	public partial class PlayerSpawnpoint : Entity
	{
		[Property] public Team Team { get; set; }
	}
}
