using Godot;
using System;

public partial class Boulder : Area2D
{
    private int _boulderHealth = 5;
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
            //decrement boulder health per "hit"
            _boulderHealth--;
            
            //clear boulder and collect rocks once boulder is out of health
            if (_boulderHealth <= 0)
            {
                GameManager.Instance.AddItem("Stone", 5);
                QueueFree(); 
            }
        }
    }








}
