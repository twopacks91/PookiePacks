using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Summons : MonoBehaviour
{
    // *** Variables
    private const float kAnimationTime = 5.0f;  // animation time in seconds

    [SerializeField]
    private Canvas selectionCanvas;
    [SerializeField]
    private Canvas animationCanvas;
    [SerializeField]
    private Canvas conclusionCanvas;


    // *** Functions
    /// <summary>
    /// Go through the motions of a summon; user selects, animation plays, conclusion shows.
    /// </summary>
    public void StartSummon()
    {
        // Disable selection & display animation
        selectionCanvas.gameObject.SetActive(false);
        animationCanvas.gameObject.SetActive(true);

        Debug.Log("Should show ANIMATION");  // REMOVE

        // Simulate a characted pulled

        
        // Show different animation based on character rarity


        // After animation completion, show conclusion
        Invoke(nameof(ShowConclusion), kAnimationTime);

        Debug.Log("Should show CONCLUSION");  // REMOVE
    }

    /// <summary>
    /// Called in the process of summoning after animation expires
    /// </summary>
    private void ShowConclusion()
    {
        // Change conclusion canvas background to character summoned

        // Initiate a smooth transition animation (for conclusion background)

        // Show conclusion canvas
        animationCanvas.gameObject.SetActive(false);
        conclusionCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// Restart summons page by going to selection canvas
    /// </summary>
    public void RestartSummonsPage()
    {
        // Enable selection canvas, disabling the rest
        conclusionCanvas.gameObject.SetActive(false);
        selectionCanvas.gameObject.SetActive(true);

        Debug.Log("Should show SELECTION");  // REMOVE
    }
}
