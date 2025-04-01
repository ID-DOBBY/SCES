using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public float Speed = 800f; // Speed of the bullet
	[Export] public float Lifetime = 3f; // Time before the bullet is deleted
	[Export] public PackedScene BloodScene;
	private Node2D _bloodContainer;
	private Vector2 _velocity;

	public void Initialize(Vector2 direction)
	{
		_velocity = direction.Normalized() * Speed;
		Rotation = direction.Angle();
		BodyEntered += OnBodyEntered;

	}

	public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;

		Lifetime -= (float)delta;
		if (Lifetime <= 0)
			QueueFree();
	}
	private void OnBodyEntered(Node2D body)
	{
		GD.Print($"Collided with: {body.Name}");
		if(body is Zombie zombie)
		{
			if (BloodScene == null)
			{
				GD.Print("BLOODSCENE NULL");
			}
			GD.Print($"Bullet GX: {GlobalPosition.X} GY: {GlobalPosition.Y} PX: {Position.X} Y: {Position.Y}");
			Blood blood = (Blood)BloodScene.Instantiate();
			blood.GlobalPosition = GlobalPosition;
			blood.Initialize();
			
			zombie.TakeDamage(1);
			QueueFree();
		}
	}
}
