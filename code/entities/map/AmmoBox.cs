using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuicideSurvival.entities.map
{
	[Library( "ss_ammo" )]
	[Hammer.EditorModel( "models/props/ammo_box.vmdl", FixedBounds = true )]
	[Hammer.EntityTool( "Ammo Box", "Suicide Survival", "Provides ammo for survivors" )]
	public partial class AmmoBox : Entity
	{

	}
}
