using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Collections;
using UnityEditor.U2D.Animation;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


// **** REMEMBER - Improve animations for greater rarity of characters eventually

[System.Serializable]
public class Summons : MonoBehaviour
{
    // *** Variables
    private const float kAnimationTime = 5.0f;  // animation time in seconds
    private const float kMagicDelay = 1.0f;
    private const float kLightningDelay = 2.0f;
    private const float kSphereBottomDelay = 3.0f;
    private const float kScreenSplashDelay = 3.5f;

    // - - First line for both databases should be ignored, last line for summon database should be ignored
    // Summon Database Format (5): ID - Character Name - Character Image Name - Rarity - Rate
    // Character Database Format (5): ID - Character Name - Rarity - HP - Attack - Defence
    private const int kCharacterName = 1;       // for summon and character database
    private const int kCharacterImageName = 2;  // for summon database only
    private const int kCharacterRarity = 3;     // for summon database only
    private const int kCharacterRate = 4;       // for summon database only
    private const int kStatHP = 3;              // for character database only
    private const int kStatAttack = 4;          // for character database only
    private const int kStatDefence = 5;         // for character database only

    private const string kRarityBronze = "bronze";
    private const string kRaritySilver = "silver";
    private const string kRarityGold = "gold";
    private const string kRarityDiamond = "diamond";
    private const string kRarityLegendary = "legendary";
    private const string kStatValuesWhenError = "NA";

    // Canvases
    [SerializeField]
    private Canvas selectionCanvas;
    [SerializeField]
    private Canvas animationCanvas;
    [SerializeField]
    private Canvas conclusionCanvas;
    [SerializeField]
    private RawImage backgroundImage;

    // Animation & Effects
    [SerializeField]
    private GameObject magicEffect;
    [SerializeField]
    private GameObject lightningEffect;
    [SerializeField]
    private GameObject sphereBottomEffect;
    [SerializeField]
    private GameObject screenSplashEffect;

    // Stat Text
    [SerializeField]
    private TextMeshProUGUI hpText;
    [SerializeField]
    private TextMeshProUGUI attackText;
    [SerializeField]
    private TextMeshProUGUI defenceText;

    // Rates Content
    [SerializeField]
    private GameObject ratesPanel;
    [SerializeField]
    private TextMeshProUGUI ratesErrorText;
    [SerializeField]
    private Transform listContainer;
    [SerializeField]
    private GameObject characterItemPrefab;

    // Character Image Panel
    [SerializeField]
    private GameObject imagePanel;
    [SerializeField]
    private RawImage imagePanelBackground;
    [SerializeField]
    private TextMeshProUGUI hpPanelText;
    [SerializeField]
    private TextMeshProUGUI attackPanelText;
    [SerializeField]
    private TextMeshProUGUI defencePanelText;


    // *** Structs
    private struct CharacterData
    {
        public string name;         // Character Name
        public string imagePath;    // Image Asset Name
        public string rate;         // Drop Rate
        public string rarity;       // Rarity
        public List<string> stats;  // Character Stats (remember - from character database)

        // Constructor
        public CharacterData(string name, string imagePath, string rate, string rarity, List<string> stats)
        {
            this.name = name;
            this.imagePath = imagePath;
            this.rate = rate;
            this.rarity = rarity;
            this.stats = stats;
        }
    }



    // *** Functions
    /// <summary>
    /// Go through the motions of a summon; user selects, animation plays, conclusion shows.
    /// </summary>
    public void StartSummon()
    {
        // Disable selection & display animation
        selectionCanvas.gameObject.SetActive(false);
        animationCanvas.gameObject.SetActive(true);

        // Ensure user has enough currency

        // Database Format (5): ID - Character Name - Character Image Name - Rarity - Rate
        string [] characterData = null;
        if(!GenerateCharacter(ref characterData))
        {
            Debug.LogError("Summon Error - failure to generate character!");
            return;
        }

        // Play animation based on rarity
        PlayRarityAnimation(characterData[kCharacterRarity]);

        // Prepare to show character stats
        string [] statData = null;
        ShowCharacterStats(characterData[kCharacterName], ref statData);

        // Save character to user's inventory
        SaveSummonToInventory(characterData[kCharacterName], characterData[kCharacterImageName], statData);
        Debug.Log("Successfully Saved to Inventory!");

        //// Prepare to show character stats
        //ShowCharacterStats(characterData[kCharacterName]);

        // Show conclusion - this function accounts for animation time
        ShowConclusion(characterData[kCharacterImageName]);
    }


