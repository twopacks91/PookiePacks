using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterOLD
{
    private Texture2D itemImage;
    private string itemName;
    private List<string> itemAttributes;
    private List<Item> equippedItems;

    public CharacterOLD(Texture2D image, string name, List<string> attributes)
    {
        this.itemImage = image;
        this.itemName = name;
        this.itemAttributes = attributes;
        this.equippedItems = new List<Item>(0);
    }

    public Texture2D GetImage()
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
