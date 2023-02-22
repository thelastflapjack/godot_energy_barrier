using Godot;
using System;
using System.Collections;
using System.Collections.Generic;


namespace EnergyBarrier
{
	public partial class Barrier : Node3D
	{

		public bool IsAccessGranted{ private set; get; }
		public bool IsPowered{ private set; get; }

		private MeshInstance3D _fieldMeshFront;
		private MeshInstance3D _fieldMeshBack;
		private CollisionShape3D _blockerShape;
		private PowerSwitch _powerProvider = null;


		public override void _Ready()
		{
			_fieldMeshBack = GetNode<MeshInstance3D>("FieldMeshBack");
			_fieldMeshFront = GetNode<MeshInstance3D>("FieldMeshFront");
			_blockerShape = GetNode<CollisionShape3D>("Blocker/CollisionShape3D");

			foreach (Node child in GetChildren())
			{
				if (child is PowerSwitch)
				{
					_powerProvider = (PowerSwitch)child;
					Powered(_powerProvider.IsProvidingPower);
					_powerProvider.SPowerStatusChanged += OnPowerProviderStatusChanged;
					break;
				}
			}
		}


		public void RegisterKey(Key newKey)
		{
			newKey.SPickedUp += OnRegisteredKeyPickedUp;
		}


		private void OnRegisteredKeyPickedUp(Key key)
		{
			ChangeAccess(true);
			FadeFieldMaterialColor();
		}

		private void OnPowerProviderStatusChanged(bool powerOn)
		{
			Powered(powerOn);
			FadeFieldMaterialAlpha();
		}
		

		private void FadeFieldMaterialColor()
		{
			float duration = 1.0f;
			float valueFinal = IsAccessGranted ? 0 : 1;
			float valueFrom = !IsAccessGranted ? 0 : 1;

			Tween fadeTween = CreateTween().SetParallel(true);
			fadeTween.SetParallel(true);
			fadeTween.TweenProperty(_fieldMeshBack, "instance_shader_parameters/deniedWeight", valueFinal, duration).From(valueFrom);
			fadeTween.TweenProperty(_fieldMeshFront, "instance_shader_parameters/deniedWeight", valueFinal, duration).From(valueFrom);
			fadeTween.Play();
		}

		private void FadeFieldMaterialAlpha()
		{
			float duration = 1.0f;
			float valueFinal = IsPowered ? 1 : 0;
			float valueFrom = !IsPowered ? 1 : 0;

			Tween fadeTween = GetTree().CreateTween();
			fadeTween.SetParallel(true);
			fadeTween.TweenProperty(_fieldMeshBack, "instance_shader_parameters/poweredWeight", valueFinal, duration).From(valueFrom);
			fadeTween.TweenProperty(_fieldMeshFront, "instance_shader_parameters/poweredWeight", valueFinal, duration).From(valueFrom);
			fadeTween.Play();
		}

		private void ChangeAccess(bool isGrant)
		{
			_blockerShape.SetDeferred("disabled", isGrant);
			IsAccessGranted = isGrant;
		}

		private void Powered(bool powerOn)
		{
			if(powerOn)
			{
				_blockerShape.SetDeferred("disabled", IsAccessGranted);
			}
			else
			{
				_blockerShape.SetDeferred("disabled", true);
			}
			IsPowered = powerOn;
		}
	}
}


