using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    private string itemImage;  // image name
    private string itemName;
    private string rarity;
    private int HP;
    private int attack;
    private int defence;
    private List<string> itemAttributes;
    private List<Item> equippedItems;

    public Character(string image, string name, List<string> attributes)
    {
        this.itemImage = image;
        this.itemName = name;
        this.itemAttributes = attributes;
        this.rarity = image.Split('_')[0];
        this.equippedItems = new List<Item>(0);
    }

    public string GetRarity()
    {
        return rarity;
    }

    public string GetImage()
    {
        return itemImage;
    }

    public string GetName()
    {
        return itemName;
    }

    public List<string> GetAttributes()
    {
        return itemAttributes;
    }

    public List<Item> GetEquippedItems()
    {
        return equippedItems;
    }
}
