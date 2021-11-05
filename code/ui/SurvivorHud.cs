using Sandbox;
using Sandbox.UI;

namespace SuicideSurvival.ui
{
	public partial class SurvivorHud : HudEntity<RootPanel>
	{
		public SurvivorHud()
		{
			if ( !IsClient ) return;
			
			RootPanel.StyleSheet.Load( "/ui/SurvivorHud.scss" );

			RootPanel.AddChild<Ammo>();
		}
		
	}
}
