using Godot;

public partial class Player : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;

	public void GetInput()
	{

		Vector2 inputDirection = Input.GetVector("ch_left", "ch_right", "ch_up", "ch_down");
		Velocity = inputDirection * Speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
}
