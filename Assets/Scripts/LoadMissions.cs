using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

struct Mission
{
    public string name;
    public string details;
    public int needed;
    public int progress;

    
}


public class LoadMissions : MonoBehaviour
{
    [SerializeField]
    private Canvas missionCardPrefab;

    private List<Canvas> missionCards = new List<Canvas>(0); // Stores all the item cards

    private List<Mission> ToDoMissions = new List<Mission>(0);

    private List<Mission> DoneMissions = new List<Mission>(0);

    private RectTransform itemContainer;

    private const int padding = 50; // Space between mission cards

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This will all be pulled from database/playerdata eventually
        ToDoMissions.Add(new Mission { name = "Go to UCLAN", details = "These details are details\nReward: 20 bucks", needed = 6, progress = 2 });
        ToDoMissions.Add(new Mission { name = "Donate to us", details = "Please send 0.465 BTC to my wallet", needed = 1, progress = 0 });
        ToDoMissions.Add(new Mission { name = "Win battles", details = "Reward: 40 buckeroos", needed = 6, progress = 4 });
        ToDoMissions.Add(new Mission { name = "Open diddy packs", details = "Reward: 1 skibidi pack", needed = 4, progress = 2 });
        ToDoMissions.Add(new Mission { name = "Anotha one", details = "Tell em to bring out the whole ocean", needed = 1, progress = 1 });
        ToDoMissions.Add(new Mission { name = "Anotha one", details = "Tell em to bring out the whole ocean", needed = 1, progress = 1 });

        DoneMissions.Add(new Mission { name = "Play fortnite", details = "we like fortnight", needed = 6, progress = 6 });
        //DoneMissions.Add(new Mission { name = "Play fortnite", details = "we like fortnight", needed = 6, progress = 6 });
        //DoneMissions.Add(new Mission { name = "Play fortnite", details = "we like fortnight", needed = 6, progress = 6 });
        //DoneMissions.Add(new Mission { name = "Play fortnite", details = "we like fortnight", needed = 6, progress = 6 });
        //DoneMissions.Add(new Mission { name = "Play fortnite", details = "we like fortnight", needed = 6, progress = 6 });
        //DoneMissions.Add(new Mission { name = "Play fortnite", details = "we like fortnight", needed = 6, progress = 6 });
        //DoneMissions.Add(new Mission { name = "Play fortnite", details = "we like fortnight", needed = 6, progress = 6 });
        //DoneMissions.Add(new Mission { name = "Play fortnite", details = "we like fortnight", needed = 6, progress = 6 });
        //DoneMissions.Add(new Mission { name = "Play fortnite", details = "we like fortnight", needed = 6, progress = 6 });


        itemContainer = (RectTransform)this.transform.Find("Mask").transform.Find("ItemContainer");

        LoadToDoList(); // Start by loading to do list
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnClaimClick(int listIndex)
    {
        Debug.Log("Pressed mission:" + listIndex.ToString());
        Mission claimedMission = ToDoMissions[listIndex];
        DoneMissions.Add(claimedMission);
        ToDoMissions.RemoveAt(listIndex);
        // TO DO: write code to claim reward from claimedMission

        // Refresh ToDoList items
        LoadToDoList();
    }

