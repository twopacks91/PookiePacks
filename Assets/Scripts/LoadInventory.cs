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
    private Texture2D defaultTexture;    // Default texture for item pictures
    [SerializeField]
    private Canvas itemCardPrefab;      // Prefab for each item card
    [SerializeField]
    private List<Texture2D> itemTextures;

    [SerializeField]
    private Canvas itemViewerCanvas;    // Canvas to switch to when item pressed

    private List<Canvas> inventorySlots = new List<Canvas>(0); // Stores all the item cards

    private List<Character> characters;

    private List<string> characterNames = new List<string> 
    { 
        "Mercenary", 
        "Flamer",
        "Space Soldier", 
        "Crabby", 
        "Bonkzilla", 
        "Demon Girl" 
    };

    private List<string> characterRarities = new List<string> 
    { 
        "Bronze", 
        "Bronze",
        "Bronze", 
        "Silver", 
        "Silver", 
        "Gold" 
    };
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

        RectTransform itemContainer = (RectTransform)this.transform.Find("ItemContainer");

        // Resize the height of the item container acording to how many items are being displayed
        float newHeight=1920.0f;
        if(itemCount/2>=3)
        {
            for(int i = 0;i<(itemCount/2)-2;i++)
            {
                newHeight += padding+canvasHeight;
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
            RawImage background = itemCanvas.GetComponentsInChildren<RawImage>()[0];
            //itemCanvas.transform.Find("Background").GetComponent<RawImage>().color = Color.black;
            // Picture to display each item
            RawImage picture = itemCanvas.GetComponentsInChildren<RawImage>()[1];

            string rarity = characters[i].GetRarity();
            if (rarity == "Bronze")
            {
                background.color = new Color(139f / 255f, 69f / 255f, 19f / 255f);
                Debug.Log("Was bronze");
            }
            else if (rarity == "Silver")
            {
                background.color = new Color(220f / 255f, 220f / 255f, 220f / 255f);
                Debug.Log("Was silver");
            }
            else if (rarity == "Gold")
            {
                background.color = new Color(128f / 255f, 0f / 255f, 128f / 255f);
                Debug.Log("Was gold");
            }
            else
            {
                background.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
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
        RawImage itemImage = itemViewerCanvas.transform.GetComponentInChildren<RawImage>();
        TMPro.TextMeshProUGUI itemNameText = itemViewerCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
        TMPro.TextMeshProUGUI itemDescriptionText = itemViewerCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1];
        Button equipButton = itemViewerCanvas.GetComponentsInChildren<Button>()[0];

        //itemImage.texture = characters[itemIndex].GetImage(); // FIX THIS FINNNNN
        itemNameText.text = characters[itemIndex].GetName();
        itemDescriptionText.text = "";
        List<string> attributes = characters[itemIndex].GetAttributes();
        foreach(string s in attributes)
        {
            itemDescriptionText.text += s + '\n';
        }
        
        // Change button text and colour based on if item is equipped
        bool isEquipped = true;
        
        if(isEquipped)
        {
            equipButton.GetComponent<Image>().color = Color.red;
            equipButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Unequip";
        }
        else
        {
            equipButton.GetComponent<Image>().color = Color.green;
            equipButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Equip";
        }

        // Change to ItemViewer
        this.transform.gameObject.SetActive(false);
        itemViewerCanvas.transform.gameObject.SetActive(true);
        
    }
}
