using Godot;
using System;

public partial class StartingArea : Node2D
{
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("escape"))
		{
			GetTree().Quit(1);
		}
	}
	
}