    /// <summary>
    /// Generates a random character using database of characters
    /// </summary>
    /// <param name="characterData"> list of character data which will update when successfully generated </param>
    /// <returns></returns>
    private bool GenerateCharacter(ref string[] characterData)
    {
        // Read summon database file, relative to current path
        //string oldDatabaseFile = Path.Combine(Application.dataPath, "Databases", "SummonTableDraft3.csv");
        string databaseFile = Path.Combine(Application.persistentDataPath, "SummonTableDraft3.csv");
        if (!File.Exists(databaseFile))
        {
            Debug.LogError($"Summon Error: Summon Database file not found at {databaseFile}");
            return false;
        }
        string[] database = File.ReadAllLines(databaseFile);

        // In database - last line & last column contains total rates of all characters in database currently
        string lastLine = database[database.Length - 1];
        string[] totalRateData = lastLine.Split(',');
        if (!float.TryParse(totalRateData[totalRateData.Length - 1], out float totalRates))
        {
            Debug.LogError("Summon Error - Unable to get total rates from database");
            return false;
        }

        Debug.Log($"Read successfully as {totalRates}");  // REMOVE

        // Generate random number in current range of rates
        float randomValue = UnityEngine.Random.Range(0, totalRates);

        Debug.Log($"RANDOM VALUE WAS - {randomValue}");  // REMOVE

        // Find character pulled using cumulative rates
        float cumulativeRate = 0;
        for (int i = 1; i < database.Length - 1; i++)  // ignore first line (headers) and last
        {
            // Get rate for current character and add to cumulative rate
            string[] data = database[i].Split(',');
            if (!float.TryParse(data[data.Length - 1], out float rate))
            {
                Debug.LogError($"Summon Error - Error parsing character rate {data[data.Length - 1]}");
                return false;
            }
            cumulativeRate += rate;

            // Get's the character which is next in rate
            if (randomValue <= cumulativeRate)
            {
                // Assign character data to parameter
                Debug.Log($"YEYYYYYYYYY we pulled - {data[1]}");  // REMOVE
                characterData = data;
                break;
            }
        }

        // Ensure parameter was updated correctly with character data
        if (characterData == null)
        {
            Debug.LogError("Summon Error - Couldn't acquire character data");
            return false;
        }
        return true;
    }


