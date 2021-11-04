using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuicideSurvival.sounds;
using SuicideSurvival.systems;

namespace SuicideSurvival.entities.player
{
	class Suicider : Player
	{
		[ServerVar]
		public static bool debug_prop_explosion { get; set; } = false;
		
		private string explosion_type = "explosivebarrel";
		private float explosive_damage = 200.0f;
		private float explosive_radius = 250.0f;
		private float explosive_force = 5.0f;
		private float explosion_delay = -1.0f;
		private string explosion_buildup = "";
		private string explosion_custom_sound = "rust_pumpshotgun.shootdouble";
		private string explosion_custom_effect = null;

		public Suicider()
		{
			Team = Team.Suicider;
		}

		public override void Respawn()
		{
			SetModel( "models/props/shrub.vmdl" );
			PlaySound( "spawn" );

			//
			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			//
			Controller = new WalkController();

			//
			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			//
			Animator = new StandardPlayerAnimator();

			//
			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			//
			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			//
			// If you have active children (like a weapon etc) you should call this to 
			// simulate those too.
			//
			SimulateActiveChild( cl, ActiveChild );

			//
			// If we're running serverside and Attack1 was just pressed, spawn a ragdoll
			//
			if ( Input.Pressed( InputButton.Attack1 ) )
			{
				DoExplosion();
			}

			if ( Input.Pressed( InputButton.Attack2 ) )
			{
				PlaySound( "shrubtaunt" );
			}

			if ( Input.Pressed( InputButton.Run ) )
			{
				PlaySound( "yalala" );
			}
		}

		public override void OnKilled()
		{
			base.OnKilled();
			PlaySound( "shrubhit" );

			EnableDrawing = false;
		}

		// Blatantly copied from Sandbox.Prop
		private void DoExplosion()
		{
			var model = GetModel();
			if ( model == null || model.IsError )
				return;

			if ( !PhysicsBody.IsValid() )
				return;

			if ( !string.IsNullOrWhiteSpace( explosion_custom_sound ) )
			{
				Sound.FromWorld( explosion_custom_sound, PhysicsBody.MassCenter );
			}
			else
			{
				// TODO: Replace with something else
				Sound.FromWorld( "SuicideSurvival.sounds.SoundEvents.ShrubExplosion", PhysicsBody.MassCenter );
			}

			if ( !string.IsNullOrWhiteSpace( explosion_custom_effect ) )
			{
				Particles.Create( explosion_custom_effect, PhysicsBody.MassCenter );
			}
			else
			{
				Particles.Create( "particles/explosion/barrel_explosion/explosion_barrel.vpcf", PhysicsBody.MassCenter );
			}

			// Damage and push away all other entities
			if ( explosive_radius > 0.0f )
			{
				var sourcePos = PhysicsBody.MassCenter;
				var overlaps = Physics.GetEntitiesInSphere( sourcePos, explosive_radius );

				if ( debug_prop_explosion )
					DebugOverlay.Sphere( sourcePos, explosive_radius, Color.Orange, true, 5 );

				foreach ( var overlap in overlaps )
				{
					if ( overlap is not ModelEntity ent || !ent.IsValid() )
						continue;

					if ( ent.LifeState != LifeState.Alive )
						continue;

					if ( !ent.PhysicsBody.IsValid() )
						continue;

					if ( ent.IsWorld )
						continue;

					var targetPos = ent.PhysicsBody.MassCenter;

					var dist = Vector3.DistanceBetween( sourcePos, targetPos );
					if ( dist > explosive_radius )
						continue;

					var tr = Trace.Ray( sourcePos, targetPos )
						.Ignore( this )
						.WorldOnly()
						.Run();

					if ( tr.Fraction < 0.95f )
					{
						if ( debug_prop_explosion )
							DebugOverlay.Line( sourcePos, tr.EndPos, Color.Red, 5, true );

						continue;
					}

					if ( debug_prop_explosion )
						DebugOverlay.Line( sourcePos, targetPos, 5, true );

					var distanceMul = 1.0f - Math.Clamp( dist / explosive_radius, 0.0f, 1.0f );
					var damage = explosive_damage * distanceMul;
					var force = (explosive_force * distanceMul) * ent.PhysicsBody.Mass;
					var forceDir = (targetPos - sourcePos).Normal;

					ent.TakeDamage( DamageInfo.Explosion( sourcePos, forceDir * force, damage )
						.WithAttacker( this ) );
				}
			}
		}
	}
}
