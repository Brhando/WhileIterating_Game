using Godot;
using System;
using System.Text;

public partial class WorkBench : Area2D
{
	public bool PlayerInRange;

	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}
	
	private void OnBodyEntered(Node body)
	{
		if (body.Name == "Player")
			PlayerInRange = true; //player is in range, and can interact with the item
	}

	private void OnBodyExited(Node body)
	{
		if (body.Name == "Player")
			PlayerInRange = false; //player no longer in range
	}
	
	public override void _Process(double delta)
	{
		var workbenchUI = GetNode<WorkBenchUi>("/root/HomeInside/WorkBenchUI");
		if (PlayerInRange && Input.IsActionJustPressed("interact"))
		{
			workbenchUI.Visible = true;
			CallDeferred(nameof(DeferredPopulate), workbenchUI);
		}
		else if (!PlayerInRange)
		{
			workbenchUI.Visible = false;
		}
	}

	private void DeferredPopulate(WorkBenchUi workbenchUI)
	{
		workbenchUI.PopulateRecipes();
	}
	
}
