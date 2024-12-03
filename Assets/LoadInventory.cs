using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadInventory : MonoBehaviour
{
    [SerializeField]
    private Texture2D defaultTexture;    // Default texture for item pictures
    [SerializeField]
    private Canvas itemCardPrefab;      // Prefab for each item card

    [SerializeField]
    private Canvas itemViewerCanvas;

    private int itemCount = 10;         // Should be equal to the number of items owned by the player
    private Canvas[] inventorySlots = new Canvas[10]; // Stores all the item cards, size should be equal to itemCount

     // Should change dynamically based on screen size maybe idk im guessing this whole thing
    void Start()
    {
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
            picture.texture = defaultTexture;

            // Text to contain item name and properties
            TMPro.TextMeshProUGUI itemName = itemCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
            itemName.text = "Item " + i.ToString();
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
            
            // Add each instantiated prefab to array
            inventorySlots[i] = itemCanvas;
            
        }
        
        
    }

    // Method to be called when an item is pressed, changes to ItemViewer canvas and updates its children
    private void OnItemClick(int itemIndex)
    {
        RawImage itemImage = itemViewerCanvas.transform.GetComponentInChildren<RawImage>();
        TMPro.TextMeshProUGUI itemNameText = itemViewerCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
        TMPro.TextMeshProUGUI itemDescriptionText = itemViewerCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1];
        Button equipButton = itemViewerCanvas.GetComponentsInChildren<Button>()[0];

        // Code to be used as such:
        // itemImage.texture = (array containing all the items owned by player)[itemIndex].image
        // All children updated in the same way
        
        itemImage.texture = defaultTexture;
        itemNameText.text = "Wild Warwick";
        itemDescriptionText.text = "+10 Damage\n+20 Attack Speed\n-30 Height";


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
