using Sandbox;
using System;
using System.Linq;

namespace Overflow
{
	partial class FEPlayer : Player
	{
		WalkController controller = new WalkController();
		bool playerWalking;
		public static bool startPlayerWin;
		bool playerWonOrLost;

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			//
			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			//
			Controller = controller;

			//
			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			//
			Animator = new StandardPlayerAnimator();

			//
			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			//
			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			// sets the walking controller when the player is alive, and when the player is dead it sets it to the noclip controller (for spectating)
			if (playerWalking)
				Controller = controller;

			if ( startPlayerWin )
				OnWin();

			//
			// If you have active children (like a weapon etc) you should call this to 
			// simulate those too.
			//
			SimulateActiveChild( cl, ActiveChild );
		}

		public override void OnKilled()
		{
			FEGame.Current.numberOfPlayersLost++;

			// stops this from being called more than once
			if ( playerWonOrLost )
				return;

			playerWonOrLost = true;

			Log.Info( GetClientOwner()?.Name + " has died to the flood!" );


			playerWalking = false;
			EnableDrawing = false;

			controller.Velocity = Vector3.Zero;
			Controller = null;
			Controller = new NoclipController();

			Camera = null;
			Camera = new FirstPersonCamera();

			var ragdoll = new ModelEntity();
			ragdoll.SetModel( "models/citizen/citizen.vmdl" );
			ragdoll.Rotation = Rotation;
			ragdoll.Position = Position;
			ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
		}

		// entity placed in hammer that is told when the player hits the end trigger
		[Library( "info_fe_manager" )]
		public partial class Manager : Entity
		{
			[Input( Name = "PlayerWon" )]
			public void PlayerWon()
			{
				startPlayerWin = true;
			}
		}

		public void OnWin()
		{
			FEGame.Current.numberOfPlayersWon++;

			// stops this from being called more than once
			if ( playerWonOrLost )
				return;

			playerWonOrLost = true;

			// stops an endless loop of this being called
			startPlayerWin = false;

			Log.Info( GetClientOwner()?.Name + " escaped the flood!" );


			playerWalking = false;
			EnableDrawing = false;

			controller.Velocity = Vector3.Zero;
			Controller = null;
			Controller = new NoclipController();

			Camera = null;
			Camera = new FirstPersonCamera();
		}
	}
}
