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

    private List<Mission> missions = new List<Mission>(0);

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int padding = 50; // Space between mission cards

        missions.Add(new Mission { name = "Go to UCLAN", details = "These details are details\nReward: 20 bucks", needed = 6, progress = 2 });
        missions.Add(new Mission { name = "Donate to us", details = "Please send 0.465 BTC to my wallet", needed = 1, progress = 0 });
        missions.Add(new Mission { name = "Win battles", details = "Reward: 40 buckeroos", needed = 6, progress = 4 });
        missions.Add(new Mission { name = "Open diddy packs", details = "Reward: 1 skibidi pack", needed = 4, progress = 2 });
        missions.Add(new Mission { name = "Anotha one", details = "Tell em to bring out the whole ocean", needed = 1, progress = 1 });
        missions.Add(new Mission { name = "Anotha one", details = "Tell em to bring out the whole ocean", needed = 1, progress = 1 });

        RectTransform itemContainer = (RectTransform)this.transform.Find("Mask").transform.Find("ItemContainer");
        float canvasHeight = missionCardPrefab.GetComponent<RectTransform>().rect.height;

        float newHeight = 1920.0f;
        int missionCount = missions.Count;
        if (missionCount > 3)
        {
            for (int i = 1; i < missionCount-3; i++)
            {
                newHeight += padding + canvasHeight;
            }
        }
        itemContainer.sizeDelta = new Vector2(1080, newHeight);
        // Move container down after height change for proper alignment
        itemContainer.localPosition = new Vector2(0, -newHeight / 2);

        int row = 0;
        foreach (Mission mission in missions)
        {
            
            Canvas missionCanvas = Instantiate(missionCardPrefab, itemContainer);
            

            TextMeshProUGUI missionName = missionCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
            TextMeshProUGUI missionDetails = missionCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1];
            Slider progressSlider = missionCanvas.GetComponentsInChildren<Slider>()[0];
            TextMeshProUGUI progressText = progressSlider.GetComponentsInChildren<TextMeshProUGUI>()[0];

            missionName.text = mission.name;
            missionDetails.text = mission.details;
            progressSlider.maxValue = mission.needed;
            progressSlider.value = mission.progress;
            progressText.text = mission.progress.ToString() + "/" + mission.needed.ToString();

            missionCanvas.transform.localPosition = new Vector2(0, -padding - (canvasHeight / 2) - (row * (canvasHeight + padding)) + newHeight / 2);

            missionCards.Add(missionCanvas);
            row++;
        }

        // Add extra height screensize-1920 so it fits within the mask 
        itemContainer.sizeDelta = new Vector2(1080, newHeight+480);
        // Move container down after height change for proper alignment
        itemContainer.localPosition = new Vector2(0, -newHeight / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
