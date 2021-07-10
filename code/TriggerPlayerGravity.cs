using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace overflow
{
	[Library( "trigger_player_gravity" )]
	public partial class TriggerPlayerGravity : BaseTrigger
	{
		[Property( Title = "Gravity" ) ]
		public float gravity { get; set; }

		public override void StartTouch( Entity other )
		{
			if ( other is OverflowPlayer client )
			{
				if ( client.playerFinished )
					return;

				WalkController controller = new WalkController();
				controller.Gravity = gravity;
				client.Controller = controller;
			}

			base.StartTouch( other );
		}

		public override void EndTouch( Entity other )
		{
			if ( other is OverflowPlayer client )
			{
				if ( client.playerFinished )
					return;

				client.Controller = new WalkController();
			}

			base.EndTouch( other );
		}
	}
}
