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

		public Client[] clientsInGame;

		public OverflowGame()
		{
			// i feel incomplete without these log.info statements
			if ( IsServer )
			{
				Log.Info( "My Gamemode Has Created Serverside!" );
				new OverflowHUD();
			}

			if ( IsClient )
			{
				Log.Info( "My Gamemode Has Created Clientside!" );
			}
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
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
		[Input( Name = "PlayerWin" )]
		public void PlayerWin()
		{
			OverflowPlayer.startWin = true;
		}

		
	}
}
