using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//

namespace FloodEscape
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
	[Library( "flood_escape" )]
	public partial class Game : Sandbox.Game
	{
		public Game()
		{
			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );
			}

			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );
			}
		}

		public static int numberOfPlayersWon;
		public static int numberOfPlayersLost;
		public bool restartingGame;
		float timeToRestart = 6f;

		//debug
		double lastNum;
		//

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if (numberOfPlayersLost + numberOfPlayersWon == Client.All.Count && !restartingGame)
			{
				Log.Info( "Game ended with " + numberOfPlayersLost + " player(s) lost and " + numberOfPlayersWon + " player(s) won!" );
				restartingGame = true;
			}

			if ( restartingGame )
			{
				timeToRestart -= Time.Delta;

				//this is just for debug, not important to the actual game
				if ( lastNum != Math.Truncate(timeToRestart))
					Log.Info( (Math.Truncate( timeToRestart ) + 1) + " seconds until load");
				lastNum = Math.Truncate( timeToRestart );
				//end of debug

				if (timeToRestart <= 0f)
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
}
