using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar 1")]
    public Image HealthBarBackground;   // Background image of health bar 1
    public Image HealthBarFill;   // Foreground image of health bar 1
    public TextMeshProUGUI PlayerHealthText;
    public float PlayerHealth;  // Current health of health bar 1
    public float PlayerMaxHealth;      // Max health of health bar 1

    [Header("Health Bar 2")]
    public Image HealthBarBackground2;   // Background image of health bar 2
    public Image HealthBarFill2;   // Foreground image of health bar 2
    public TextMeshProUGUI OpponentHealthText;
    public float OpponentHealth;  // Current health of health bar 2
    public float OpponentMaxHealth;      // Max health of health bar 2


    private void Start()
    {
        SetHealthValues(200, 350);
    }

    private void Update()
    {
        // Update both health bars
        UpdateHealthBar(HealthBarFill, PlayerHealth, PlayerMaxHealth);
        UpdateHealthText(PlayerHealthText, PlayerHealth, PlayerMaxHealth);

        UpdateHealthBar(HealthBarFill2, OpponentHealth, OpponentMaxHealth);
        UpdateHealthText(OpponentHealthText, OpponentHealth, OpponentMaxHealth);

    }
    public void SetHealthValues(float pHealth, float oHealth)
    {
        PlayerHealth = pHealth;
        PlayerMaxHealth = pHealth;
        OpponentHealth = oHealth;
        OpponentMaxHealth = oHealth;
    }

    private void UpdateHealthText(TextMeshProUGUI healthText, float currentHealth, float maxHealth)
    {
        // Set the text to display the current health and max health
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    // This method updates the foreground image width based on the health ratio
    private void UpdateHealthBar(Image foregroundImage, float currentHealth, float maxHealth)
    {
        // Calculate the health percentage
        float healthPercentage = currentHealth / maxHealth;

        // Adjust the width of the foreground image based on the health percentage
        foregroundImage.fillAmount = healthPercentage;
    }

    // Example methods to modify health values (can be used for testing)
    public void PlayerDamage(float damage)
    {
        PlayerHealth = Mathf.Clamp(PlayerHealth - damage, 0f, PlayerMaxHealth);
    }

    public void PlayerHeal(float healAmount)
    {
        PlayerHealth = Mathf.Clamp(PlayerHealth + healAmount, 0f, PlayerMaxHealth);
    }

    public void OpponentDamage(float damage)
    {
        OpponentHealth = Mathf.Clamp(OpponentHealth - damage, 0f, OpponentMaxHealth);
    }

    public void OpponentHeal(float healAmount)
    {
        OpponentHealth = Mathf.Clamp(OpponentHealth + healAmount, 0f, OpponentMaxHealth);
    }
}