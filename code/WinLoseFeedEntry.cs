
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

namespace Sandbox.UI
{
	public partial class WinLoseFeedEntry : Panel
	{
		public Label Text;
		public string text;

		public RealTimeSince TimeSinceBorn = 0;

		public WinLoseFeedEntry()
		{
			Text = Add.Label( "" );
		}

		public override void Tick() 
		{
			base.Tick();
			Text.Text = text;

			if ( TimeSinceBorn > 6 ) 
			{ 
				Delete();
			}
		}

	}
}
