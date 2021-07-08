using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

namespace overflow
{
	[Library]
	public partial class OverflowHUD : HudEntity<RootPanel>
	{
		public OverflowHUD()
		{
			if ( !IsClient )
				return;

			RootPanel.AddChild<ChatBox>();
		}
	}
}
