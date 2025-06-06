using Godot;
using System;
using System.Collections.Generic;

public partial class DreamManager : Node
{
    public static DreamManager Instance;
    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            QueueFree();
        }
        InitializeDreamTextList1();
        InitializeDreamTextList2();
        InitializeRandomDreamTextList();
    }
    //used to manage dream lore and track milestones
    
    //list of text to include in sleep scene
    public List<string> DreamTextList1 = new();
    public List<string> DreamTextList2 = new();
    public List<string> RandomDreamTextList = new();

    private void InitializeDreamTextList1()
    {
        DreamTextList1.Add("You hear a voice that is familiar and completely alien.");
        DreamTextList1.Add("It reflects on your day.");
    }

    private void InitializeDreamTextList2()
    {
        DreamTextList2.Add("Enemies Defeated: "); //add enemies defeated tracker -> in CombatManager
        DreamTextList2.Add("Enemies that remember: "); //add enemies that leveled up due to dungeon progression -> in EnemyManager
        //Add other trackers: NPCs talked to? Special Boss messages? 
        //If Dungeon was defeated: "The corruption grows...The world remembers..."
    }

    public void RefreshDreamTextList2()
    {
        DreamTextList2.Clear();
        //add in updated trackers here
    }

    private void InitializeRandomDreamTextList()
    {
        RandomDreamTextList.Add("You remember dying. You’re not sure which time it was.");
        RandomDreamTextList.Add("A voice like yours says, 'You’ve done this before… many times.'");
        RandomDreamTextList.Add("The dungeon grows not with stone, but with memory.");
        RandomDreamTextList.Add("You see your hand holding a blade—then a flame—then nothing at all.");
        RandomDreamTextList.Add("A child’s laugh echoes through stone halls. You wake cold.");
        RandomDreamTextList.Add("A symbol burns behind your eyes. It feels... familiar.");
        RandomDreamTextList.Add("You dream of a village with no people. Only shadows looking out.");
        RandomDreamTextList.Add("Something watches from beneath the floor. It is patient.");
        RandomDreamTextList.Add("The monsters speak in dreams. You almost understand them.");
        RandomDreamTextList.Add("You wake with dirt under your nails and no memory of digging.");
        RandomDreamTextList.Add("You were someone else. You still are. Both remember.");
        RandomDreamTextList.Add("The Turning comes for all things. Even gods forget.");
        RandomDreamTextList.Add("You remember being feared. You don’t remember why.");
        RandomDreamTextList.Add("The world does not reset. Only you do.");
        RandomDreamTextList.Add("A blade rusts where you last fell. You wonder who will find it this time.");
        RandomDreamTextList.Add("Each dungeon heart beats louder in your dreams.");
        RandomDreamTextList.Add("The village elder once knew your name. They no longer speak it.");
        RandomDreamTextList.Add("You hear the gods arguing. One of them weeps.");
        RandomDreamTextList.Add("Your footsteps echo where no one walks.");
        RandomDreamTextList.Add("The dungeon remembers its own death.");
    }

    public string GetDreamTextInitial(int index)
    {
        return DreamTextList1[index];
    }

    public string GetDreamText()
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();
        var index = rng.RandiRange(0, RandomDreamTextList.Count - 1);
        return RandomDreamTextList[index];
    }




}