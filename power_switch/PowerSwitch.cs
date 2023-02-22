using Godot;
using System;
using System.Collections;
using System.Collections.Generic;


namespace EnergyBarrier
{
	public partial class PowerSwitch : Area3D
	{
		[Signal]
		public delegate void SPowerStatusChangedEventHandler(bool isProvidingPower);

		[Export]
		private bool _initialPowerState = true;

		public bool IsProvidingPower{ private set; get; }

		
		public override void _Ready()
		{
			Switch(_initialPowerState);
		}

		private void OnBodyEntered(Node3D body)
		{
			Switch(!IsProvidingPower);
		}


		private void Switch(bool switchOn)
		{
			if (IsProvidingPower != switchOn)
			{
				IsProvidingPower = switchOn;
				EmitSignal(SignalName.SPowerStatusChanged, IsProvidingPower);
			}
		}
	}
}


