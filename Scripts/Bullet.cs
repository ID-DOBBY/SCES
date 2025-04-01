using Godot;
using System;

public partial class Bullet : Node2D
{
	[Export] public float Speed = 500f; // Speed of the bullet
	[Export] public float Lifetime = 2f; // Time before the bullet is deleted

	private Vector2 _velocity;

	public void Initialize(Vector2 direction)
	{
		_velocity = direction.Normalized() * Speed;
		Rotation = direction.Angle();
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;

		Lifetime -= (float)delta;
		if (Lifetime <= 0)
			QueueFree();
	}
}
