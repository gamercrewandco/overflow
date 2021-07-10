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
		public bool playerFinished;
		public bool cameraToggle;
		public bool ignoreSpectate;
		public int selectedClientIndex;

		public Color killedColor;
		public Color escapedColor;

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

			killedColor = Color.Red;
			escapedColor = Color.Green;

			base.Respawn();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

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

			if ( playerFinished )
			{
				EnableAllCollisions = false;
				EnableDrawing = false;
				cameraToggle = false;

				Controller = new NoclipController();
				Camera = new FirstPersonCamera();
			}
		}

		public override void OnKilled()
		{
			if ( playerFinished )
				return;
			playerFinished = true;
			OverflowGame.Current.playersLost++;

			Velocity = Vector3.Zero;
			Client clientOwner = GetClientOwner();
			Log.Info( clientOwner.Name + " has died to the flood!" );
			OnKilledMessage( killedColor, $"{clientOwner.Name} has died to the flood!" );
		}

		public void OnWin()
		{
			if ( playerFinished )
				return;
			playerFinished = true;
			OverflowGame.Current.playersWon++;

			Velocity = Vector3.Zero;
			Client clientOwner = GetClientOwner();
			Log.Info( GetClientOwner()?.Name + " escaped the flood!" );
			OnKilledMessage( escapedColor, $"{clientOwner.Name} escaped the flood!" );
		}

		[ClientRpc]
		public virtual void OnKilledMessage( Color color, string text )
		{
			WinLoseFeed.Current?.AddEntry( color, text );
		}
	}
}
