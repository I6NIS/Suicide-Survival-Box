using Sandbox;
using SuicideSurvival.systems;
using SuicideSurvival.ui;

namespace SuicideSurvival.entities.player
{
	public partial class Survivor : Player
	{
		[Net] public int Ammo { get; private set;}
		[Net] public int MaxAmmo { get; private set; } = 10;
		
		private float lastAttack = 0.0f;
		private float attackDelay = 2.0f;
		private SurvivorHud _survivorHud;
		
		
		public Survivor()
		{
			Team = Team.Survivor;

			if ( IsServer )
			{
				_survivorHud = new SurvivorHud();
			}
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

			Ammo = MaxAmmo;

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

			if ( Input.Pressed( InputButton.Attack2 ) )
			{
				PlaySound( "survivortaunt" );
			}
			
			if ( IsServer && Input.Pressed( InputButton.Attack1 ) )
			{
				// If we have no ammo, or our last attack was before the delay, return
				if (( Ammo < 1 ) || ( Time.Now < (lastAttack + attackDelay))) return;
				
				// BOOK LORE
				var book = new Book();
				book.Position = EyePos + EyeRot.Forward * 40;
				book.Rotation = Rotation.LookAt( Vector3.Random.Normal );
				book.PhysicsGroup.Velocity = EyeRot.Forward * 2000;

				using ( Prediction.Off() )
				{
					PlaySound( "boing" );
				}
				
				Ammo -= 1;
				lastAttack = Time.Now;
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
			
			_survivorHud.Delete();

			EnableDrawing = false;
		}
	}
}
