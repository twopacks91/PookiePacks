using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BattleSelection : MonoBehaviour
{
    // *** Variables

    // Canvas Elements
    [SerializeField]
    private GameObject battleSelectionPanel;
    [SerializeField]
    private RawImage panelImageBackground;
    [SerializeField]
    private Button equipCharacterText;
    [SerializeField]
    private Button pveButton;
    [SerializeField]
    private TextMeshProUGUI pveTextField;
    [SerializeField]
    private Button pvpButton;

    /// <summary>
    /// Load battle selection panel with necessary content (equipped character image)
    /// </summary>
    public void ShowBattleSelection()
    {
        // Show panel
        equipCharacterText.gameObject.SetActive(false);
        battleSelectionPanel.SetActive(true);
        battleSelectionPanel.GetComponent<Image>().color = new Color(152f,152f,152f,190f);
        pvpButton.interactable = false;

        // Change image background to character equipped
        Character equipped = PlayerData.GetInstance().GetEquippedCharacter();
        Sprite sprite;
        if (equipped == null)
        {
            // Show to user character is not equipped
            Debug.LogWarning("User has no character equipped!");
            sprite = Resources.Load<Sprite>($"Images/Summons/image_not_found");
            equipCharacterText.gameObject.SetActive(true);

            // Change pve colour to show its inactive and unclickable
            pveTextField.color = Color.white;
            pveButton.colors = pvpButton.colors;
            pveButton.interactable = false;
        }
        else
        {
            sprite = Resources.Load<Sprite>($"Images/Summons/{equipped.GetImage()}");
            if (sprite == null)
            {
                // Log error and show replacement image if sprite not found
                Debug.Log($"Summon Error - Couldn't load summon sprite at file path: Images/Summons/{equipped.GetImage()}");
                sprite = Resources.Load<Sprite>($"Images/Summons/image_not_found");
            }
        }
        panelImageBackground.texture = sprite.texture;
    }


    /// <summary>
    /// Closes battle selection panel
    /// </summary>
    public void CloseBattleSelection()
    {
        // Reset panel and make inactive
        equipCharacterText.gameObject.SetActive(false);
        battleSelectionPanel.SetActive(false);
    }
}
