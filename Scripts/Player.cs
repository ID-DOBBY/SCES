using Godot;

public partial class Player : CharacterBody2D
{
	
	float ax = 0f; //Acceleration X axis
	float dx = 0f; //Velocity X axis
	
	float ay = 0f; //Acceleration Y axis
	float dy = 0f; //Velocity Y Axis
	
	float amax = 20f;//Max acceleration
	float aspeed = 6f; //How fast the player accelerates
	float dmax = 400f;//Max Velocity
	float resistance = -32f; //How fast the player decelerates
	int moveUp = 0; //Leave 0
	int moveSide = 0; //Leave 0
	private AnimatedSprite2D _animatedSprite;
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	public void GetInput()
	{
		if(Input.IsActionPressed("ch_right") || Input.IsActionPressed("ch_left"))
		{
			if(Input.IsActionPressed("ch_right") && Input.IsActionPressed("ch_left"))
			{
				_animatedSprite.Play("Idle");
				moveSide = 0;
			}
			else if (Input.IsActionPressed("ch_right"))
			{
				moveSide = 1;
				_animatedSprite.Play("WalkingSide");
				_animatedSprite.FlipH = false;
				if(ax <= amax)
				{
					ax = ax + aspeed;
				}
			}
			else if (Input.IsActionPressed("ch_left"))
			{
				moveSide = -1;
				_animatedSprite.Play("WalkingSide");
				_animatedSprite.FlipH = true;
				if(ax >= -amax)
				{
					ax = ax - aspeed;
				}
			}
		}
		else
		{
			moveSide = 0;
		}
		
		if(Input.IsActionPressed("ch_down") || Input.IsActionPressed("ch_up"))
		{
			if(Input.IsActionPressed("ch_down") && Input.IsActionPressed("ch_up"))
			{
				_animatedSprite.Play("Idle");
				moveUp = 0;
			}
			if(Input.IsActionPressed("ch_down"))
			{
				moveUp = -1;
				if(moveSide == 0)
				{
				_animatedSprite.Play("WalkingUp");
				}
				if(ay <= amax)
				{
					ay = ay + aspeed;
				}
			}
			else if(Input.IsActionPressed("ch_up"))
			{
				moveUp = 1;
				if(moveSide == 0)
				{
				_animatedSprite.Play("WalkingUp");
				}
				if(ay >= -amax)
				{
					ay = ay - aspeed;
				}
			}
		}
		else
		{
			moveUp = 0;
		}
		
		if(moveUp == 0 && moveSide == 0)
		{
			_animatedSprite.Play("Idle");
		}
	}
	
	public void CalcMovement()
	{
		if(moveSide != 0 && moveUp != 0)
		{
			if(dx>(dmax/2) || dx<(-dmax/2))
			{
				if(dx>(dmax/2))
				{
					dy = (dx/2)*-moveUp;
					dx = dmax/2;
				}
				if(dx<(-dmax/2))
				{
					dy = (dx/2)*moveUp;
					dx = -dmax/2;
				}
			}
			
			if(dy>(dmax/2) || dy<(-dmax/2))
			{
				if(dy>(dmax/2))
				{
					dx = (dy/2)*moveSide;
					dy = dmax/2;
				}
				if(dy<(-dmax/2))
				{
					dx = (dy/2)*-moveSide;
					dy = -dmax/2;
				}
			}
			
			if((dx+ax) <= (dmax/2) && (dx+ax) >= (-dmax/2))
			{
				dx = dx+ax;
			}
			if((dy+ay) <= (dmax/2) && (dy+ay) >= (-dmax/2))
			{
				dy = dy+ay;
			}
		}
		else
		{
			if((dx+ax) <= dmax && (dx+ax) >= (-dmax))
			{
				dx = dx+ax;
			}
			else if (dx < dmax && dx > -dmax)
			{
				if(dx>0)
				{
					dx = dmax;
				}
				if(dx<0)
				{
					dx = -dmax;
				}
			}
			if((dy+ay) <= dmax && (dy+ay) >= (-dmax))
			{
				dy = dy+ay;
			}
			else if (dy < dmax && dy > -dmax)
			{
				if(dy>0)
				{
					dy = dmax;
				}
				if(dy<0)
				{
					dy = -dmax;
				}
			}
		}
		
		if ((dx != 0) || (dy != 0))
		{
			if (moveUp == 0)
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
			
			if(moveSide == 0)
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
