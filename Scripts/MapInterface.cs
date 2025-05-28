using Godot;
using System;

public partial class MapInterface : Node2D
{
    private Button _button1;
    private Button _button2;
    private Button _button3;
    private Button _button4;
    private Panel _playerUi;
    private bool _isNight = false;
        public override void _Ready()
        {
            _button1 = GetNode<Button>("Button1");
            _button2 = GetNode<Button>("Button2");
            _button3 = GetNode<Button>("Button3");
            _button4 = GetNode<Button>("Button4");
            _playerUi = GetNode<Panel>("PlayerUI/CanvasLayer/Panel");
            
            _button1.Pressed += OnButton1Pressed;
            _button2.Pressed += OnButton2Pressed;
            _button3.Pressed += OnButton3Pressed;
            _button4.Pressed += OnButton4Pressed;
            _playerUi.Visible = false;
            
            CheckTimeOfDay();
            if (_isNight)
            {
                _button2.Disabled = true;
                _button3.Disabled = true;
                _button4.Disabled = true;
            }
            else
            {
                _button2.Disabled = false;
                _button3.Disabled = false;
                _button4.Disabled = false;
            }
        }

        private void OnButton4Pressed()
        {
            GameManager.Instance.Travel("Town");
            GD.Print("Time advanced!");
            GetTree().ChangeSceneToFile("res://Scenes/town_0.tscn");
            
        }

        private void OnButton1Pressed()
        {
            GameManager.Instance.Travel("Home");
            GD.Print("Time advanced!");
            GetTree().ChangeSceneToFile("res://Scenes/home_base.tscn");
        }
        
        private void OnButton3Pressed()
        {
            GameManager.Instance.Travel("Dungeon");
            GD.Print("Time advanced!");
            GetTree().ChangeSceneToFile("res://Scenes/battle_scene.tscn");
        }
        
        private void OnButton2Pressed()
        {
            GameManager.Instance.Travel("ResourceArea");
            GD.Print("Time advanced!");
            GetTree().ChangeSceneToFile("res://Scenes/resource_area_0.tscn");
        }
        
        //func to check the time and change the night flag
        private void CheckTimeOfDay()
        {
            _isNight = GameManager.Instance.CurrentTimeOfDay == GameManager.TimeOfDay.Night;
        }
}
