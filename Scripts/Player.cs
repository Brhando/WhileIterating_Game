using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 100f;
    private AnimatedSprite2D _anim;
    private AudioStreamPlayer2D _audio;
    private double _stepCooldown = 0.0;
    
    
    //used to track direction the player is facing
    //default to right
    private Vector2 _facingDirection = Vector2.Right;
    
    //used to track the class the player chooses (based on weapon selection)
    //set to empty at first
    private string _chosenClass = "";
    
    //function for setting the combat class of the player
    public void SetClass(string className)
    {
        _chosenClass = className;
        GD.Print("Player class is now: " + _chosenClass);
    }
    
    public override void _Ready()
    {
        _anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _audio = GetNode<AudioStreamPlayer2D>("StepSound");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Vector2.Zero;
        
        _stepCooldown -= delta;
        
        //movement
        if (Input.IsActionPressed("move_right"))
            direction.X += 1;
        if (Input.IsActionPressed("move_left"))
            direction.X -= 1;
        if (Input.IsActionPressed("move_down"))
            direction.Y += 1;
        if (Input.IsActionPressed("move_up"))
            direction.Y -= 1;

        direction = direction.Normalized(); //make sure diagonal movement isn't faster/slower
        Velocity = direction * Speed;
        MoveAndSlide();

        if (direction != Vector2.Zero)
        {
            _facingDirection = direction;

            if (_stepCooldown <= 0.0)
            {
                _audio.Play();
                _stepCooldown = 0.5;
            }

            // Direction-based animation (prioritize vertical over horizontal and vice versa)
            if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
            {
                _anim.Play(direction.X > 0 ? "Walk_right" : "Walk_left");
            }
            else
            {
                _anim.Play(direction.Y > 0 ? "Walk_down" : "Walk_up");
            }
        }
        else
        {
            // Standing animation based on last direction
            if (Mathf.Abs(_facingDirection.X) > Mathf.Abs(_facingDirection.Y))
            {
                _anim.Play(_facingDirection.X > 0 ? "Stand_right" : "Stand_left");  
            }
            else
            {
                _anim.Play(_facingDirection.Y > 0 ? "Stand_down" : "Stand_up");
            }
            
        }

        
    }
}