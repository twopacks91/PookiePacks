using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Mission
{
    public int id { get; set; }
    public string name { get; set; }
    public string details { get; set; }
    public int reward { get; set; }
    public int needed { get; set; }
    public int progress { get; set; }
    public long timeLastProgressMade { get; set; }

    public Mission(int id, string name, string details, int reward, int needed, int progress)
    {
        this.id = id;
        this.name = name;
        this.details = details;
        this.reward = reward;
        this.needed = needed;
        this.progress = progress;
        timeLastProgressMade = 0;
    }
}


public class LoadMissions : MonoBehaviour
{
    [SerializeField]
    private Canvas missionCardPrefab;

    private List<Canvas> missionCards = new List<Canvas>(0); // Stores all the mission card objects

    private List<Mission> ToDoMissions;

    private List<Mission> DoneMissions;

    private RectTransform itemContainer; // Contains all the mission cards so they can be scrollable

    PlayerData playerDataCopy;

    private const int padding = 50; // Space between mission cards

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDataCopy = new PlayerData();
        ToDoMissions = playerDataCopy.GetToDoMissions();
        DoneMissions = playerDataCopy.GetDoneMissions();

        itemContainer = (RectTransform)this.transform.Find("Mask").transform.Find("ItemContainer");
        UpdateMissionProgress();
        LoadToDoList(); // Start by loading to do list
    }


    private void OnClaimClick(int listIndex)
    {
        Debug.Log("Pressed mission:" + listIndex.ToString());
        Mission claimedMission = ToDoMissions[listIndex];
        playerDataCopy.AddMoney(claimedMission.reward);
        DoneMissions.Add(claimedMission);
        ToDoMissions.RemoveAt(listIndex);
        // TO DO: write code to claim reward from claimedMission
        playerDataCopy.SavePlayer();
        // Refresh ToDoList items
        LoadToDoList();
    }

    public void LoadToDoList()
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
            missionName.text = mission.name + " : $" + mission.reward;
            missionDetails.text = mission.details;
            progressSlider.maxValue = mission.needed;
            progressSlider.value = mission.progress;
            progressText.text = mission.progress.ToString() + "/" + mission.needed.ToString();

            // If mission is available to be claimed, display button to claim
            if(mission.needed <=mission.progress)
            {
                claimButton.gameObject.SetActive(true);

                // Give lambda function to button onClick event
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

        // Add extra height screensize-1920 so container fits within the mask 
        itemContainer.sizeDelta = new Vector2(1080, newHeight + 480);
        // Move container down after height change for proper alignment
        itemContainer.localPosition = new Vector2(0, -newHeight / 2);
    }

    long GetUnixTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    void UpdateMissionProgress()
    {
        foreach(Mission mission in playerDataCopy.GetToDoMissions())
        {
            switch(mission.id)
            {
                case 0:
                    // Go to uclan
                    LocationManager locMan = this.AddComponent<LocationManager>();
                    long timeSinceProgress = GetUnixTimestamp() - mission.timeLastProgressMade;
                    // 86400 is seconds in a day so user can only make progress on this mission once per day
                    if(locMan.IsUserAtUClan() && mission.progress < mission.needed && timeSinceProgress<=86400)
                    {
                        mission.progress += 1;
                    }
                    break;
                case 1:
                    // Win battles, idk how i am implementing this yet
                    break;
                case 2:
                    // open packs
                    break;
                case 3:
                    // Block attacks, need connor to work before I can do this
                    break;
                case 4:
                    // Pack phaval
                    // Some beautiful, readable code here
                    // Just checks to see if phaval is in the users inventory and the mission isnt already complete
                    if(playerDataCopy.GetCharacters().Select(m => m.GetName().ToLower()).Contains("phaval") && mission.progress < mission.needed)
                    {
                        mission.progress += 1;
                    }
                    break;

            }
        }
    }
}
