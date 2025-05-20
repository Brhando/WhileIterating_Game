using Godot;
using System;

public partial class ShortSword : Area2D
{	
	
	private bool _playerInRange = false; //used to track when a player is close enough to interact
	private bool _hasShownDialogue = false; //used to track if init dialogue was displayed (only display once)
	//private DialogUI _dialogUi;
	
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
		
		//store a reference so we don't have to look it up every time
		//_dialogUi = GetNode<DialogUI>("/root/ResourceArea_Start/DialogUI");
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
	/*private async void ShowIntroDialogue()
	{
		_dialogUi.ShowMessage("The trees whisper...", 2f);
		await ToSignal(GetTree().CreateTimer(2.5f), "timeout");

		_dialogUi.ShowMessage("Your class is what you wield, not what you're born into.", 3f);
		await ToSignal(GetTree().CreateTimer(3.5f), "timeout");

		_dialogUi.ShowMessage("...Choose wisely.", 2f);
		await ToSignal(GetTree().CreateTimer(2.5f), "timeout");
	}*/
	
	public override void _Process(double delta)
	{
		
		if (_playerInRange && !_hasShownDialogue)
		{
			_hasShownDialogue = true;
			//ShowIntroDialogue();
		}
		if (_playerInRange && Input.IsActionJustPressed("interact"))
		{
			GD.Print("Sword picked up!");
			GetParent().GetNode<Player>("Player").SetClass("Sword Swinger");
			QueueFree(); // Remove sword
		}
	}
}
