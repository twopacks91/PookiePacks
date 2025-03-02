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
    private bool isEquippedCharacter;

    public Character(string image, string name, List<string> attributes, int hp, int atk, int def )
    {
        this.itemImage = image;
        this.itemName = name;
        this.itemAttributes = attributes;
        this.rarity = image.Split('_')[0];
        this.equippedItems = new List<Item>(0);
        this.isEquippedCharacter = false;
        this.HP = hp;
        this.defence = def;
        this.attack = atk;
    }

    public bool IsEquipped()
    {
        return isEquippedCharacter;
    }

    public void Equip()
    {
        isEquippedCharacter = true;
        PlayerData.GetInstance().SetEquippedCharacter(this);
    }

    public void Dequip()
    {
        isEquippedCharacter = false;
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

    public int GetHP()
    {
        return HP;
    }

    public int GetAttack()
    {
        return attack;
    }

    public int GetDefence()
    {
        return defence;
    }

    public List<Item> GetEquippedItems()
    {
        return equippedItems;
    }
}
