using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LoadInventory : MonoBehaviour
{
    [SerializeField]
    private RectTransform itemContainer;

    [SerializeField]
    private Texture2D defaultTexture;    // Default texture for item pictures
    [SerializeField]
    private Canvas itemCardPrefab;      // Prefab for each item card

    [SerializeField]
    private Canvas itemViewerCanvas;    // Canvas to switch to when item pressed

    private List<Canvas> inventorySlots = new List<Canvas>(0); // Stores all the item cards

    private List<Character> characters;

    void Start()
    {
        PlayerData playerData = PlayerData.GetInstance();
        characters = playerData.GetCharacters();
        int itemCount = characters.Count;

        // If no characters are present, do nothing
        // We SHOULD indicate to the player their inventory is empty
        // I am lazy
        if(itemCount==0)
        {
            return;
        }

        int padding = 50; // Space between cards
        int row = 0;

        // Get dimensions of itemViewPrefab
        float canvasHeight = itemCardPrefab.GetComponent<RectTransform>().rect.height;
        float canvasWidth = itemCardPrefab.GetComponent<RectTransform>().rect.width;

        

        // Resize the height of the item container acording to how many items are being displayed
        float newHeight=2400.0f;
        if(itemCount>4)
        {
            for(int i = 0;i<itemCount-4;i++)
            {
                if(i%2==0)
                {
                    newHeight += padding + canvasHeight;
                }
            }
        }
        itemContainer.sizeDelta = new Vector2(1080,newHeight);
        // Move container down after height change for proper alignment
        itemContainer.localPosition = new Vector2(0,-newHeight/2);


        for(int i=0;i<itemCount;i++)
        {
            // Create new canvas gameobject from item view prefab and add to itemContainer as child
            Canvas itemCanvas = Instantiate(itemCardPrefab,itemContainer);

            // Background image for each "card"
            RawImage background = itemCanvas.transform.Find("Background").GetComponent<RawImage>();

            // Picture to display each item
            RawImage picture = itemCanvas.GetComponentsInChildren<RawImage>()[1];

            string rarity = characters[i].GetRarity();
            Debug.Log(rarity);
            if (rarity == "bronze")
            {
                background.color = new Color(139.0f / 255.0f, 69.0f / 255.0f, 19.0f / 255.0f);
            }
            else if (rarity == "silver")
            {
                background.color = new Color(220.0f / 255.0f, 220.0f / 255.0f, 220.0f / 255.0f);
            }
            else if (rarity == "gold")
            {
                background.color = new Color(128.0f / 255.0f, 0.0f / 255.0f, 128.0f / 255.0f);
            }
            else if (rarity == "diamond")
            {
                background.color = new Color(185.0f / 255.0f, 242.0f / 255.0f, 255.0f / 255.0f);
            }
            else if (rarity == "legendary")
            {
                background.color = new Color(89.0f / 255.0f, 70.0f / 255.0f, 178.0f / 255.0f);
            }
            else
            {
                background.color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
            }

            Sprite sprite = Resources.Load<Sprite>($"Images/Summons/{characters[i].GetImage()}");
            picture.texture = sprite.texture;
            //itemCanvas.transform.Find("Background").GetComponent<RawImage>().color = Color.black;
            // Text to contain item name
            TMPro.TextMeshProUGUI itemName = itemCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
            //itemName.text = characters[i].GetName();
            itemName.text = "";
            itemName.color = Color.black;

            // Button covering canvas to determine wether item has been clicked
            Button button = itemCanvas.GetComponentsInChildren<Button>()[0];
            int buttonNum = i;
            button.onClick.AddListener(() => OnItemClick(buttonNum)); // Assigns each button lambda function with its own button number

            //Track row count
            if(i%2==0 && i!=0)
            {
                row++;
            }

            // Move each card left/right a lil depending on weather i is odd or even
            // Sloppy and unreadable but it works trust
            if(i%2==0)
            {
                itemCanvas.transform.localPosition = new Vector2(-(padding+canvasWidth)/2,-padding-(canvasHeight/2)-(row*(canvasHeight+padding))+newHeight/2);
            }
            else
            {
                itemCanvas.transform.localPosition = new Vector2((padding+canvasWidth)/2,-padding-(canvasHeight/2)-(row*(canvasHeight+padding))+newHeight/2);
            }
            
            // Add each instantiated prefab to list
            inventorySlots.Add(itemCanvas);
            
        }
        
        
    }

    // Method to be called when an item is pressed, changes to ItemViewer canvas and updates its children
    private void OnItemClick(int itemIndex)
    {
        RawImage itemImage = itemViewerCanvas.transform.GetComponentsInChildren<RawImage>()[1];
        //TMPro.TextMeshProUGUI itemNameText = itemViewerCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
        TMPro.TextMeshProUGUI itemDescriptionText = itemViewerCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
        Button equipButton = itemViewerCanvas.GetComponentsInChildren<Button>()[0];

        Sprite sprite = Resources.Load<Sprite>($"Images/Summons/{characters[itemIndex].GetImage()}");
        itemImage.texture = sprite.texture;
        //itemImage.texture = characters[itemIndex].GetImage(); // FIX THIS FINNNNN
        //itemNameText.text = characters[itemIndex].GetName();
        itemDescriptionText.text = "";
        List<string> attributes = characters[itemIndex].GetAttributes();
        foreach(string s in attributes)
        {
            itemDescriptionText.text += s + '\n';
        }
        

        
        bool isEquipped = characters[itemIndex].IsEquipped();

        // Remove previously added listeners to refresh them
        equipButton.onClick.RemoveAllListeners();

        // Change button text and colour based on if item is equipped
        // Also add listener to button to be able to equip/unequip a character
        if (isEquipped)
        {
            equipButton.GetComponent<Image>().color = Color.red;
            equipButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Unequip";
            int index = itemIndex;
            equipButton.onClick.AddListener(() => OnEquipClick(index, false));
        }
        else
        {
            equipButton.GetComponent<Image>().color = Color.green;
            equipButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Equip";
            int index = itemIndex;
            equipButton.onClick.AddListener(() => OnEquipClick(index, true));
        }

        // Change to ItemViewer
        this.transform.gameObject.SetActive(false);
        itemViewerCanvas.transform.gameObject.SetActive(true);
        
    }

    private void OnEquipClick(int index, bool setEquipped)
    {
        if(setEquipped)
        {
            foreach (Character character in characters)
            {
                character.Dequip();
            }
            characters[index].Equip();
        }
        else
        {
            characters[index].Dequip();
        }
        PlayerData pd = PlayerData.GetInstance();
        pd.SavePlayer();
        OnItemClick(index);
    }

}
