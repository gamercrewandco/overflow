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
		public static bool playerFinished;
		public static bool startWin;
		public bool cameraToggle;
		public PlayerSpectateCamera spectateCam;

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			// set controller, animator, and camera (pretty cool)
			Controller = new WalkController();
			Animator = new StandardPlayerAnimator();
			Camera = new ThirdPersonCamera();
			spectateCam = new PlayerSpectateCamera();

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

			if ( Input.Pressed( InputButton.View ) && IsServer)
			{
				if ( cameraToggle == false )
				{
					Camera = new FirstPersonCamera();
					cameraToggle = true;
				}
				else
				{
					Camera = new ThirdPersonCamera();
					cameraToggle = false;
				}
			}

			// after finished stuff (win/death, doesn't matter)
			if (playerFinished && Client.All.Count > 0 )
			{
				Controller = null;
				Camera = spectateCam;
			}
			else if ( playerFinished )
			{
				Controller = new NoclipController();
				Camera = new FirstPersonCamera();
			}

			if (spectateCam != null)
				spectateCam.FocusPoint = Client.All[Client.All.Count - 1].Pawn.Position;
		}

		public override void OnKilled()
		{
			if ( playerFinished )
				return;
			playerFinished = true;

			Velocity = Vector3.Zero;

			EnableAllCollisions = false;
			EnableDrawing = false;

			Log.Info( GetClientOwner()?.Name + " has died to the flood!" );
		}

		public void OnWin()
		{
			if ( playerFinished )
				return;
			playerFinished = true;

			Log.Info( GetClientOwner()?.Name + " escaped the flood!" );
		}
	}
}
