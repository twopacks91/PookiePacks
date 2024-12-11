using UnityEngine;

public class BattleController : MonoBehaviour
{
    public PlayableCharacter CurrentCharacter; // This will hold the reference to the current character
    public GameObject demonGirlPrefab;  // Prefab for DemonGirl (Drag & Drop in the Inspector)

    [SerializeField]
    private HealthBar healthBar;

    void Start()
    {
        // Start with DemonGirl
        SwitchCharacter("DemonGirl");
    }

    public void SwitchCharacter(string characterName)
    {
        if (CurrentCharacter != null)
        {
            Destroy(CurrentCharacter.gameObject); // Destroy the current character (if any)
        }

        // Instantiate the new character based on the name
        if (characterName == "DemonGirl")
        {
            GameObject characterObject = Instantiate(demonGirlPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_DemonGirl>();
        }

        // Add more characters as needed by switching their name and adding their prefab
    }

    // Call this from a button to trigger DemonGirl's attack
    public void OnAttackButtonPressed()
    {
        int damage = CurrentCharacter.Attack();
        healthBar.OpponentDamage(damage);
    }

    // Add additional methods to trigger other actions for other characters, if you have them
}