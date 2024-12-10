using UnityEngine;

[System.Serializable]
public class Item
{
    private string itemImage;
    private string itemName;
    private string[] itemAttributes;
    private bool isEquipped;

    public Item(string image, string name, string[] attributes)
    {
        this.itemImage = image;
        this.itemName = name;
        this.itemAttributes = attributes;
        this.isEquipped = false;
    }

    public string GetImage()
    {
        return itemImage;
    }

    public string GetName()
    {
        return itemName;
    }

    public string[] GetAttributes()
    {
        return itemAttributes;
    }

    public bool IsEquipped()
    {
        return isEquipped;
    }
}
