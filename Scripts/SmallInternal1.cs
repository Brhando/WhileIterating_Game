using Godot;
using System;

public partial class SmallInternal1 : Node2D
{   
    private bool _playerInTravelArea = false;

    private CanvasLayer _inventoryUi;
    private Label _playerUi;
    private Area2D _exitArea;
    private AudioStreamPlayer2D _audio;
    
    
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
        _inventoryUi = GetNode<CanvasLayer>("InventoryUI/CanvasLayer");
        _playerUi = GetNode<Label>("PlayerUI/CanvasLayer/Panel/Label");
        _exitArea = GetNode<Area2D>("ExitArea");
        _audio = GetNode<AudioStreamPlayer2D>("DoorClose");
        _audio.Finished += OnAudioFinished;
        
        _inventoryUi.Visible = false;
        _playerUi.Visible = false;
        
        _exitArea.Connect("body_entered", new Callable(this, nameof(BodyEntered)));
        _exitArea.Connect("body_exited", new Callable(this, nameof(BodyExited)));
    }
    
    private void ShowMessage(string msg)
    {   
        _playerUi.Visible = true;
        _playerUi.Text = msg;
    }
    
    //overloaded .Finished to handle the scene chang AFTER the audio completes
    private void OnAudioFinished()
    {
        SceneData.DataInstance.ChangeSpawnPointName("Spawn2");
        GetTree().ChangeSceneToFile("Scenes/town_0.tscn");
    }
    
    public override void _Process(double delta)
    {
        if (_playerInTravelArea)
        {
            ShowMessage("Press 'E' to exit.");
            
            if (Input.IsActionJustPressed("interact"))
            {
                _audio.Play();
            }
        }
        else
        {
            _playerUi.Visible = false;
        }

        if (Input.IsActionJustPressed("inventory"))
        {
            _inventoryUi.Visible = !_inventoryUi.Visible;
        }
    }
}
