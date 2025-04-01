using Godot;
using System;

public partial class Blood : GpuParticles2D
{
	public void Initialize()
	{
		Restart();
		GD.Print($"PLEASE JUST WORK FFS X: {GlobalPosition.X} Y: {GlobalPosition.Y}");
	}
}
