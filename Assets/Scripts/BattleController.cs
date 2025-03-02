using UnityEngine;

public class BattleController : MonoBehaviour
{
    public PlayableCharacter CurrentCharacter; // This will hold the reference to the current character
    public PlayableCharacter OpponentCharacter;

    public GameObject DemonGirlPrefab;
    public GameObject CrabbyPrefab;
    public GameObject JawskiPrefab;
    public GameObject AlexaPrefab;
    public GameObject FlamerPrefab;

    [SerializeField]
    private HealthBar HealthBar;

    void Start()
    {
        LoadScene();
    }

    void LoadScene()
    {
        //SwitchCharacter("Alexa");
        SwitchCharacter(PlayerData.GetInstance().GetEquippedCharacter().GetName());

        //opponent
        GameObject opponentObject = Instantiate(CrabbyPrefab);
        OpponentCharacter = opponentObject.GetComponent<Playable_Crabby>();
        
        //Putting a delayed call on this shi makes it work, dk why
        Invoke(nameof(SetOpponentPosition), 0.1f);
        Invoke(nameof(SetHealthBar), 0.1f);
    }

    void SetHealthBar()
    {
        Debug.Log("Character Health: " + CurrentCharacter.GetHealth() + "Opponent Health: " + OpponentCharacter.GetHealth());
        HealthBar.SetHealthValues(CurrentCharacter.GetHealth(), OpponentCharacter.GetHealth());
    }
    void SetOpponentPosition()
    {
        if (OpponentCharacter != null)
        {
            OpponentCharacter.SetOpponentPosition();
        }
    }
    private void SwitchCharacter(string characterName)
    {
        if (CurrentCharacter != null)
        {
            Destroy(CurrentCharacter.gameObject); // Destroy the current character (if any)
        }

        // Instantiate the new character based on the name
        if (characterName == "Demon Girl")
        {
            GameObject characterObject = Instantiate(DemonGirlPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_DemonGirl>();
        }
        else if (characterName == "Crabby")
        {
            GameObject characterObject = Instantiate(CrabbyPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Crabby>();
        }
        else if (characterName == "Jawski")
        {
            GameObject characterObject = Instantiate(JawskiPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Croc>();
        }
        else if (characterName == "Alexa")
        {
            GameObject characterObject = Instantiate(AlexaPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Alexa>();
        }
        else if (characterName == "Flamer")
        {
            GameObject characterObject = Instantiate(FlamerPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Flamer>();
        }

        // Add more characters as needed by switching their name and adding their prefab
    }

    // Call this from a button to trigger DemonGirl's attack
    public void OnAttackButtonPressed()
    {
        int damage = CurrentCharacter.Attack();
        HealthBar.OpponentDamage(damage);
    }

    // Add additional methods to trigger other actions for other characters, if you have them
}