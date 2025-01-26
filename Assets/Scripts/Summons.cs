using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Summons : MonoBehaviour
{
    // *** Variables
    private const float kAnimationTime = 5.0f;  // animation time in seconds
    private const float kMagicDelay = 1.0f;
    private const float kLightningDelay = 3.0f;
    //List<Characters> characterList;  // FUTURE

    // Canvases
    [SerializeField]
    private Canvas selectionCanvas;
    [SerializeField]
    private Canvas animationCanvas;
    [SerializeField]
    private Canvas conclusionCanvas;

    // Animation & Effects
    [SerializeField]
    private GameObject magicEffect;
    [SerializeField]
    private GameObject lightningEffect;

    // Character Generation
    [SerializeField]
    private RawImage backgroundImage;
    [SerializeField]
    List<Sprite> characterList;  // remove once character class exists
    List<string> characterNames = new List<string> { "Mercenary", "Flamer",
        "Space Soldier", "Crabby", "Bonkzilla", "Demon Girl" }; // static setup based on sprite entry currently


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

        // TESTING DATABASE - REMOVEEEEEEEE
        GenerateCharacterNew();
        return;

        // Generate a character to display
        int characterNum = GenerateCharacter();

        // Save character to user's inventory
        SaveSummonToInventory(characterNum);

        // Show conclusion - this function accounts for animation time
        ShowConclusion(characterNum);
    }

    /// <summary>
    /// Generate a character for the specified summon
    /// </summary>
    private int GenerateCharacter()
    {
        // - - IMPORTANT = currently static, will change when more characters class exists
        // Generate a random number (will have biased rarities)
        int randomNum = UnityEngine.Random.Range(0, characterList.Count);
        Debug.Log("Random number: " + randomNum);  // REMOVE

        // Modify animation based on character rarity
        if (randomNum >= 3)  // silver rarity or gold
        {
            Invoke(nameof(PlayMagicEffect), kMagicDelay);
            if (randomNum >= 5)  // gold rarity
            {
                Invoke(nameof(PlayLightningEffect), kLightningDelay);
            }
        }

        // Return index of character generated
        return randomNum;
    }

    private void GenerateCharacterNew()
    {
        // Use summon database file, relative to current path
        string databaseFile = Path.Combine(Application.dataPath, "Scripts", "SummonTableDraft1.csv");
        if (!File.Exists(databaseFile))
        {
            Debug.LogError($"Summon Error: Database file not found at {databaseFile}");
            return;
        }

        // Read database file
        string[] database = File.ReadAllLines(databaseFile);

        // In database - last line & column 4 contains total rates of all characters in database currently
        string lastLine = database[database.Length - 1];
        string[] totalRateData = lastLine.Split(',');
        if (!float.TryParse(totalRateData[totalRateData.Length - 1], out float totalRates))
        {
            Debug.LogError("Summon Error - Unable to get total rates from database");
            return;
        }

        Debug.Log($"Read successfully as {totalRates}");  // REMOVE

        // Generate random number in current range of rates
        float randomValue = UnityEngine.Random.Range(0, totalRates);

        Debug.Log($"RANDOM VALUE WAS - {randomValue}");  // REMOVE

        // Find character pulled using cumulative rates
        float cumulativeRate = 0;
        string[] characterData;
        for (int i = 1; i < database.Length - 1; i++)  // ignore first line (headers) and last
        {
            // Get rate for current character and add to cumulative rate
            string[] data = database[i].Split(',');
            if (!float.TryParse(data[data.Length - 1], out float rate))
            {
                Debug.LogError($"Summon Error - Error parsing character rate {data[data.Length - 1]}");
                return;
            }
            cumulativeRate += rate;

            // Get's the character which is next in rate
            if (randomValue <= cumulativeRate)
            {
                // Return some sort of character
                Debug.Log($"YEYYYYYYYYY we pulled - {data[1]}");  // REMOVE
                characterData = data;
                break;
            }
        }

        // *********** Continue coding after Fulham beat Man Utd D; ********************
        // Load Image to display

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
    /// Called in the process of summoning to save the character to the user's inventory
    /// </summary>
    /// <param name="characterNum"></param>
    private void SaveSummonToInventory(int characterNum)
    {
        // Make instance of character and save it to current player 
        Debug.Log("Character saved to inventory: " + characterNames[characterNum]);  // REMOVE

        Character newChar = new Character(characterList[characterNum].texture.ToString(), characterNames[characterNum], 
            new List<string> { "Snoop", "Doggy", "Dawg", "should have power values for character" });

        Debug.Log("Made new character");  // REMOVE

        PlayerData currentPlayer = new PlayerData();

        Debug.Log("Made new player");  // REMOVE

        currentPlayer.InsertCharacter(newChar);

        Debug.Log("Inserted new character");  // REMOVE

        currentPlayer.SavePlayer();

        Debug.Log("SSUCCESS in saving to inventory");  // REMOVE
    }

    /// <summary>
    /// Called in the process of summoning after animation expires
    /// </summary>
    private void ShowConclusion(int charNum)
    {
        // Change conclusion canvas background to character summoned
        backgroundImage.texture = characterList[charNum].texture;

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
}
