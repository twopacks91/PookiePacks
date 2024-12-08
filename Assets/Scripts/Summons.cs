using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


        // Generate a character to display
        int characterNum = GenerateCharacter();

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
