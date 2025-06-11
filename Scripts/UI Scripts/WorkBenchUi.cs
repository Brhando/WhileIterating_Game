using Godot;
using System;
using System.Text;

public partial class WorkBenchUi : CanvasLayer
{
	private ItemList _recipeList;
	private Label _recipeDetailsLabel;
	private Button _craftButton;
	private Label _messageLabel;
	

	private string _selectedRecipe;
	public bool PlayerInRange;

	public override void _Ready()
	{
		_recipeList = GetNode<ItemList>("PanelContainer/VBoxContainer/RecipeList");
		_recipeDetailsLabel = GetNode<Label>("PanelContainer/VBoxContainer/RecipeDetailsLabel");
		_craftButton = GetNode<Button>("PanelContainer/VBoxContainer/CraftButton");
		_messageLabel = GetNode<Label>("PanelContainer/VBoxContainer/MessageLabel");
		

		_recipeList.ItemSelected += OnRecipeSelected;
		_craftButton.Pressed += OnCraftPressed;
	}
	
	public void PopulateRecipes()
	{
		_recipeList.Clear();
		var index = 0;
		foreach (var recipe in CraftingManager.Instance.CraftingRecipes)
		{
			_recipeList.AddItem(recipe.Key);
			_recipeList.SetItemTooltip(index, recipe.Value.ToolTip);
			index++;
		}
		GD.Print("ItemList has ", _recipeList.GetItemCount(), " items after population.");
	}


	private void OnRecipeSelected(long index)
	{
		_selectedRecipe = _recipeList.GetItemText((int)index);
		DisplayRecipeDetails(_selectedRecipe);
	}

	private void DisplayRecipeDetails(string recipeName)
	{
		var recipe = CraftingManager.Instance.CraftingRecipes[recipeName];
		StringBuilder sb = new StringBuilder();

		sb.AppendLine($"Item: {recipe.OutputItem} (x{recipe.OutputAmount})");
		sb.AppendLine($"Description: {recipe.ToolTip}\n");
		sb.AppendLine("Required Materials:");

		foreach (var req in recipe.RequiredItems)
		{
			sb.AppendLine($"- {req.Key} x{req.Value}");
		}

		_recipeDetailsLabel.Text = sb.ToString();
		_messageLabel.Text = "";
	}

	private void OnCraftPressed()
	{
		if (string.IsNullOrEmpty(_selectedRecipe))
		{
			_messageLabel.Text = "Please select a recipe.";
			return;
		}

		if (CraftingManager.Instance != null)
		{
			if (CraftingManager.Instance.CraftItemWithResult(_selectedRecipe))
			{
				_messageLabel.Text = $"Successfully crafted {_selectedRecipe}!";
			}
			else
			{
				_messageLabel.Text = "You do not have the required materials.";
			}
		}
	}
}
