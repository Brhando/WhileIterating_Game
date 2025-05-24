using Godot;
using System;

public partial class Town0 : Node2D
{
    private bool _playerInTravelAreaMain = false;
    private bool _playerInTravelArea0 = false;
    private bool _playerInTravelArea1 = false;
    
    private CanvasLayer _inventoryUi;
    private Label _playerUi;
    private Area2D _enterMain, _enterSmall0, _enterSmall1;
    private AudioStreamPlayer2D _audio;
    
    private void BodyEntered(Node2D body)
    {
        if (body.Name != "Player") return;
        
        //Check which area the player is in
        if (_enterMain.GetOverlappingBodies().Contains(body))
            _playerInTravelAreaMain = true;
        else if (_enterSmall0.GetOverlappingBodies().Contains(body))
            _playerInTravelArea0 = true;
        else if (_enterSmall1.GetOverlappingBodies().Contains(body))
            _playerInTravelArea1 = true;
    }

    private void BodyExited(Node2D body)
    {
        //check which area the player is leaving
        if (body.Name != "Player") return;

        if (!_enterMain.GetOverlappingBodies().Contains(body))
            _playerInTravelAreaMain = false;
        if (!_enterSmall0.GetOverlappingBodies().Contains(body))
            _playerInTravelArea0 = false;
        if (!_enterSmall1.GetOverlappingBodies().Contains(body))
            _playerInTravelArea1 = false;
    }
    
    public override void _Ready()
    {
        _inventoryUi = GetNode<CanvasLayer>("InventoryUI/CanvasLayer");
        _playerUi = GetNode<Label>("PlayerUI/CanvasLayer/Panel/Label");
        _enterMain = GetNode<Area2D>("EnterMain");
        _enterSmall0 = GetNode<Area2D>("EnterSmall0");
        _enterSmall1 = GetNode<Area2D>("EnterSmall1");
        _audio = GetNode<AudioStreamPlayer2D>("DoorOpen");
        
        _audio.Finished += OnAudioFinished;
        
        _enterMain.Connect("body_entered", new Callable(this, nameof(BodyEntered)));
        _enterMain.Connect("body_exited", new Callable(this, nameof(BodyExited)));
        _enterSmall0.Connect("body_entered", new Callable(this, nameof(BodyEntered)));
        _enterSmall0.Connect("body_exited", new Callable(this, nameof(BodyExited)));
        _enterSmall1.Connect("body_entered", new Callable(this, nameof(BodyEntered)));
        _enterSmall1.Connect("body_exited", new Callable(this, nameof(BodyExited)));

        _inventoryUi.Visible = false;
        _playerUi.Visible = false;
        
        // Move the player to that spawn point
        var player = GetNode<Node2D>("Player");
        if (SceneData.DataInstance.GetSpawnPointName() != "default")
        {
            var spawnPoint = GetNode<Node2D>(SceneData.DataInstance.SpawnPointName);
            player.GlobalPosition = spawnPoint.GlobalPosition;
        }
    }
    
    //change scene after audio has finished
    private void OnAudioFinished()
    {
        if (_playerInTravelAreaMain)
            GetTree().ChangeSceneToFile("Scenes/Areas Inside/main_house_internal.tscn");
        else if (_playerInTravelArea0)
            GetTree().ChangeSceneToFile("Scenes/Areas Inside/small_internal_0.tscn");
        else if (_playerInTravelArea1)
            GetTree().ChangeSceneToFile("Scenes/Areas Inside/small_internal_1.tscn");
    }

    private void ShowMessage(string msg)
    {   
        _playerUi.Visible = true;
        _playerUi.Text = msg;
    }

    private void ChangeScene()
    {
        _audio.Play(); //overloaded .Finished to handle scene change (see OnAudioFinished func)
    }

    public override void _Process(double delta)
    {
        if (_playerInTravelAreaMain || _playerInTravelArea0 || _playerInTravelArea1)
        {
            ShowMessage("Press 'E' to enter.");
            if (Input.IsActionJustPressed("interact"))
            {
                ChangeScene();
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
