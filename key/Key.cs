using Godot;
using System;
using System.Collections;
using System.Collections.Generic;


namespace EnergyBarrier
{
	public partial class Key : Area3D
	{
		[Signal]
		public delegate void SPickedUpEventHandler(Key key);

		[Export]
		private Barrier _barrier;

		public override void _Ready()
		{
			_barrier.RegisterKey(this);
		}

		public override void _PhysicsProcess(double delta)
		{
			RotateY(0.3f * (float)delta);
		}
		
		private void OnBodyEntered(Node3D body)
		{
			PickedUp();
		}

		private void PickedUp()
		{
			Hide();
			GetNode<CollisionShape3D>("CollisionShape3D").SetDeferred("disabled", true);
			EmitSignal(SignalName.SPickedUp, this);
		}
	}
}


