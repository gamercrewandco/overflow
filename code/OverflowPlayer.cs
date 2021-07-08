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
		public static bool startWin;
		public bool cameraToggle;

		public PlayerSpectateCamera spectateCam;
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

			// spectator code (depends on if there is someone to specate)
			if (playerFinished && Client.All.Count > 1 )
			{
				Spectate();
			}
			else if ( playerFinished )
			{
				Controller = new NoclipController();
				Camera = new FirstPersonCamera();
			}
		}

		public void Spectate()
		{
			// handles changing the selected client's index
			if ( Input.Pressed( InputButton.Use ) )
				selectedClientIndex++;
			if ( Input.Pressed( InputButton.Menu ) )
				selectedClientIndex--;

			// handles overflow/underflow
			if ( selectedClientIndex > Client.All.Count - 1 )
				selectedClientIndex = 0;
			if ( selectedClientIndex < 0 )
				selectedClientIndex = Client.All.Count - 1;

			currentSpectateClient = Client.All[selectedClientIndex];

			// handles not allowing to spectate non-spectatable players
			if ((currentSpectateClient.Pawn as OverflowPlayer).ignoreSpectate )
			{
				if ( Input.Down( InputButton.Use ) )
					selectedClientIndex++;
				else if ( Input.Down( InputButton.Menu ) )
					selectedClientIndex--;
				else
					selectedClientIndex++;

				// handles overflow/underflow (again)
				if ( selectedClientIndex > Client.All.Count - 1 )
					selectedClientIndex = 0;
				if ( selectedClientIndex < 0 )
					selectedClientIndex = Client.All.Count - 1;

				currentSpectateClient = Client.All[selectedClientIndex];
			}

			Controller = null;
			Camera = spectateCam;
			Position = currentSpectateClient.Pawn.EyePos - new Vector3( 0, 0, 30 );
		}

		public override void OnKilled()
		{
			if ( playerFinished )
				return;
			playerFinished = true;

			Velocity = Vector3.Zero;

			EnableAllCollisions = false;
			EnableDrawing = false;

			ignoreSpectate = true;

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
