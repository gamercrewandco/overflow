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

		ModelEntity pants;
		ModelEntity jacket;
		ModelEntity shoes;
		ModelEntity hat;

		bool dressed = false;


		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			// set controller, animator, and camera (pretty cool)
			Controller = null;
			Animator = new StandardPlayerAnimator();
			Camera = new ThirdPersonCamera();
			
			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			killedColor = Color.Red;
			escapedColor = Color.Green;

			Dress();

			base.Respawn();
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			// disallow player movement until the game has started
			if ( OverflowGame.Current.gameStarted && !playerFinished && Controller == null )
				Controller = new WalkController();

			// first/third person camera toggling
			if ( Input.Pressed( InputButton.View ) && IsServer && !playerFinished )
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

			// start spectating when the player wins/loses
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
			if ( playerFinished ) return;
			playerFinished = true;

			OverflowGame.Current.playersLost++;

			Velocity = Vector3.Zero;
			Client clientOwner = GetClientOwner();
			Log.Info( clientOwner.Name + " has died to the flood!" );
			OnKilledMessage( killedColor, $"{clientOwner.Name} has died to the flood!" );
		}

		public void OnWin()
		{
			if ( playerFinished ) return;
			playerFinished = true;

			OverflowGame.Current.playersWon++;

			Velocity = Vector3.Zero;
			Client clientOwner = GetClientOwner();
			Log.Info( GetClientOwner()?.Name + " escaped the flood!" );
			OnKilledMessage( escapedColor, $"{clientOwner.Name} escaped the flood!" );
		}

		/// <summary>
		/// Creates the UI message stating the player either escaped or died.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="text"></param>
		[ClientRpc]
		public virtual void OnKilledMessage( Color color, string text )
		{
			WinLoseFeed.Current?.AddEntry( color, text );
		}
		
		public void Dress()
		{		
			if ( dressed ) return;
			dressed = true;

			if ( true )
			{
				var model = Rand.FromArray( new[]
				{
				"models/citizen_clothes/trousers/trousers.jeans.vmdl",
				"models/citizen_clothes/trousers/trousers.lab.vmdl",
				"models/citizen_clothes/trousers/trousers.police.vmdl",
				"models/citizen_clothes/trousers/trousers.smart.vmdl",
				"models/citizen_clothes/trousers/trousers.smarttan.vmdl",
				"models/citizen/clothes/trousers_tracksuit.vmdl",
				"models/citizen_clothes/trousers/trousers_tracksuitblue.vmdl",
				"models/citizen_clothes/trousers/trousers_tracksuit.vmdl",
				"models/citizen_clothes/shoes/shorts.cargo.vmdl",
			} );

				pants = new ModelEntity();
				pants.SetModel( model );
				pants.SetParent( this, true );
				pants.EnableShadowInFirstPerson = true;
				pants.EnableHideInFirstPerson = true;

				pants.SetBodyGroup( "Legs", 1 );
			}

			if ( true )
			{
				var model = Rand.FromArray( new[]
				{
				"models/citizen_clothes/jacket/labcoat.vmdl",
				"models/citizen_clothes/jacket/jacket.red.vmdl",
				"models/citizen_clothes/jacket/jacket.tuxedo.vmdl",
				"models/citizen_clothes/jacket/jacket_heavy.vmdl",
			} );

				jacket = new ModelEntity();
				jacket.SetModel( model );
				jacket.SetParent( this, true );
				jacket.EnableShadowInFirstPerson = true;
				jacket.EnableHideInFirstPerson = true;

				var propInfo = jacket.GetModel().GetPropData();
				if ( propInfo.ParentBodyGroupName != null )
				{
					jacket.SetBodyGroup( propInfo.ParentBodyGroupName, propInfo.ParentBodyGroupValue );
				}
				else
				{
					jacket.SetBodyGroup( "Chest", 0 );
				}
			}

			if ( true )
			{
				var model = Rand.FromArray( new[]
				{
				"models/citizen_clothes/shoes/trainers.vmdl",
				"models/citizen_clothes/shoes/shoes.workboots.vmdl"
			} );

				shoes = new ModelEntity();
				shoes.SetModel( model );
				shoes.SetParent( this, true );
				shoes.EnableShadowInFirstPerson = true;
				shoes.EnableHideInFirstPerson = true;

				shoes.SetBodyGroup( "Feet", 1 );
			}

			if ( true )
			{
				var model = Rand.FromArray( new[]
				{
				"models/citizen_clothes/hat/hat_hardhat.vmdl",
				"models/citizen_clothes/hat/hat_woolly.vmdl",
				"models/citizen_clothes/hat/hat_securityhelmet.vmdl",
				"models/citizen_clothes/hair/hair_malestyle02.vmdl",
				"models/citizen_clothes/hair/hair_femalebun.black.vmdl",
				"models/citizen_clothes/hat/hat_beret.red.vmdl",
				"models/citizen_clothes/hat/hat.tophat.vmdl",
				"models/citizen_clothes/hat/hat_beret.black.vmdl",
				"models/citizen_clothes/hat/hat_cap.vmdl",
				"models/citizen_clothes/hat/hat_leathercap.vmdl",
				"models/citizen_clothes/hat/hat_leathercapnobadge.vmdl",
				"models/citizen_clothes/hat/hat_securityhelmetnostrap.vmdl",
				"models/citizen_clothes/hat/hat_service.vmdl",
				"models/citizen_clothes/hat/hat_uniform.police.vmdl",
				"models/citizen_clothes/hat/hat_woollybobble.vmdl",
			} );

				hat = new ModelEntity();
				hat.SetModel( model );
				//hat.SetParent( this, true );
				hat.EnableShadowInFirstPerson = true;
				hat.EnableHideInFirstPerson = true;
			}
		}
	}
}
