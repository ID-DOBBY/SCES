using Godot;

public partial class GunPivot : Node2D
{
	[Export] public float Distance = 42f; //Gun distance from pivot (Should be lower than bullet distance)
	[Export] public float RotationSpeed = 10f; //How fast the gun rotates
	[Export] public PackedScene BulletScene;
	[Export] public float FireRate = 0.2f; //Gun fire speed
	[Export] public float bulletDistanceFromPivot = 70f; //Bullet distance from the pivot (Should be higher than gun distance)
	[Export] public int magSize = 30;
	[Export] public float reloadTime = 2f;
	
	int ammoInMag;
	private Sprite2D _sprite;
	private Node2D _player;
	private Node2D _bulletContainer;
	private float _fireCooldown = 0f; //Honestly idk what this does so just leave it

	public override void _Ready()
	{
		 _sprite = GetNode<Sprite2D>("Sprite2D"); // Get the child Sprite2D node
		_player = GetParent<Node2D>();
		
		ammoInMag = magSize;
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

        var Inv = GetTree().Root.GetNode<Node>("Node/Player").GetNode<Inventory>("Inventory");
        Vector2 mousePos = GetGlobalMousePosition();
		Vector2 playerPos = _player.GlobalPosition;
		Vector2 direction = (mousePos - playerPos).Normalized();
		//GD.Print($"Direction: {direction}");
		
		Position = direction * Distance;
		if(direction.X < 0) //Checking which way the gun is facing so the sprite is not upside down
		{
			_sprite.FlipV = true;
		}
		else
		{
			_sprite.FlipV = false;
		}
		
		float targetRotation = direction.Angle();
		Rotation = Mathf.LerpAngle(Rotation, targetRotation, (float)delta * RotationSpeed);
		

		_fireCooldown -= (float)delta;
		if (Input.IsActionPressed("shoot") && _fireCooldown <= 0)
		{
			//GD.Print($"Bullets in mag: {ammoInMag}"); //Used for debug
			
			if(ammoInMag > 0) //Checking if gun needs to be reloaded
			Shoot(direction);
			else
			Reload();
			
			_fireCooldown = FireRate;
		}
        Label magLabel = GetTree().Root.GetNode<CanvasLayer>("Node/UI").GetNode<Label>("MagLabel");
        magLabel.Text = $"Mag: {ammoInMag}/30";

        Label ammoLabel = GetTree().Root.GetNode<CanvasLayer>("Node/UI").GetNode<Label>("AmmoLabel");
        ammoLabel.Text = $"Ammo: {Inv.ammo}";
    }
	bool isReloading = false;
    private async void Reload()
	{
        var Inv = GetTree().Root.GetNode<Node>("Node/Player").GetNode<Inventory>("Inventory");
        if (isReloading == false)
		{
			if (Inv.ammo != 0)
			{
				GD.Print($"Ammo in Inv: {Inv.ammo}");
				if (Inv.ammo > 30)
				{
					isReloading = true;
					GD.Print("Reloading!");
					await ToSignal(GetTree().CreateTimer(reloadTime), "timeout");
					ammoInMag = magSize;
					Inv.ammo -= magSize;
					isReloading = false;
				}
				else if (Inv.ammo < 30 && Inv.ammo > 0)
				{
					isReloading = true;
					GD.Print("Reloading with whats left!");
					await ToSignal(GetTree().CreateTimer(reloadTime), "timeout");
					ammoInMag = Inv.ammo;
					Inv.ammo = 0;
					isReloading = false;
				}
			}
			else
			{
				GD.Print("No Ammo Left!");
			}
            
        }
    }
    

    private void Shoot(Vector2 direction) //Called when shoot input (Left click) is triggered
	{
		ammoInMag--; //Removing a bullet from mag
		if (BulletScene == null) //Just checking if Bullet scene is attached
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
