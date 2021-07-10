
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

namespace Sandbox.UI
{
	public partial class WinLoseFeed : Panel
	{
		public static WinLoseFeed Current;

		public WinLoseFeed()
		{
			Current = this;

			StyleSheet.Load( "/winlosefeed.scss" );
		}

		public virtual Panel AddEntry( Color color, string text )
		{
			var e = Current.AddChild<WinLoseFeedEntry>();

			e.text = text;
			e.Text.Style.FontColor = color;

			return e;
		}
	}
}
