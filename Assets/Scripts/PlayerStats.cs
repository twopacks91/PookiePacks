using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PlayerStats : MonoBehaviour
{
    TextMeshProUGUI stats;
    private Character character;

    void Start()
    {
        stats = this.GetComponent<TextMeshProUGUI>();

        // Retrieve the equipped character from PlayerData
        character = PlayerData.GetInstance().GetEquippedCharacter();

        // Display the character's stats
        DisplayCharacterStats();
    }

    void DisplayCharacterStats()
    {
        stats.text = $"Attack: {character.GetAttack()} Defence: {character.GetDefence()} HP: {character.GetHP()}";
    }
}