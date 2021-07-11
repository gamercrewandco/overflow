using Sandbox;
using Sandbox.UI.Construct;
using Sandbox.UI;
using System;
using System.IO;
using System.Threading.Tasks;

// make sure everything is in the same namespace, it's pretty important
namespace overflow
{
	[Library( "overflow" )]
	public partial class OverflowGame : Game
    {
		// this is how we let every other class easily reference this one without static variables, neat!
		public static new OverflowGame Current => Game.Current as OverflowGame;

		[Net] public int playersWon { get; set; }
		[Net] public int playersLost { get; set; }
		[Net] public float startDelay { get; private set; }
		[Net] public bool gameStarted { get; private set; }

		public OverflowGame()
		{
			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );

				new OverflowHUD();
				new OverflowHUDEntity();

				startDelay = 15f;
				StartGame();
			}

			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );
			}
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( IsServer && playersWon + playersLost == Client.All.Count )
			{
				Restart();
			}
		}

		/// <summary>
		/// Counts down, then starts the game.
		/// </summary>
		public async void StartGame()
		{
			await Task.DelaySeconds( startDelay );
			gameStarted = true;
			Log.Info( "Game has started!" );
		}

		/// <summary>
		/// Waits a few seconds, then loads the next level (for now).
		/// </summary>
		public async void Restart()
		{
			await Task.DelaySeconds( 10 );
			ConsoleSystem.Run( "changelevel", "gamercrew.overflow_woods" );
		}

		// an epic gamer just joined the server, better give them an epic gamer pawn to play with
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new OverflowPlayer();
			client.Pawn = player;

			player.Respawn();
		}
	}


	[Library( "info_overflow_game_manager" )]
	public partial class OverflowGameManager : Entity
	{
		public Output StartWaterRise;

		
	}
}
