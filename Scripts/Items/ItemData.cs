public class ItemData
{
    public string ID { get; set; }  // Unique identifier (ex: "wood_log")
    public string DisplayName { get; set; }
    public ItemType Type { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public int MaxStackSize { get; set; }

    // Optional: economy values
    public int SellPrice { get; set; }
    public int BuyPrice { get; set; }

    // Constructor
    public ItemData(string id, string displayName, ItemType type, string description, string iconPath, int maxStackSize, int buyPrice, int sellPrice)
    {
        ID = id;
        DisplayName = displayName;
        Type = type;
        Description = description;
        IconPath = iconPath;
        MaxStackSize = maxStackSize;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
    }
}