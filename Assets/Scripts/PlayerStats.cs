using UnityEngine;
using TMPro;

public class PlayerStats
{
    private int HP;
    private int attack;
    private int defence;

    private Character character;

    void Start()
    {
        // Retrieve the equipped character from PlayerData
        character = PlayerData.GetInstance().GetEquippedCharacter();

        // Display the character's stats
        DisplayCharacterStats();
    }

    void DisplayCharacterStats()
    {
        attackText.text = $"Attack: {character.GetAttack()}";
        defenceText.text = $"Defence: {character.GetDefence()}";
        hpText.text = $"HP: {character.GetHP()}";
    }
}