using Godot;
using System;

public partial class ShortSword : Area2D
{	
	
	private bool _playerInRange = false; //used to track when a player is close enough to interact
	private bool _hasShownDialogue = false; //used to track if dialogue was displayed (only display once)
	private bool _hasShownClassDialogue = false;
	private bool _completedIntro = false;
	
	
	private Panel _dialoguePanel;
	private Label _dialogueLabel;
	
	
	
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
		
		_dialoguePanel = GetNode<Panel>("CanvasLayer/Panel");
		_dialogueLabel = GetNode<Label>("CanvasLayer/Panel/Label");
		
		_dialoguePanel.Visible = false;
		_dialogueLabel.Visible = false;
	}

	private void OnBodyEntered(Node body)
	{
		if (body.Name == "Player")
			_playerInRange = true; //player is in range, and can interact with the item
	}

	private void OnBodyExited(Node body)
	{
		if (body.Name == "Player")
			_playerInRange = false; //player no longer in range
	}
	
	//function for displaying Intro Dialogue
	private async void ShowIntroDialogue()
	{
		_dialoguePanel.Visible = true;
		_dialogueLabel.Visible = true;

		_dialogueLabel.Text = "The trees whisper...";
		await ToSignal(GetTree().CreateTimer(3.5f), "timeout");

		_dialogueLabel.Text = "Your class is what you wield, not what you're born into.";
		await ToSignal(GetTree().CreateTimer(3.5f), "timeout");

		_dialogueLabel.Text = "...Choose wisely.";
		await ToSignal(GetTree().CreateTimer(3.5f), "timeout");
		
		_dialoguePanel.Visible = false;
		_dialogueLabel.Visible = false;
		
		_completedIntro = true;
	}
	
	//function for displaying class selection dialogue
	private async void ShowClassDialogue()
	{
		_dialoguePanel.Visible = true;
		_dialogueLabel.Visible = true;
		
		_dialogueLabel.Text = "The sword will sing in your hands!";
		await ToSignal(GetTree().CreateTimer(3.5f), "timeout");
		
		_dialogueLabel.Text = "Walk with pride \"Sword Swinger\"";
		await ToSignal(GetTree().CreateTimer(3.5f), "timeout");
		
		_dialoguePanel.Visible = false;
		_dialogueLabel.Visible = false;
		
		
		
		PlayerData.Instance.SetClass("Sword Swinger");
		
		QueueFree(); // Remove sword
	}
	
	public override void _Process(double delta)
	{
		
		if (_playerInRange && !_hasShownDialogue)
		{
			ShowIntroDialogue();
			_hasShownDialogue = true;
		}
		if (_playerInRange && Input.IsActionJustPressed("interact") && _completedIntro)
		{
			if (!_hasShownClassDialogue)
			{
				_hasShownClassDialogue = true;
				ShowClassDialogue();
			}
		}
	}
}