    /// <summary>
    /// Plays an animation on screen based on the rarity of character generated
    /// </summary>
    /// <param name="rarity"></param>
    private void PlayRarityAnimation(string rarity)
    {
        // Modify animation based on character rarity
        if ((rarity == kRaritySilver)  || (rarity == kRarityGold) || (rarity == kRarityDiamond) || (rarity == kRarityLegendary))
        {
            Invoke(nameof(PlayMagicEffect), kMagicDelay);
            if ((rarity == kRarityGold) || (rarity == kRarityDiamond) || (rarity == kRarityLegendary))
            {
                Invoke(nameof(PlayLightningEffect), kLightningDelay);

                if ((rarity == kRarityDiamond) || (rarity == kRarityLegendary))
                {
                    Invoke(nameof(PlaySphereEffect), kSphereBottomDelay);

                    if (rarity == kRarityLegendary)
                    {
                        Invoke(nameof(PlayScreenSplashEffect), kScreenSplashDelay);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called in generate character, plays sparkle effect which is initially disabled.
    /// </summary>
    private void PlayMagicEffect()
    {
        magicEffect.SetActive(true);
    }

    /// <summary>
    /// Called in generate character, plays lightning effect which is initially disabled.
    /// </summary>
    private void PlayLightningEffect()
    {
        lightningEffect.SetActive(true);
    }

    /// <summary>
    /// Called in generate character, plays bottom sphere effect which is initially disabled.
    /// </summary>
    private void PlaySphereEffect()
    {
        sphereBottomEffect.SetActive(true);
    }

    /// <summary>
    /// Called in generate character, plays screen splash effect which is initially disabled.
    /// </summary>
    private void PlayScreenSplashEffect()
    { 
        screenSplashEffect.SetActive(true); 
    }

    /// <summary>
    /// Saves character summoned to user's inventory
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="characterImageName"></param>
    private void SaveSummonToInventory(string characterName, string characterImageName, string[] statData)
    {
        // Make instance of character and save it to current player 
        List<string> statList = new List<string>();
        statList.Add($"HP: {statData[0]}");
        statList.Add($"ATTACK: {statData[1]}");
        statList.Add($"DEFENCE: {statData[2]}");
        Character newChar = new Character(characterImageName, characterName, statList, int.Parse(statData[0]), int.Parse(statData[1]), int.Parse(statData[2]));

        // Get current player data and insert new character into their inventory
        PlayerData currentPlayer = PlayerData.GetInstance();
        currentPlayer.InsertCharacter(newChar);
        currentPlayer.SavePlayer();
    }


    /// <summary>
    /// Changes conslusion screen stats based on the character generated, stats stored in local database
    /// </summary>
    /// <param name="characterName"> string as name of character to search for </param>
    /// <param name="statData"> list of strings containing character data </param>
    private void ShowCharacterStats(string characterName, ref string[] statData)
    {
        // Get this character's stats from database
        //string oldDatabaseFile = Path.Combine(Application.dataPath, "Databases", "CharacterTableDraft1.csv");
        string databaseFile = Path.Combine(Application.persistentDataPath, "CharacterTableDraft1.csv");
        Debug.Log(databaseFile);
        if (!File.Exists(databaseFile))
        {
            // Mark text as NA for not available and return early
            Debug.LogError($"Summon Error: Character database file not found at {databaseFile}");
            hpText.text = kStatValuesWhenError;
            attackText.text = kStatValuesWhenError;
            defenceText.text = kStatValuesWhenError;
            return;
        }
        string[] database = File.ReadAllLines(databaseFile);

        // Ignore line 1 in database, it's just headings
        // Find this character and thier details in database
        string[] characterData = { };
        for (int i = 1; i < database.Length; i++)
        {
            string[] data = database[i].Split(',');
            if (data[kCharacterName] == characterName)
            {
                characterData = data;
                break;
            }
        }

        // Ensure character was found in database, otherwise log error and set stat values to NA for "Not available"
        if (characterData.Length == 0)
        {
            Debug.LogError($"Summon Error: Character not found in database, name is {characterName}");
            hpText.text = kStatValuesWhenError;
            attackText.text = kStatValuesWhenError;
            defenceText.text = kStatValuesWhenError;
            return;
        }

        // Update conclusion screen stat text based on character data
        hpText.text = characterData[kStatHP];
        attackText.text = characterData[kStatAttack];
        defenceText.text = characterData[kStatDefence];
        string[] tempData = { characterData[kStatHP], characterData[kStatAttack], characterData[kStatDefence] };
        statData = tempData;
    }


    /// <summary>
    /// Updates to display final character image and end animations for a smooth transition
    /// </summary>
    /// <param name="characterImageName"></param>
    private void ShowConclusion(string characterImageName)
    {
        // Update conclusion canvas background
        Sprite sprite = Resources.Load<Sprite>($"Images/Summons/{characterImageName}");
        if (sprite == null)
        {
            // Log error and show replacement image if sprite not found
            Debug.Log($"Summon Error - Couldn't load summon sprite at file path: Images/Summons/{characterImageName}");
            sprite = Resources.Load<Sprite>($"Images/Summons/image_not_found");
        }
        backgroundImage.texture = sprite.texture;

        // Show conclusion canvas after animation time
        Invoke(nameof(ShowConclusionHelper), kAnimationTime);
    }

    /// <summary>
    /// Helper function called in show conclusion to change canvas to conclusion - since Invoke can't take function parameters
    /// </summary>
    private void ShowConclusionHelper()
    {
        // Show conclusion and disable other canvases
        animationCanvas.gameObject.SetActive(false);
        conclusionCanvas.gameObject.SetActive(true);

        // Disable effects
        magicEffect.SetActive(false);
        lightningEffect.SetActive(false);
        sphereBottomEffect.SetActive(false);
        screenSplashEffect.SetActive(false);
    }

    /// <summary>
    /// Restart summons page by going to selection canvas
    /// </summary>
    public void RestartSummonsPage()
    {
        // Enable selection canvas, disabling the rest
        conclusionCanvas.gameObject.SetActive(false);
        selectionCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// Shows the rates for current summon available for each character, also shows more in-depth information when character is clicked.
    /// </summary>
    public void ShowRates()
    {
        // Show rates panel
        ratesPanel.SetActive(true);

        // Get a list of all characters for this summmon
        List<CharacterData> characters = new List<CharacterData>();
        string result = LoadCharacterRates("SummonTableDraft3.csv",ref characters);
        if (result != "")
        {
            // Show error to user on screen
            ratesErrorText.text = result;
            return;
        }

        Debug.Log($"BEFORE characters in list: {characters.Count}");

        // Sort in order of rarity (highest rarity on top, so lowest rates in front of list)
        characters.Sort((a, b) => float.Parse(a.rate).CompareTo(float.Parse(b.rate)));

        // Create a heading on list container
        GameObject headingItem = Instantiate(characterItemPrefab, listContainer);
        headingItem.SetActive(true);

        // Get the text components from prefab so they can be updated for this character
        TextMeshProUGUI headingName = headingItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI headingRarity = headingItem.transform.Find("RarityText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI headingRate = headingItem.transform.Find("RateText").GetComponent<TextMeshProUGUI>();

        // Update text components with this character's information
        headingName.text = "Character Name";
        headingRarity.text = "Rarity";
        headingRate.text = "Drop Rate";

        // Display characters on panel
        foreach (CharacterData character in characters)
        {
            // Make an item using custom prefab I made and add it to the list container
            GameObject item = Instantiate(characterItemPrefab, listContainer);
            item.SetActive(true);

            // Get the text components from prefab so they can be updated for this character
            TextMeshProUGUI nameText = item.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI rarityText = item.transform.Find("RarityText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI rateText = item.transform.Find("RateText").GetComponent<TextMeshProUGUI>();

            // Update text components with this character's information
            nameText.text = character.name;
            rarityText.text = character.rarity.ToUpper();
            rateText.text = character.rate + "%";

            // Add functionality to the button component
            Button button = item.transform.Find("ButtonBackground").GetComponent<Button>();
            button.onClick.AddListener(() => ShowCharacter(character));
        }
    }

    /// <summary>
    /// Display another panel with the character background image as well as thier stats to user.
    /// </summary>
    /// <param name="character"> struct containing necessary character details for the one to be displayed </param>
    private void ShowCharacter(CharacterData character)
    {
        // Give character background image to panel
        Sprite sprite = Resources.Load<Sprite>($"Images/Summons/{character.imagePath}");
        if (sprite == null)
        {
            // Log error and show replacement image if sprite not found
            Debug.Log($"Summon Error - Couldn't load summon sprite at file path: Images/Summons/{character.imagePath}");
            sprite = Resources.Load<Sprite>($"Images/Summons/image_not_found");
        }
        imagePanelBackground.texture = sprite.texture;

        // Update panel to include character stats
        hpPanelText.text = character.stats[0];
        attackPanelText.text = character.stats[1];
        defencePanelText.text = character.stats[2];

        // Open image panel for display to user
        imagePanel.SetActive(true);
    }

    /// <summary>
    /// Closes rates panel, called by close button on panel.
    /// </summary>
    public void CloseRatesPanel()
    {
        ratesPanel.SetActive(false);
    }

    /// <summary>
    /// Close image panel (on top of rates) called by close button on panel
    /// </summary>
    public void CloseImagePane()
    {
        imagePanel.SetActive(false);
    }

    /// <summary>
    /// Load up all character information, including rates, stats and stats for every character for this particular summon.
    /// </summary>
    /// <param name="summonDbName"> string database name containing data of character which can be summoned </param>
    /// <param name="characters"> list of character database passed by reference which will be updated with data obtained </param>
    /// <returns></returns>
    private string LoadCharacterRates(string summonDbName, ref List<CharacterData> characters)
    {
        // Get rates from summon database
        string databaseFile = Path.Combine(Application.persistentDataPath, summonDbName);
        if (!File.Exists(databaseFile))
        {
            Debug.LogError($"Summon Error: Summon Database file not found at {databaseFile}");
            return "Error: Summon database could not be found. We suggest you reload the application.";
        }
        string[] summonDb = File.ReadAllLines(databaseFile);

        // Get character database information
        string characterFile = Path.Combine(Application.persistentDataPath, "CharacterTableDraft1.csv");
        if (!File.Exists(characterFile))
        {
            // Mark text as NA for not available and return early
            Debug.LogError($"Summon Error: Character database file not found at {characterFile}");
            return "Error: Character database could not be found. We suggest you reload the application.";
        }
        string[] characterDb = File.ReadAllLines(characterFile);

        // Fill character list with characters
        for (int i = 1; i < (summonDb.Length - 1); i++)
        {
            string[] summonDbValues = summonDb[i].Split(',');
            List<string> characterStats = new List<string>();

            // Find this character's stats in character database
            for (int j = 1; j < characterDb.Length; j++)
            {
                string[] data = characterDb[j].Split(',');
                if (summonDbValues[kCharacterName] == data[kCharacterName])
                {
                    characterStats.Add(data[kStatHP]);
                    characterStats.Add(data[kStatAttack]);
                    characterStats.Add(data[kStatDefence]);
                    break;
                }
            }

            // No character stats found, show user error
            if (characterStats.Count == 0)
            {
                Debug.LogError($"Summon Error: Could not find stats in character database for this character: {summonDbValues[kCharacterName]}");
                return "Error: Could not find stats for a certain character, we suggest reloading the app.";
            }

            // Add character to database list
            CharacterData characterData = new CharacterData(summonDbValues[kCharacterName], summonDbValues[kCharacterImageName], 
                summonDbValues[kCharacterRate], summonDbValues[kCharacterRarity], characterStats);
            characters.Add(characterData);
        }

        // Successful with empty return
        return "";
    }
}
