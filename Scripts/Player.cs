using Godot;

public partial class Player : CharacterBody2D
{
	
	float ax = 0f; //Acceleration X axis
	float dx = 0f; //Velocity X axis
	
	float ay = 0f; //Acceleration Y axis
	float dy = 0f; //Velocity Y Axis
	
	float amax = 20f;//Max acceleration
	float aspeed = 5f; //How fast the player accelerates
	float dmax = 350f;//Max Velocity
	float resistance = -35f; //How fast the player decelerates
	bool moveUp = false; //Leave false
	bool moveSide = false; //Leave false
	private AnimatedSprite2D _animatedSprite;
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	public void GetInput()
	{
		if(Input.IsActionPressed("ch_right") || Input.IsActionPressed("ch_left"))
		{
			moveSide = true;
			if(Input.IsActionPressed("ch_right") && Input.IsActionPressed("ch_left"))
			{
				_animatedSprite.Play("Idle");
				moveSide = false;
			}
			else if (Input.IsActionPressed("ch_right"))
			{
				_animatedSprite.Play("WalkingSide");
				_animatedSprite.FlipH = false;
				if(ax <= 10)
				{
					ax = ax + aspeed;
				}
			}
			else if (Input.IsActionPressed("ch_left"))
			{
				_animatedSprite.Play("WalkingSide");
				_animatedSprite.FlipH = true;
				if(ax >= -10)
				{
					ax = ax - aspeed;
				}
			}
		}
		else
		{
			moveSide = false;
		}
		
		if(Input.IsActionPressed("ch_down") || Input.IsActionPressed("ch_up"))
		{
			moveUp = true;
			if(Input.IsActionPressed("ch_down") && Input.IsActionPressed("ch_up"))
			{
				_animatedSprite.Play("Idle");
				moveUp = false;
			}
			if(Input.IsActionPressed("ch_down"))
			{
				if(moveSide == false)
				{
				_animatedSprite.Play("WalkingUp");
				}
				if(ay <= 10)
				{
					ay = ay + aspeed;
				}
			}
			else if(Input.IsActionPressed("ch_up"))
			{
				if(moveSide == false)
				{
				_animatedSprite.Play("WalkingUp");
				}
				if(ay >= -10)
				{
					ay = ay - aspeed;
				}
			}
		}
		else
		{
			moveUp = false;
		}
		
		if(moveUp == false && moveSide == false)
		{
			_animatedSprite.Play("Idle");
		}
	}
	
	public void CalcMovement()
	{
		/*
		if((dx+dy) >= dmax || (-dx-dy) >= dmax || (dx-dy) >= dmax || (-dx+dy) >= dmax)
		{
			if(dx < 0)
			{
				if (ax > 0)
				{
					dx = dx+ax;
				}
			}
			else if (dx > 0)
			{
				if (ax < 0)
				{
					dx = dx+ax;
				}
			}
			
			if(dy < 0)
			{
				if (ay > 0)
				{
					dy = dy+ay;
				}
			}
			else if (dy > 0)
			{
				if (ay < 0)
				{
					dy = dy+ay;
				}
			}
		}
		
		else
		{
		*/
			if((dx+ax) <= dmax && (dx+ax) >= (-dmax))
			{
				dx = dx+ax;
			}
			if((dy+ay) <= dmax && (dy+ay) >= (-dmax))
			{
				dy = dy+ay;
			}
		//}
		
		if ((dx != 0) || (dy != 0))
		{
			if (moveUp == false)
			{
				if(dy == 0)
				{
					ay = 0;
				}
				
				if(dy != 0)
				{
					if(dy>0)
					{
						if(dy+ay>0)
						{
							ay = resistance;
						}
						else
						{
							ay = 0;
							dy = 0;
						}
					}
					else
					{
						if(dy+ay<0)
						{
							ay = -resistance;
						}
						else
						{
							ay = 0;
							dy = 0;
						}
					}
				}
			}
			
			if(moveSide == false)
			{
				if(dx == 0)
				{
					ax = 0;
				}
				
				if(dx != 0)
				{
					if(dx>0)
					{
						if(dx+ax>0)
						{
							ax = resistance;
						}
						else
						{
							ax = 0;
							dx = 0;
						}
					}
					else
					{
						if(dx+ax<0)
						{
							ax = -resistance;
						}
						else
						{
							ax = 0;
							dx = 0;
						}
					}
				}
			}
		}
		
		Vector2 velocity = Vector2.Zero;
		velocity = new Vector2(dx, dy);
		GD.Print("velocity: "+ velocity);
		Velocity = velocity;
	}
	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		CalcMovement();
		MoveAndSlide();
	}
}
