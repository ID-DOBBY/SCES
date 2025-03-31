using Godot;


public partial class GunPivot : Node2D
{
	[Export] public float Distance = 45f; // Distance from the player
	[Export] public float RotationSpeed = 10f; // How fast it follows the cursor
	
	private Node2D _player; 

	public override void _Ready()
	{
		_player = GetParent<Node2D>(); // The player should be the parent
	}

	public override void _Process(double delta)
	{
		if (_player == null) return;

		// Get cursor position in world space
		Vector2 mousePos = GetViewport().GetMousePosition();
		Vector2 playerPos = _player.GlobalPosition;

		// Calculate direction to mouse
		Vector2 direction = (mousePos - playerPos).Normalized();

		// Set weapon holder position relative to player
		GlobalPosition = playerPos + direction * Distance;

		// Smoothly rotate to look at the mouse
		float targetRotation = direction.Angle();
		Rotation = Mathf.LerpAngle(Rotation, targetRotation, (float)delta * RotationSpeed);
	}
}
