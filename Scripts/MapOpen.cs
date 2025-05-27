using Godot;
using System;

public partial class MapOpen : Area2D
{
    private Label _playerUi;
    public bool PlayerInTravelArea = false;

    public override void _Ready()
    {
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
    }
    
    public void OnBodyEntered(Node body)
    {
        if (body.Name == "Player")
            PlayerInTravelArea = true; //player is in range, and can interact with the item
    }

    public void OnBodyExited(Node body)
    {
        if (body.Name == "Player")
            PlayerInTravelArea = false; //player no longer in range
    }
    
}
