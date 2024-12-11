using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadInventoryOLD : MonoBehaviour
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

    private List<CharacterOLD> characters;

    void Start()
    {
        //PlayerData playerData = new PlayerData();
        //characters = playerData.GetCharacters();
        characters = new List<CharacterOLD>(0);
        List<string> attr = new List<string>(0)
        {
            "-1000 Aura",
            "-100 Smell"
        };
        List<string> attr2 = new List<string>(0)
        {
            "-10 IQ",
            "+100 Smell"
        };
        List<string> attr3 = new List<string>(0)
        {
            "+1 Height",
            "-5000 Car"
        };
        CharacterOLD c1 = new CharacterOLD(itemTextures[0],"Muusman",attr);
        CharacterOLD c2 = new CharacterOLD(itemTextures[1],"MTahir",attr2);
        CharacterOLD c3 = new CharacterOLD(itemTextures[2],"Gwigg",attr3);
        characters.Add(c1);
        characters.Add(c2);
        characters.Add(c3);
        int itemCount = characters.Count;
        Debug.Log(itemCount);

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
        float newHeight=0.0f;
        if(itemCount/2>2)
        {
            for(int i = 0;i<itemCount/2;i++)
            {
                newHeight += padding+canvasHeight;
            }
        }
        else
        {
            newHeight = 1920;
        }
        itemContainer.sizeDelta = new Vector2(1080,newHeight);
        // Move container down after height change
        itemContainer.localPosition = new Vector2(0,-newHeight/2);


        for(int i=0;i<itemCount;i++)
        {
            // Create new canvas gameobject from item view prefab and add to InventoryCanvas as child
            Canvas itemCanvas = Instantiate(itemCardPrefab,itemContainer);     

            // Background image for each "card"
            RawImage background = itemCanvas.GetComponentsInChildren<RawImage>()[0];
            background.color = Color.white;

            // Picture to display each item
            RawImage picture = itemCanvas.GetComponentsInChildren<RawImage>()[1];
            picture.texture = characters[i].GetImage();

            // Text to contain item name and properties
            TMPro.TextMeshProUGUI itemName = itemCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
            itemName.text = characters[i].GetName();
            itemName.color = Color.black;

            // Button covering canvas to determine weather item has been clicked
            Button button = itemCanvas.GetComponentsInChildren<Button>()[0];
            int buttonNum = i;
            button.onClick.AddListener(() => OnItemClick(buttonNum));

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

        itemImage.texture = characters[itemIndex].GetImage();
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
