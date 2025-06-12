using Godot;

public partial class HarvestableResource : Area2D
{
    [Export] public string ItemID = "";
    [Export] public int DropAmount = 1;
    [Export] public int MaxHealth = 5;

    private int _currentHealth;
    private bool _playerInRange = false;
    private AudioStreamPlayer2D _audio;

    private void OnBodyEntered(Node body)
    {
        if (body.Name == "Player")
            _playerInRange = true;
    }

    private void OnBodyExited(Node body)
    {
        if (body.Name == "Player")
            _playerInRange = false;
    }

    public override void _Ready()
    {
        _currentHealth = MaxHealth;
        _audio = GetNode<AudioStreamPlayer2D>("Swing");
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
    }

    public override void _Input(InputEvent @event)
    {
        if (_playerInRange && Input.IsActionJustPressed("interact"))
        {
            _audio.Play();
            _currentHealth--;

            if (_currentHealth <= 0)
            {
                GameManager.Instance.AddItem(ItemID, DropAmount);
                QueueFree();
            }
        }
    }
}