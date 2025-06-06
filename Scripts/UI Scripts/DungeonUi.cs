using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class DungeonUi : Node2D
{
    private Label _turnLabel;
    private Label _playerHp;
    private Label _playerStamina;
    private Label _playerBlock;
    private Label _actionLabel;
    private Button _skill1;
    private Button _skill2;
    private Button _skill3;
    private Button _skill4;
    private Button _skill5;
    private Button _storedBuffButton;
    private Button _victoryButton;
    private Button _endTurnButton;

    public override void _Ready()
    {
        _turnLabel = GetNode<Label>("CanvasLayer/Panel_center/Turn");
        _playerHp = GetNode<Label>("CanvasLayer/Panel_left/PlayerHP");
        _playerBlock = GetNode<Label>("CanvasLayer/Panel_left/PlayerBlock");
        _playerStamina = GetNode<Label>("CanvasLayer/Panel_left/PlayerStamina");
        _actionLabel = GetNode<Label>("CanvasLayer/ActionLabel");
        
        _skill1 = GetNode<Button>("CanvasLayer/Skill1");
        _skill2 = GetNode<Button>("CanvasLayer/Skill2");
        _skill3 = GetNode<Button>("CanvasLayer/Skill3");
        _skill4 = GetNode<Button>("CanvasLayer/Skill4");
        _skill5 = GetNode<Button>("CanvasLayer/Skill5");
        _storedBuffButton = GetNode<Button>("CanvasLayer/StoredBuffButton");
        _victoryButton = GetNode<Button>("CanvasLayer/VictoryButton");
        _endTurnButton = GetNode<Button>("CanvasLayer/EndTurnButton");

        _skill1.Pressed += OnSkill1Pressed;
        _skill2.Pressed += OnSkill2Pressed;
        _skill3.Pressed += OnSkill3Pressed;
        _skill4.Pressed += OnSkill4Pressed;
        _skill5.Pressed += OnSkill5Pressed;
        _storedBuffButton.Pressed += OnBuffPressed;
        _victoryButton.Pressed += OnVictoryPressed;
        _endTurnButton.Pressed += OnEndPressed;

        _skill1.Text = PlayerData.Instance?.PlayerSkills["Skill1"].Name;
        _skill2.Text = PlayerData.Instance?.PlayerSkills["Skill2"].Name;
        _skill3.Text = PlayerData.Instance?.PlayerSkills["Skill3"].Name;
        _skill4.Text = PlayerData.Instance?.PlayerSkills["Skill4"].Name;
        _skill5.Text = PlayerData.Instance?.PlayerSkills["Skill5"].Name;
        _victoryButton.Visible = false;
        
        CombatManager.Instance.BattleStateChanged += OnBattleStateChanged;
        if (PlayerData.Instance != null)
            PlayerData.Instance.HealthBlockStaminaChanged += UpdatePlayerLabels;

        if (CombatManager.Instance != null)
            CombatManager.Instance.ActionUsed += UpdateActions;
        
        UpdateActions();
        UpdateBuffIcon();

    }

    

    private void UpdateTurnLabel()
    {
        _turnLabel.Text = CombatManager.Instance.GetBattleState();
    }

    private void UpdateBuffIcon()
    {
        _storedBuffButton.Icon = GD.Load<Texture2D>($"res://Assets/Icons/{PlayerData.Instance.StoredBuff}.png");
    }

    private void UpdateActions()
    {
        _actionLabel.Text = "Actions: " + CombatManager.Instance.GetActionsLeft();
    }

    private void UpdatePlayerLabels()
    {
        _playerHp.Text = "Health: " + PlayerData.Instance.GetPlayerHealth() + " / " + PlayerData.Instance.GetPlayerMaxHealth();
        _playerBlock.Text = "Block: " + PlayerData.Instance.PlayerBlock;
        _playerStamina.Text = "Stamina: " + PlayerData.Instance.GetPlayerStamina() + " / " + PlayerData.Instance.GetPlayerMaxStamina();
    }

    private void UpdateButtons()
    {
        if (CombatManager.Instance.ButtonLock)
        {
            _skill1.Disabled = true;
            _skill2.Disabled = true;
            _skill3.Disabled = true;
            _skill4.Disabled = true;
            _skill5.Disabled = true;
            _storedBuffButton.Disabled = true;
            _endTurnButton.Disabled = true;
            _endTurnButton.Visible = false;
        }
        else
        {
            _skill1.Disabled = false;
            _skill2.Disabled = false;
            _skill3.Disabled = false;
            _skill4.Disabled = false;
            _skill5.Disabled = false;
            _storedBuffButton.Disabled = false;
            _endTurnButton.Disabled = false;
            _endTurnButton.Visible = true;
        }

        if (CombatManager.Instance.VictoryButtonLock)
        {
            _victoryButton.Visible = false;
            _victoryButton.Disabled = true;
        }
        else
        {
            _victoryButton.Visible = true;
            _victoryButton.Disabled = false;
        }
        
    }
    
    private void OnBattleStateChanged()
    {
        UpdateTurnLabel();
        UpdateButtons();
    }
    
    private static void OnSkill1Pressed()
    {
        CombatManager.Instance.ExecutePlayerSkill(PlayerData.Instance.PlayerSkills["Skill1"]);
    }

    private static void OnSkill2Pressed()
    {
        CombatManager.Instance.ExecutePlayerSkill(PlayerData.Instance.PlayerSkills["Skill2"]);
    }

    private static void OnSkill3Pressed()
    {
        CombatManager.Instance.ExecutePlayerSkill(PlayerData.Instance.PlayerSkills["Skill3"]);
    }

    private void OnSkill4Pressed()
    {
        CombatManager.Instance.ExecutePlayerSkill(PlayerData.Instance.PlayerSkills["Skill4"]);
        UpdateBuffIcon();
    }

    private static void OnSkill5Pressed()
    {
        CombatManager.Instance.ExecutePlayerSkill(PlayerData.Instance.PlayerSkills["Skill5"]);
    }

    private void OnBuffPressed()
    {
        CombatManager.Instance.ApplyBlessing();
        UpdateBuffIcon();
        UpdatePlayerLabels();
    }
    private void OnVictoryPressed()
    {
        Cleanup();
        DungeonRoomManager.Instance.NextRoom();
        CombatManager.Instance.VictoryButtonLock = true;
        UpdateButtons();
    }

    private static void OnEndPressed()
    {
        CombatManager.Instance.EndPlayerTurn();
    }
    
    public void UpdateSkillTexts()
    {
        _skill1.Text = PlayerData.Instance?.PlayerSkills["Skill1"].Name;
        _skill2.Text = PlayerData.Instance?.PlayerSkills["Skill2"].Name;
        _skill3.Text = PlayerData.Instance?.PlayerSkills["Skill3"].Name;
        _skill4.Text = PlayerData.Instance?.PlayerSkills["Skill4"].Name;
        _skill5.Text = PlayerData.Instance?.PlayerSkills["Skill5"].Name;
    }
    public void Cleanup()
    {
        PlayerData.Instance.HealthBlockStaminaChanged -= UpdatePlayerLabels;
        CombatManager.Instance.BattleStateChanged -= OnBattleStateChanged;
        CombatManager.Instance.ActionUsed -= UpdateActions;
    }
}
