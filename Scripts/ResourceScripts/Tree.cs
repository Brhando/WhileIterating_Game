using Godot;
using System;

public partial class Tree : Area2D
{
    private int _treeHealth = 10;
    private bool _playerInRange = false;
    private AudioStreamPlayer2D _audio;
    
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
    
    public override void _Ready()
    {
        _audio = GetNode<AudioStreamPlayer2D>("Swing");
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
    }
    
    public override void _Input(InputEvent @event)
    {
        if (_playerInRange && Input.IsActionJustPressed("interact"))
        {
            //play sound 
            _audio.Play();
            
            //decrement tree health per "hit"
            _treeHealth--;
            
            //clear tree and collect wood once tree is out of health
            if (_treeHealth <= 0)
            {
                GameManager.Instance.AddItem("Wood", 5);
                QueueFree(); 
            }
        }
    }

    
}
