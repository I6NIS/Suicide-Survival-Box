using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuicideSurvival.systems;

namespace SuicideSurvival.entities.player
{
	public class Survivor : Player
	{
		public Survivor()
		{
			Team = Team.Survivor;
		}
		
		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

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
			Camera = new FirstPersonCamera();

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
				PlaySound( "boing" );
			}

			if ( Input.Pressed( InputButton.Attack2 ) )
			{
				PlaySound( "survivortaunt" );
			}
			
			if ( IsServer && Input.Pressed( InputButton.Attack1 ) )
			{
				var projectile = new ModelEntity();
				projectile.SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );
				projectile.Position = EyePos + EyeRot.Forward * 40;
				projectile.Rotation = Rotation.LookAt( Vector3.Random.Normal );
				projectile.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
				projectile.PhysicsGroup.Velocity = EyeRot.Forward * 1000;
			}
		}

		public override void OnAnimEventFootstep( Vector3 pos, int foot, float volume )
		{
			PlaySound( "footstep_human" );

			base.OnAnimEventFootstep( pos, foot, volume );
		}

		public override void OnKilled()
		{
			PlaySound( "surprised" );
			base.OnKilled();

			EnableDrawing = false;
		}
	}
}
