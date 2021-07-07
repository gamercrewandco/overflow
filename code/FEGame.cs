using Sandbox;
using Sandbox.UI.Construct;
using Sandbox.UI;
using System;
using System.IO;
using System.Threading.Tasks;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//

namespace Overflow
{
	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// 
	/// Your game needs to be registered (using [Library] here) with the same name 
	/// as your game addon. If it isn't then we won't be able to find it.
	/// </summary>
	[Library( "overflow" )]
	public partial class FEGame : Sandbox.Game
	{
		public static new FEGame Current => Game.Current as FEGame;

		public FEGame()
		{
			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );
				new MinimalHudEntity();
				new FEHud();
			}

			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );
			}

			timeToLoadLevel = 6;
		}

		[Net] public int numberOfPlayersWon { get; set; }
		[Net] public int numberOfPlayersLost { get; set; }
		[Net] public bool loadingLevel { get; set; }
		public float timeToLoadLevel { get; set; }

		//debug
		double lastNum;
		//

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( numberOfPlayersLost + numberOfPlayersWon == Client.All.Count && !loadingLevel )
			{
				Log.Info( "Game ended with " + numberOfPlayersLost + " player(s) lost and " + numberOfPlayersWon + " player(s) won!" );
				loadingLevel = true;
			}
			
			if ( loadingLevel )
			{
				timeToLoadLevel -= Time.Delta;

				{
					//this is just for debug, not important to the actual game
					if ( lastNum != Math.Truncate( timeToLoadLevel ) )
						Log.Info( (Math.Truncate( timeToLoadLevel ) + 1) + " seconds until load" );
					lastNum = Math.Truncate( timeToLoadLevel );
					//end of debug
				}

				if ( timeToLoadLevel <= 0f )
					ConsoleSystem.Run( "changelevel", "flood_escape_testmap" );
			}

		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new FEPlayer();
			client.Pawn = player;

			player.Respawn();
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
			text.Text = (Client.All.Count - (FEGame.Current.numberOfPlayersWon + FEGame.Current.numberOfPlayersLost)) + " Player(s) remaining";
		}
	}

	public class TimeUntilLoad : Panel
	{
		public Label text;

		public TimeUntilLoad()
		{
			text = AddChild<Label>( "" );
		}
		
		public override void Tick()
		{
			if ( !FEGame.Current.loadingLevel )
				return;

			text.Text = ("Loading Level In " + Math.Truncate( FEGame.Current.timeToLoadLevel + 1 ) + " Seconds");
		}
	}
}
