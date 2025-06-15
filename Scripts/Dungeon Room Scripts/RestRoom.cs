using Godot;
using System;

public partial class RestRoom : Node2D
{
    private Area2D _travelArea;
    private Area2D _bedArea;
    private bool _playerInTravelArea;
    private bool _playerInBedArea;
    private bool _playerHealed = false;
    private Label _playerUi;
    private TimeLabel _timeLabel;

    public override void _Ready()
    {
        _travelArea = GetNode<Area2D>("TravelArea");
        _bedArea = GetNode<Area2D>("BedArea");
        _playerUi = GetNode<Label>("PlayerUI/CanvasLayer/Panel/Label");
        _timeLabel = GetNode<TimeLabel>("PlayerUI/CanvasLayer/TimeLabel");
        
        _travelArea.Connect("body_entered", new Callable(this, nameof(BodyEntered)));
        _travelArea.Connect("body_exited", new Callable(this, nameof(BodyExited)));
        _bedArea.Connect("body_entered", new Callable(this, nameof(BodyEntered1)));
        _bedArea.Connect("body_exited", new Callable(this, nameof(BodyExited1)));
    }
    
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
    private void BodyEntered1(Node body)
    {
        if (body.Name == "Player")
            _playerInBedArea = true; //player is in range, and can interact with the item
    }

    private void BodyExited1(Node body)
    {
        if (body.Name == "Player")
            _playerInBedArea = false; //player no longer in range
    }
    
    private void PrintMessage(string message)
    {
        _playerUi.Visible = true;
        _playerUi.Text = message;
    }

    public override void _Process(double delta)
    {
        var message = "";
    
        if (_playerInTravelArea)
        {
            message = "Press 'E' to venture further.";
        
            if (Input.IsActionJustPressed("interact"))
            {
                DungeonRoomManager.Instance.NextRoom();
            }
        }

        if (_playerInBedArea)
        {
            message = "Press 'E' to rest.";

            if (Input.IsActionJustPressed("interact") && !_playerHealed)
            {
                PlayerData.Instance.Heal(Mathf.RoundToInt(PlayerData.Instance.GetPlayerMaxHealth() * 0.33f));
                _playerHealed = true;
                //play audio queue for heal effect
                //update player ui
                _timeLabel.UpdateTimeLabel();
            }
        }
        
        if (message != "")
        {
            PrintMessage(message);
        }
        else
        {
            _playerUi.Visible = false;
        }
    }


}
