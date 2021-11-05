using System;
using System.Threading.Tasks;
using Sandbox;
using SuicideSurvival.entities.player;

namespace SuicideSurvival.entities
{
	public class Book : ModelEntity
	{
		private bool _causeDamage = true;
		
		public Book()
		{
			if ( !IsServer ) return;
			this.SetModel( "models/props/book.vmdl" );
			this.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
			this.EnableTouch = true;
			
			DeleteAfterDelay( 60000, this );
		}
		
		private async Task DeleteAfterDelay( int delay, Entity ent )
		{
			if ( !IsServer ) return;
			await Task.Delay( delay );
			if (ent.IsValid())
				ent.Delete();
		}

		public override void Touch(Entity other)
		{
			if ( other is Suicider && _causeDamage)
			{
				other.TakeDamage( DamageInfo.Generic( Single.PositiveInfinity ) );
			}
			_causeDamage = false;
		}
	}
}
