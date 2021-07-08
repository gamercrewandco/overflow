using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

namespace overflow
{
	public partial class OverflowHUDEntity : Sandbox.HudEntity<RootPanel>
	{
		public OverflowHUDEntity()
		{
			if ( IsClient )
			{
				RootPanel.SetTemplate( "/overflowhud.html" );
			}
		}
	}

	[Library]
	public partial class OverflowHUD : HudEntity<RootPanel>
	{
		public OverflowHUD()
		{
			if ( !IsClient )
				return;

			RootPanel.StyleSheet.Load( "/overflowhud.scss" );
			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<CurrentlySpectating>();
		}
	}
}
