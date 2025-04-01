using Godot;

public partial class Zombie : CharacterBody2D
{
	[Export] public CharacterBody2D Player; // Exported field for manual assignment
	[Export] public float Speed = 100f;
	[Export] public float DetectionRadius = 200f;
	private AnimatedSprite2D _animatedSprite;
	 public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Player == null) return;

		float distance = GlobalPosition.DistanceTo(Player.GlobalPosition);

		if (distance <= DetectionRadius)
		{
			// Chase the player
			Vector2 direction = (Player.GlobalPosition - GlobalPosition).Normalized();
			if(direction.X > 0)
			{
				_animatedSprite.Play("ZombWalk");
				_animatedSprite.FlipH = false;
			}
			if(direction.X < 0)
			{
				_animatedSprite.Play("ZombWalk");
				_animatedSprite.FlipH = true;
			}
			Velocity = direction * Speed;
			MoveAndSlide();
		}
		else
		{
			// Stop moving if out of range
			Velocity = Vector2.Zero;
			_animatedSprite.Play("ZombIdle");
		}
	}
}
