using Godot;

public partial class Zombie : CharacterBody2D
{
	public CharacterBody2D Player;
	double TimeA;
	double TimeB;
	[Export] double hitCooldown = 1000; //in Milliseconds
	[Export] public float Speed = 100f;
	[Export] public float DetectionRadius = 200f;
	[Export] public int Health = 5;
	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		AddToGroup("Enemy");
		
		// Auto-assign the player if not manually assigned
		if (Player == null)
		{
			Player = GetTree().GetFirstNodeInGroup("Player") as CharacterBody2D;
			if (Player == null)
			{
				GD.PrintErr("Player not found! Make sure the player is in the 'Player' group.");
			}
		}
	}
	
	
	async public void TakeDamage(int damage)
	{
		Health -= damage;
		GD.Print("Zombie took damage! Health: " + Health);
		_animatedSprite.Modulate = new Color(255,0,0);
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		_animatedSprite.Modulate = new Color(1,1,1);
		if (Health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		GD.Print("Zombie died!");
		QueueFree(); // Removes the zombie from the scene
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Player == null) 
		{
			GD.Print("Player is Null");
			return;
		}

		float distance = GlobalPosition.DistanceTo(Player.GlobalPosition);

		if (distance <= DetectionRadius)
		{
			// Chase the player
			Vector2 direction = (Player.GlobalPosition - GlobalPosition).Normalized();
			if (direction.X > 0)
			{
				_animatedSprite.Play("ZombWalk");
				_animatedSprite.FlipH = false;
			}
			else if (direction.X < 0)
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
		
		int collisionCount = GetSlideCollisionCount();
		for (int i = 0; i < collisionCount; i++)
		{
			KinematicCollision2D collision = GetSlideCollision(i);
			var collider = collision.GetCollider() as Node;
			if(collider.IsInGroup("Player"))
			{
				TimeA = Time.GetTicksMsec();
				GD.Print($"TimeA: {TimeA}  TimeB:{TimeB}");
				if(TimeB != 0)
				{
					if((TimeA-TimeB)>hitCooldown)
					{
						GD.Print("Hit!");
						Player.Call("TakeDamage");
						TimeB = TimeA;
					}
					
				}
				else
				{
					TimeB = TimeA;
				}
			}
		}
	}
}
