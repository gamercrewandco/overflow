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
			RootPanel.AddChild<NameTags>();
			RootPanel.AddChild<VoiceList>();
			RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			RootPanel.AddChild<Health>();
			RootPanel.AddChild<WinLoseFeed>();
			//RootPanel.AddChild<PlayersRemaining>();
		}
	}

	public class PlayersRemaining : Panel
	{
		public Label text;

		public PlayersRemaining()
		{
			text = AddChild<Label>( "" );
		}

		public override void Tick()
		{
			text.Text = (Client.All.Count - (OverflowGame.Current.playersWon + OverflowGame.Current.playersLost)).ToString() + " players remaining";
		}
	}

	public class WinnersScreen : Panel
	{
		public Label text;

		public WinnersScreen()
		{
			text = AddChild<Label>( "" );
		}

		public void Display()
		{

		}
	}
}
