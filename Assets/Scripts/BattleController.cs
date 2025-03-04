using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    public GameObject CharacterObject;
    public GameObject OpponentObject;

    public TextMeshProUGUI PlayerActionText, OpponentActionText, PlayerPenText, OppPenText, OutcomeText;
    private float ActionDisplayTime = 4f;
    private float ActionTextTimer = 0f;

    private List<string> CharacterNames = new List<string>
    {
        "Demon Girl", "Crabby", "Jawski", "Alexa", "Flamer", "Brawlerina", "Bob", "Huzz",
        "Lesss Gooo", "Matt", "Weekday Duo", "Frederick", "Maxwell", "Antony"
    };

    public enum Action { None, Strike, Feint, Parry }
    private Action PlayerAction = Action.None;
    private Action OpponentAction = Action.None;

    public Button StrikeButton;
    public Button FeintButton;
    public Button ParryButton;

    [SerializeField]
    private HealthBar HealthBar;

    public static BattleController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        LoadScene();
        PlayerActionText.gameObject.SetActive(false);
        OpponentActionText.gameObject.SetActive(false);
        PlayerPenText.gameObject.SetActive(false);
        OppPenText.gameObject.SetActive(false);
        OutcomeText.gameObject.SetActive(false);

        StrikeButton.onClick.AddListener(() => SetPlayerAction(Action.Strike));
        FeintButton.onClick.AddListener(() => SetPlayerAction(Action.Feint));
        ParryButton.onClick.AddListener(() => SetPlayerAction(Action.Parry));
    }

    void LoadScene()
    {
        //SwitchCharacter("Antony");
        SwitchCharacter(PlayerData.GetInstance().GetEquippedCharacter().GetName(), ref CurrentCharacter, ref CharacterObject);

        //opponent
        //GameObject opponentObject = Instantiate(DemonGirlPrefab);
        //OpponentCharacter = opponentObject.GetComponent<Playable_DemonGirl>();
        SwitchCharacter(GetRandomCharacterName(), ref OpponentCharacter, ref OpponentObject );

        //Putting a delayed call on this shi makes it work, dk why
        Invoke(nameof(SetOpponentPosition), 0.1f);
        Invoke(nameof(SetHealthBar), 0.1f);
    }
    private void Update()
    {
        if (PlayerActionText.gameObject.activeSelf)
        {
            ActionTextTimer -= Time.deltaTime;
            if (ActionTextTimer <= 0f)
            {
                PlayerActionText.gameObject.SetActive(false);
                OpponentActionText.gameObject.SetActive(false);
            }
        }
    }


    void SetPlayerAction(Action action)
    {
        if (PlayerAction == Action.None) // Ensure player can only pick once per turn
        {
            PlayerAction = action;
            StartCoroutine(ResolveTurn());
        }
    }

    Action GetOpponentAction()
    {
        Action[] actions = { Action.Strike, Action.Feint, Action.Parry };
        return actions[Random.Range(0, actions.Length)];
    }


    IEnumerator ResolveTurn()
    {
        OpponentAction = GetOpponentAction();
        yield return new WaitForSeconds(1f);

        Debug.Log($"Player chose: {PlayerAction}, CPU chose: {OpponentAction}");
        CombatLogic();

        PlayerAction = Action.None;
        OpponentAction = Action.None;
    }


    bool PlayerParryPenalty = false;
    int PlayerParryRoundCounter = 0;
    bool OpponentParryPenalty = false;
    int OpponentParryRoundCounter = 0;
    void CombatLogic()
    {
        int PlayerDamage = CurrentCharacter.Attack();
        int OpponentDamage = OpponentCharacter.Attack();



        if (PlayerAction == Action.Strike && OpponentAction == Action.Strike)
        {
            CurrentCharacter.TakeDamage(OpponentDamage, PlayerParryPenalty);
            OpponentCharacter.TakeDamage(PlayerDamage, OpponentParryPenalty);
        }
        else if (PlayerAction == Action.Strike && OpponentAction == Action.Feint)
        {
            OpponentCharacter.TakeDamage(PlayerDamage, OpponentParryPenalty);
        }
        else if (PlayerAction == Action.Strike && OpponentAction == Action.Parry)
        {
            CurrentCharacter.TakeDamage(OpponentDamage, PlayerParryPenalty);
        }


        else if (PlayerAction == Action.Feint && OpponentAction == Action.Strike)
        {
            CurrentCharacter.TakeDamage(OpponentDamage, PlayerParryPenalty);
        }
        else if (PlayerAction == Action.Feint && OpponentAction == Action.Feint)
        {
            //nothing happens
        }
        else if (PlayerAction == Action.Feint && OpponentAction == Action.Parry)
        {
            OpponentParryPenalty = true;
            OpponentParryRoundCounter = 0;
        }


        else if (PlayerAction == Action.Parry && OpponentAction == Action.Strike)
        {
            foreach (Mission mission in PlayerData.GetInstance().GetToDoMissions())
            {
                if (mission.id == 3 && mission.progress < mission.needed)
                {
                    mission.progress++;
                    PlayerData.GetInstance().SavePlayer();
                }
            }
            OpponentCharacter.TakeDamage(PlayerDamage, OpponentParryPenalty);
        }
        else if (PlayerAction == Action.Parry && OpponentAction == Action.Feint)
        {
            PlayerParryPenalty = true;
            PlayerParryRoundCounter = 0;
        }
        else if (PlayerAction == Action.Parry && OpponentAction == Action.Parry)
        {
            PlayerParryPenalty = true;
            PlayerParryRoundCounter = 0;
            OpponentParryPenalty = true;
            OpponentParryRoundCounter = 0;
        }

        if (PlayerParryPenalty)
        {
            PlayerParryRoundCounter += 1;
            PlayerPenText.gameObject.SetActive(true);
        }
        if (OpponentParryPenalty)
        {
            OpponentParryRoundCounter += 1;
            OppPenText.gameObject.SetActive(true);
        }

        if (PlayerParryRoundCounter > 1)
        {
            PlayerParryPenalty = false;
            PlayerParryRoundCounter = 0;
            PlayerPenText.gameObject.SetActive(false);
        }
        if (OpponentParryRoundCounter > 1)
        {
            OpponentParryPenalty = false;
            PlayerParryRoundCounter = 0;
            OppPenText.gameObject.SetActive(false);
        }

        PlayerActionText.text = "You did " + GetActionName(PlayerAction); 
        PlayerActionText.gameObject.SetActive(true);

        OpponentActionText.text = "They did " + GetActionName(OpponentAction);
        OpponentActionText.gameObject.SetActive(true);


        ActionTextTimer = ActionDisplayTime;

        CheckGameOutcome();

        
    }
    void CheckGameOutcome()
    {
        if (CurrentCharacter.GetHealth() <= 0)
        {
            Debug.Log("CPU Wins!");
            OutcomeText.text = "TRY AGAIN!";
            OutcomeText.gameObject.SetActive(true);

            StartCoroutine(LoadHomeScene(5f));
        }
        else if (OpponentCharacter.GetHealth() <= 0)
        {
            Debug.Log("Player Wins!");
            foreach(Mission mission in PlayerData.GetInstance().GetToDoMissions())
            {
                if(mission.id==1 && mission.progress<mission.needed)
                {
                    mission.progress++;
                    PlayerData.GetInstance().SavePlayer();
                }
            }

            OutcomeText.text = "YOU WIN!";
            OutcomeText.gameObject.SetActive(true);

            StartCoroutine(LoadHomeScene(5f));
        }
    }


    IEnumerator LoadHomeScene(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("Home");
    }

    string GetActionName(Action action)
    {
        if (action == Action.Parry)
        {
            return "Parry";
        }
        else if (action == Action.Strike)
        {
            return "Strike";
        }
        else if (action == Action.Feint)
        {
            return "Feint";
        }
        return "None";
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

    public string GetRandomCharacterName()
    {
        System.Random random = new System.Random();
        int index = random.Next(CharacterNames.Count);
        return CharacterNames[index];
    }
    private void SwitchCharacter(string characterName, ref PlayableCharacter character, ref GameObject prefab)
    {
        if (character != null)
        {
            Destroy(character.gameObject); // Destroy the current character (if any)
        }

        // Instantiate the new character based on the name
        if (characterName == "Demon Girl")
        {
            prefab = Instantiate(DemonGirlPrefab);
            character = prefab.GetComponent<Playable_DemonGirl>();
        }
        else if (characterName == "Crabby")
        {
            prefab = Instantiate(CrabbyPrefab);
            character = prefab.GetComponent<Playable_Crabby>();
        }
        else if (characterName == "Jawski")
        {
            prefab = Instantiate(JawskiPrefab);
            character = prefab.GetComponent<Playable_Croc>();
        }
        else if (characterName == "Alexa")
        {
            prefab = Instantiate(AlexaPrefab);
            character = prefab.GetComponent<Playable_Alexa>();
        }
        else if (characterName == "Flamer")
        {
            prefab = Instantiate(FlamerPrefab);
            character = prefab.GetComponent<Playable_Flamer>();
        }
        else if (characterName == "Brawlerina")
        {
            prefab = Instantiate(BrawlerinaPrefab);
            character = prefab.GetComponent<Playable_Brawlerina>();
        }
        else if (characterName == "Pengting")
        {
            prefab = Instantiate(PengtingPrefab);
            character = prefab.GetComponent<Playable_Pengting>();
        }
        else if (characterName == "Grindroid")
        {
            prefab = Instantiate(GrindroidPrefab);
            character = prefab.GetComponent<Playable_Pengting>();
        }
        else if (characterName == "Bob")
        {
            prefab = Instantiate(BobPrefab);
            character = prefab.GetComponent<Playable_Bob>();
        }
        else if (characterName == "Huzz")
        {
            prefab = Instantiate(HuzzPrefab);
            character = prefab.GetComponent<Playable_Huzz>();
        }
        else if (characterName == "Lesss Gooo")
        {
            prefab = Instantiate(LesssGoooPrefab);
            character = prefab.GetComponent<Playable_Lesssgooo>();
        }
        else if (characterName == "Matt")
        {
            prefab = Instantiate(MattPrefab);
            character = prefab.GetComponent<Playable_Matt>();
        }
        else if (characterName == "Weekday Duo")
        {
            prefab = Instantiate(WeekdayPrefab);
            character = prefab.GetComponent<Playable_Weekday>();
        }
        else if (characterName == "Frederick")
        {
            prefab = Instantiate(FrederickPrefab);
            character = prefab.GetComponent<Playable_Frederick>();
        }
        else if (characterName == "Maxwell")
        {
            prefab = Instantiate(MaxwellPrefab);
            character = prefab.GetComponent<Playable_Maxwell>();
        }
        else if (characterName == "Antony")
        {
            prefab = Instantiate(AntonyPrefab);
            character = prefab.GetComponent<Playable_Antony>();
        }
        // Add more characters as needed by switching their name and adding their prefab
    }


    // Add additional methods to trigger other actions for other characters, if you have them
}