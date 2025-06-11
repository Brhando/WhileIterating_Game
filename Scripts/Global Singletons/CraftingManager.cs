using Godot;
using System.Collections.Generic;

public partial class CraftingManager : Node
{
    public static CraftingManager Instance;
    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();
        
        InitializeRecipes();
    }
    public class CraftingRecipe
    {
        public string OutputItem;
        public int OutputAmount;
        public Dictionary<string, int> RequiredItems { get; set; }
        public string ToolTip;
        public string FlavorText;
    
        //constructor
        public CraftingRecipe(string outputItem, int outputAmount, Dictionary<string, int> requiredItems, string toolTip, string flavorText)
        {
            OutputItem = outputItem;
            OutputAmount = outputAmount;
            RequiredItems = requiredItems;
            ToolTip = toolTip;
            FlavorText = flavorText;
            
        }
    }
    
    public Dictionary<string, CraftingRecipe> CraftingRecipes = new();
    
    private void InitializeRecipes()
    {
        CraftingRecipes.Add("Plank", new CraftingRecipe("Plank", 1, new Dictionary<string, int> { { "Wood", 3 } }, "A material with several uses", "You feel accomplished 'refining' this, but you are really only splitting wood into more wood"));
        CraftingRecipes.Add("Stone Plate", new CraftingRecipe("Stone Plate", 1, new Dictionary<string, int> { { "Stone", 5 } }, "Basic defensive material. Combining 3 gives you a base shield.", "Welcome to the stone age." ));
    }

    private bool CheckItems(string item)
    {
        var recipe = CraftingRecipes[item];
        foreach (var req in recipe.RequiredItems)
        {
            if (!GameManager.Instance.Inventory.ContainsKey(req.Key) || GameManager.Instance.Inventory[req.Key] < req.Value)
            {
                return false;
            }
        }
        return true;
    }

    public bool CraftItemWithResult(string item)
    {
        if (CheckItems(item))
        {
            foreach (var req in CraftingRecipes[item].RequiredItems)
            {
                GameManager.Instance?.RemoveItem(req.Key, req.Value);
            }
            GameManager.Instance?.AddItem(CraftingRecipes[item].OutputItem, CraftingRecipes[item].OutputAmount);
            return true;
        }
        else
        {
            return false;
        }
    }
}