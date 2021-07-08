using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace overflow
{
	[Library( "trigger_player_win" )]
	public partial class TriggerPlayerWin : BaseTrigger
	{
		public override void StartTouch( Entity other )
		{
			if ( other is OverflowPlayer client )
			{
				client.OnWin();
			}

			base.StartTouch( other );
		}
	}
}
