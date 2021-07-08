using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace overflow
{
	partial class OverflowPlayer : Player
	{
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

		public override void OnKilled()
		{
			Controller = new NoclipController();
			Camera = new FirstPersonCamera();
			Velocity = Vector3.Zero;

			EnableAllCollisions = false;
			EnableDrawing = false;
		}
	}
}
