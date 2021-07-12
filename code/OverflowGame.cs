using Sandbox;
using Sandbox.UI.Construct;
using Sandbox.UI;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

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
		[Net] public bool gameStarting { get; private set; }

		public bool resetWater;

		public OverflowGame()
		{
			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );

				new OverflowHUD();
				new OverflowHUDEntity();

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

			if ( IsServer && gameStarted && playersWon + playersLost == Client.All.Count )
			{
				Restart();
			}
		}

		/// <summary>
		/// Counts down, then starts the game.
		/// </summary>
		public async void StartGame()
		{
			startDelay = 10f;
			await Task.DelaySeconds( startDelay );
			gameStarted = true;
			gameStarting = false;
		}

		/// <summary>
		/// Waits a few seconds, then resets everything (for now).
		/// </summary>
		public async void Restart()
		{
			gameStarted = false;

			await Task.DelaySeconds( 10 );

			gameStarting = true;
			playersLost = 0;
			playersWon = 0;

			resetWater = true;

			// respawns all players
			Client.All.ToList().ForEach( Client => (Client.Pawn as OverflowPlayer)?.Respawn() );

			StartGame();
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
		/// <summary>
		/// Fires when the game starts.
		/// </summary>
		protected Output GameStarted { get; set; }

		[Event.Tick]
		public void Tick()
		{
			if ( OverflowGame.Current.gameStarted )
			{
				GameStarted.Fire( new Entity() );
			}
		}
	}

	[Library("overflow_water_mover")]
	public partial class OverflowWaterMover : Entity
	{
		[Event.Tick]
		public void Tick()
		{
			if ( OverflowGame.Current.resetWater )
			{
				Position = Vector3.Zero;
				OverflowGame.Current.resetWater = false;
			}
		}

		[Input]
		public void Move()
		{
			Position += new Vector3( 0f, 0f, .5f );
		}
	}
}
