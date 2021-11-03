
using Gamelib.Extensions;
using Sandbox;
using Sandbox.UI.Construct;
using SuicideSurvival.entities.map;
using SuicideSurvival.entities.player;
using SuicideSurvival.systems;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Player = SuicideSurvival.entities.player.Player;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace SuicideSurvival
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	[Library( "suicideSurvival", Title = "Suicide Survival" )]
	public partial class Game : Sandbox.Game
	{
		public Game()
		{
			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );

				// Create a HUD entity. This entity is globally networked
				// and when it is created clientside it creates the actual
				// UI panels. You don't have to create your HUD via an entity,
				// this just feels like a nice neat way to do it.
				new MinimalHudEntity();
			}

			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );
			}
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			// Debug code, makes you a random "team" at join
			Player player = new Random().Next( 2 ) > 0 ? new Suicider() : new Survivor();
			client.Pawn = player;
			
			player.Respawn();
		}

		public override void MoveToSpawnpoint( Entity pawn )
		{
			if ( pawn is Player player )
			{
				var team = player.Team;

				if ( team == Team.None )
					team = Team.Survivor;

				var spawnpoints = All.OfType<PlayerSpawnpoint>()
					.Where( e => e.Team == team )
					.ToList()
					.Shuffle();

				if ( spawnpoints.Count > 0 )
				{
					var spawnpoint = spawnpoints[0];
					player.Transform = spawnpoint.Transform;
					return;
				}
			}

			base.MoveToSpawnpoint( pawn );
		}
	}

}
