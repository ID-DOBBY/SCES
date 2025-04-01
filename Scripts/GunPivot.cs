using Godot;

public partial class GunPivot : Node2D
{
	[Export] public float Distance = 42f; //Gun distance from pivot (Should be lower than bullet distance)
	[Export] public float RotationSpeed = 10f; //How fast the gun rotates
	[Export] public PackedScene BulletScene;
	[Export] public float FireRate = 0.2f; //Gun fire speed
	[Export] public float bulletDistanceFromPivot = 70f; //Bullet distance from the pivot (Should be higher than gun distance)
	
	private Node2D _player;
	private Node2D _bulletContainer;
	private float _fireCooldown = 0f; //Honestly idk what this does so just leave it

	public override void _Ready()
	{
		_player = GetParent<Node2D>();

		// Find the BulletContainer node in the scene
		_bulletContainer = GetTree().CurrentScene.GetNodeOrNull<Node2D>("BulletContainer"); //Checking bullet scene is attached
		if (_bulletContainer == null)
		{
			GD.PrintErr("BulletContainer node not found! Bullets will be added to the current scene.");
		}
	}

	public override void _Process(double delta)
	{
		if (_player == null) return;

		Vector2 mousePos = GetGlobalMousePosition();
		Vector2 playerPos = _player.GlobalPosition;
		Vector2 direction = (mousePos - playerPos).Normalized();

		Position = direction * Distance;

		float targetRotation = direction.Angle();
		Rotation = Mathf.LerpAngle(Rotation, targetRotation, (float)delta * RotationSpeed);

		_fireCooldown -= (float)delta;
		if (Input.IsActionPressed("shoot") && _fireCooldown <= 0)
		{
			Shoot(direction);
			_fireCooldown = FireRate;
		}
	}

	private void Shoot(Vector2 direction) //Called when shoot input (Left click) is triggered
	{
		if (BulletScene == null)
		{
			GD.PrintErr("BulletScene not assigned!");
			return;
		}

		Bullet bullet = (Bullet)BulletScene.Instantiate();
		bullet.GlobalPosition = GlobalPosition + (direction * bulletDistanceFromPivot);
		bullet.Initialize(direction);

		// Add to BulletContainer if available, otherwise add to the current scene
		if (_bulletContainer != null)
			_bulletContainer.AddChild(bullet);
		else
			GetTree().CurrentScene.AddChild(bullet);
	}
}
