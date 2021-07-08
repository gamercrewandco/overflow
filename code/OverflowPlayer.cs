using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;

namespace overflow
{
	partial class OverflowPlayer : Player
	{
		public static bool startWin;

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			// set controller, animator, and camera (pretty cool)
			Controller = new WalkController();
			Animator = new StandardPlayerAnimator();
			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			
			if ( startWin )
			{
				startWin = false;
				OnWin();
			}
		}

		public override void OnKilled()
		{
			Controller = new NoclipController();
			Camera = new FirstPersonCamera();
			Velocity = Vector3.Zero;

			EnableAllCollisions = false;
			EnableDrawing = false;

			Log.Info( GetClientOwner()?.Name + " has died to the flood!" );
		}

		public void OnWin()
		{
			Log.Info( GetClientOwner()?.Name + " escaped the flood!" );
		}
	}
}
