using Godot;
using System;

public partial class Chest : Area2D
{
    private AnimatedSprite2D _anim;
    public bool PlayerInRange = false;
    
    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _anim.Play("default");

        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));

        GameManager.Instance.ActiveChest = this; // Register self
    }
    
    public void OnBodyEntered(Node body)
    {
        if (body.Name == "Player")
            PlayerInRange = true; //player is in range, and can interact with the item
    }

    public void OnBodyExited(Node body)
    {
        if (body.Name == "Player")
            PlayerInRange = false; //player no longer in range
    }
    
    
    //deregister the chest upon exiting the tree
    public override void _ExitTree()
    {
        if (GameManager.Instance.ActiveChest == this)
            GameManager.Instance.ActiveChest = null;
    }
    
}
