sdusing Godot;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;
	private AnimatedSprite2D _animatedSprite;
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	public void GetInput()
	{

		Vector2 inputDirection = Input.GetVector("ch_left", "ch_right", "ch_up", "ch_down");
		Velocity = inputDirection * Speed;
		if(Input.IsActionPressed("ch_right") || Input.IsActionPressed("ch_left"))
		{
			if(Input.IsActionPressed("ch_right") && Input.IsActionPressed("ch_left"))
			{
				_animatedSprite.Play("Idle");
			}
			else if (Input.IsActionPressed("ch_right"))
			{
				_animatedSprite.Play("WalkingSide");
				_animatedSprite.FlipH = false;
			}
			else if (Input.IsActionPressed("ch_left"))
			{
				_animatedSprite.Play("WalkingSide");
				_animatedSprite.FlipH = true;
			}
		}
		else if(Input.IsActionPressed("ch_down") || Input.IsActionPressed("ch_up"))
		{
			if(Input.IsActionPressed("ch_down") && Input.IsActionPressed("ch_up"))
			{
				_animatedSprite.Play("Idle");
			}
			if(Input.IsActionPressed("ch_down") || Input.IsActionPressed("ch_up"))
			{
				_animatedSprite.Play("WalkingUp");
			}
		}
		else
		{
			_animatedSprite.Play("Idle");
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
		/*
		if(Position.X < 0)
		{
		GD.Print("X below");
		Position = new Vector2(1, Position.Y);
		}
		
		if(Position.Y < 0)
		{
		GD.Print("Y below");
		Position = new Vector2(Position.X, 1);
		}
		*/
	}
}
