using Sandbox.UI;
using Sandbox;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Overflow
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class MinimalHudEntity : Sandbox.HudEntity<RootPanel>
	{
		public MinimalHudEntity()
		{
			if ( IsClient )
			{
				RootPanel.SetTemplate( "/minimalhud.html" );
			}
		}
	}

	[Library]
	public partial class FEHud : HudEntity<RootPanel>
	{
		public FEHud()
		{
			if ( !IsClient )
				return;

			RootPanel.StyleSheet.Load( "/minimalhud.scss" );
			RootPanel.AddChild<TimeUntilLoad>();
			RootPanel.AddChild<PlayersRemaining>();
		}
	}
}
