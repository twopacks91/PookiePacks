using UnityEngine;

[System.Serializable]
public class Item
{
    private Texture2D itemImage;
    private string itemName;
    private string[] itemAttributes;
    private bool isEquipped;

    public Item(Texture2D image, string name, string[] attributes)
    {
        this.itemImage = image;
        this.itemName = name;
        this.itemAttributes = attributes;
        this.isEquipped = false;
    }

    public Texture2D GetImage()
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
