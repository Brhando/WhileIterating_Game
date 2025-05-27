using Godot;
using System;

public partial class MapInterface : Node2D
{
    public partial class OverworldMap : Node
    {
        public override void _Ready()
        {
            GetNode<Button>("Button1").Pressed += OnButton1Pressed;
            GetNode<Button>("Button2").Pressed += OnButton2Pressed;
            GetNode<Button>("Button3").Pressed += OnButton3Pressed;
            GetNode<Button>("Button4").Pressed += OnButton4Pressed;
        }

        private void OnButton2Pressed()
        {
            GetTree().ChangeSceneToFile("res://Scenes/town_0.tscn");
        }

        private void OnButton1Pressed()
        {
            GetTree().ChangeSceneToFile("res://Scenes/home_base.tscn");
        }
        
        private void OnButton3Pressed()
        {
            GetTree().ChangeSceneToFile("res://Scenes/battle_scene.tscn");
        }
        
        private void OnButton4Pressed()
        {
            GetTree().ChangeSceneToFile("res://Scenes/resource_area_0.tscn");
        }
    }
}
