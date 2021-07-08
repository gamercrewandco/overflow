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
		public Client currentSpectateClient;
		public int selectedClientIndex;

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

		/// <summary>
		/// don't use this, it doesn't work
		/// </summary>
		public void Spectate()
		{
			// handles changing the selected client's index
			if ( Input.Pressed( InputButton.Use ) )
				//IncreaseIndex();
			if ( Input.Pressed( InputButton.Menu ) )
				//DecreaseIndex();

			currentSpectateClient = Client.All[selectedClientIndex];

			Controller = null;
			Position = currentSpectateClient.Pawn.EyePos - new Vector3( 0, 0, 30 );

			// debug information
			//Log.Info(( currentSpectateClient.Name + " is client number " + selectedClientIndex) + " and is located at " + (currentSpectateClient.Pawn.EyePos - new Vector3( 0, 0, 30 )));
		}

		public override void OnKilled()
		{
			if ( playerFinished )
				return;
			playerFinished = true;
			OverflowGame.Current.playersLost++;

			Velocity = Vector3.Zero;
			Log.Info( GetClientOwner()?.Name + " has died to the flood!" );
		}

		public void OnWin()
		{
			if ( playerFinished )
				return;
			playerFinished = true;
			OverflowGame.Current.playersWon++;

			Velocity = Vector3.Zero;
			Log.Info( GetClientOwner()?.Name + " escaped the flood!" );
		}
	}
}