    public void LoadToDoList()
    {
        // Clear all mission cards from the screen
        foreach(Canvas missionCanvas in missionCards)
        {
            Destroy(missionCanvas.gameObject);
        }
        missionCards.Clear();

        // Resize itemContainer to fit all items
        float canvasHeight = missionCardPrefab.GetComponent<RectTransform>().rect.height;
        float newHeight = 1920.0f;
        int missionCount = ToDoMissions.Count;
        if (missionCount > 3)
        {
            for (int i = 1; i < missionCount - 3; i++)
            {
                newHeight += padding + canvasHeight;
            }
        }
        itemContainer.sizeDelta = new Vector2(1080, newHeight);
        // Move container down after height change for proper alignment
        itemContainer.localPosition = new Vector2(0, -newHeight / 2);

        int row = 0;
        foreach (Mission mission in ToDoMissions)
        {
            // Create new missionCanvas from prefab
            Canvas missionCanvas = Instantiate(missionCardPrefab, itemContainer);

            // Create variables for each gameObject in prefab
            TextMeshProUGUI missionName = missionCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
            TextMeshProUGUI missionDetails = missionCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1];
            Slider progressSlider = missionCanvas.GetComponentsInChildren<Slider>()[0];
            TextMeshProUGUI progressText = progressSlider.GetComponentsInChildren<TextMeshProUGUI>()[0];
            Button claimButton = missionCanvas.GetComponentsInChildren<Button>()[0];

            // Assign gameObjects with relevant information
            missionName.text = mission.name;
            missionDetails.text = mission.details;
            progressSlider.maxValue = mission.needed;
            progressSlider.value = mission.progress;
            progressText.text = mission.progress.ToString() + "/" + mission.needed.ToString();

            // If mission is available to be claimed, display button to claim
            if(mission.needed <=mission.progress)
            {
                claimButton.gameObject.SetActive(true);

                // Give lambda function to button onClick
                int butIndex = row;
                claimButton.onClick.AddListener(() => OnClaimClick(butIndex));
            }
            else
            {
                claimButton.gameObject.SetActive(false);
            }

            // Change instance position
            missionCanvas.transform.localPosition = new Vector2(0, -padding - (canvasHeight / 2) - (row * (canvasHeight + padding)) + newHeight / 2);

            missionCards.Add(missionCanvas);
            row++;
        }

        // Add extra height screensize-1920 so it fits within the mask 
        itemContainer.sizeDelta = new Vector2(1080, newHeight + 480);
        // Move container down after height change for proper alignment
        itemContainer.localPosition = new Vector2(0, -newHeight / 2);
    }

    public void LoadDoneList()
    {
        // Clear all mission cards from the screen
        foreach (Canvas missionCanvas in missionCards)
        {
            Destroy(missionCanvas.gameObject);
        }
        missionCards.Clear();

        // Resize itemContainer to fit all items
        float canvasHeight = missionCardPrefab.GetComponent<RectTransform>().rect.height;
        float newHeight = 1920.0f;
        int missionCount = DoneMissions.Count;
        if (missionCount > 3)
        {
            for (int i = 1; i < missionCount - 3; i++)
            {
                newHeight += padding + canvasHeight;
            }
        }
        itemContainer.sizeDelta = new Vector2(1080, newHeight);
        // Move container down after height change for proper alignment
        itemContainer.localPosition = new Vector2(0, -newHeight / 2);

        int row = 0;
        foreach (Mission mission in DoneMissions)
        {
            // Create new missionCanvas from prefab
            Canvas missionCanvas = Instantiate(missionCardPrefab, itemContainer);

            // Create variables for each gameObject in prefab
            TextMeshProUGUI missionName = missionCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
            TextMeshProUGUI missionDetails = missionCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1];
            Slider progressSlider = missionCanvas.GetComponentsInChildren<Slider>()[0];
            TextMeshProUGUI progressText = progressSlider.GetComponentsInChildren<TextMeshProUGUI>()[0];
            Button claimButton = missionCanvas.GetComponentsInChildren<Button>()[0];

            // Assign gameObjects with relevant information
            missionName.text = mission.name;
            missionDetails.text = mission.details;
            progressSlider.maxValue = mission.needed;
            progressSlider.value = mission.progress;
            progressText.text = mission.progress.ToString() + "/" + mission.needed.ToString();
            claimButton.gameObject.SetActive(false);

            // Change instance position
            missionCanvas.transform.localPosition = new Vector2(0, -padding - (canvasHeight / 2) - (row * (canvasHeight + padding)) + newHeight / 2);

            missionCards.Add(missionCanvas);
            row++;
        }

        // Add extra height screensize-1920 so it fits within the mask 
        itemContainer.sizeDelta = new Vector2(1080, newHeight + 480);
        // Move container down after height change for proper alignment
        itemContainer.localPosition = new Vector2(0, -newHeight / 2);
    }
}
