using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public float Speed = 800f; // Speed of the bullet
	[Export] public float Lifetime = 3f; // Time before the bullet is deleted
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
			
			//GD.Print($"Bullet GX: {GlobalPosition.X} GY: {GlobalPosition.Y} PX: {Position.X} Y: {Position.Y}");
			
			zombie.TakeDamage(1);
			QueueFree();
		}
		if(body.IsInGroup("rock")){
			body.Call("_drop_stone");
			GD.Print(body);
			QueueFree();
		}
			
		
	}
}
