using UnityEngine;
using UnityEngine.UI;

public class ProfilePhoto : MonoBehaviour
{
    [SerializeField]
    private RawImage ProfileImage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Change image background to character equipped
        Character equipped = PlayerData.GetInstance().GetEquippedCharacter();
        Sprite sprite;
        if (equipped == null)
        {
            // Show to user character is not equipped
            Debug.LogWarning("User has no character equipped!");
            Debug.LogWarning(equipped);
            sprite = Resources.Load<Sprite>($"Images/Summons/image_not_found");
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
        ProfileImage.texture = sprite.texture;
    }
}
