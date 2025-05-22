using Godot;
using System;

public partial class StartingArea : Node2D
{
	private Area2D _exit;
	private Panel _playerUi;
	private Label _uiLabel;
	
	private bool _playerInTravelArea = false;
	
	private void BodyEntered(Node body)
	{
		if (body.Name == "Player")
			_playerInTravelArea = true; //player is in range, and can interact with the item
	}

	private void BodyExited(Node body)
	{
		if (body.Name == "Player")
			_playerInTravelArea = false; //player no longer in range
	}

	public override void _Ready()
	{
		_playerUi = GetNode<Panel>("PlayerUI/CanvasLayer/Panel");
		_uiLabel = GetNode<Label>("PlayerUI/CanvasLayer/Panel/Label");
		
		_playerUi.Visible = false;
		_uiLabel.Visible = false;
		
		_exit = GetNode<Area2D>("Exit");
		_exit.Connect("body_entered", new Callable(this, nameof(BodyEntered)));
		_exit.Connect("body_exited", new Callable(this, nameof(BodyExited)));
	}
	public override void _Process(double delta)
	{
		
		if (Input.IsActionJustPressed("escape"))
		{
			GetTree().Quit(1);
		}

		if (GetNodeOrNull("ShortSword") == null) 
		{
			_playerUi.Visible = true;
		}
		
		if (_playerInTravelArea)
		{
			_uiLabel.Visible = true;
			_uiLabel.Text = "Press 'E' to travel.";
			if (Input.IsActionJustPressed("interact"))
			{
				GetTree().ChangeSceneToFile("res://Scenes/home_base.tscn");
			}
		}
		else
		{
			_uiLabel.Visible = false;
		}
	}
	
}
