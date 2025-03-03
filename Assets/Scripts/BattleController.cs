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
    public GameObject BrawlerinaPrefab;
    public GameObject PengtingPrefab;
    public GameObject GrindroidPrefab;
    public GameObject BobPrefab;
    public GameObject HuzzPrefab;
    public GameObject LesssGoooPrefab;
    public GameObject MattPrefab;
    public GameObject WeekdayPrefab;
    public GameObject FrederickPrefab;
    public GameObject MaxwellPrefab;
    public GameObject AntonyPrefab;

    [SerializeField]
    private HealthBar HealthBar;

    void Start()
    {
        LoadScene();
    }

    void LoadScene()
    {
        //SwitchCharacter("Antony");
        SwitchCharacter(PlayerData.GetInstance().GetEquippedCharacter().GetName());

        //opponent
        GameObject opponentObject = Instantiate(DemonGirlPrefab);
        OpponentCharacter = opponentObject.GetComponent<Playable_DemonGirl>();

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
        else if (characterName == "Brawlerina")
        {
            GameObject characterObject = Instantiate(BrawlerinaPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Brawlerina>();
        }
        else if (characterName == "Pengting")
        {
            GameObject characterObject = Instantiate(PengtingPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Pengting>();
        }
        else if (characterName == "Grindroid")
        {
            GameObject characterObject = Instantiate(GrindroidPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Pengting>();
        }
        else if (characterName == "Bob")
        {
            GameObject characterObject = Instantiate(BobPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Bob>();
        }
        else if (characterName == "Huzz")
        {
            GameObject characterObject = Instantiate(HuzzPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Huzz>();
        }
        else if (characterName == "Lesss Gooo")
        {
            GameObject characterObject = Instantiate(LesssGoooPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Lesssgooo>();
        }
        else if (characterName == "Matt")
        {
            GameObject characterObject = Instantiate(MattPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Matt>();
        }
        else if (characterName == "Weekday Duo")
        {
            GameObject characterObject = Instantiate(WeekdayPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Weekday>();
        }
        else if (characterName == "Frederick")
        {
            GameObject characterObject = Instantiate(FrederickPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Frederick>();
        }
        else if (characterName == "Maxwell")
        {
            GameObject characterObject = Instantiate(MaxwellPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Maxwell>();
        }
        else if (characterName == "Antony")
        {
            GameObject characterObject = Instantiate(AntonyPrefab);
            CurrentCharacter = characterObject.GetComponent<Playable_Antony>();
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