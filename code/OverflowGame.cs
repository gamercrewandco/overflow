using Sandbox;
using Sandbox.UI.Construct;
using Sandbox.UI;
using System;
using System.IO;
using System.Threading.Tasks;

namespace overflow
{
	[Library( "overflow" )]
	public partial class OverflowGame : Game
    {
		public static new OverflowGame Current => Game.Current as OverflowGame;

		public OverflowGame()
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


}
