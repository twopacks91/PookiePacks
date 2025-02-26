using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

[System.Serializable]
public class Setting
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public bool enabled { get; set; }
    public Setting(int id, string name, string description, bool enabled)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.enabled = enabled;
    }
}



public class LoadSettings : MonoBehaviour
{
    [SerializeField]
    private Canvas settingCardPrefab;

    [SerializeField]
    private RectTransform itemContainer;

    [SerializeField]
    private Texture2D enabledImage;
    [SerializeField]
    private Texture2D disabledImage;

    private List<Canvas> settingsCards;

    private PlayerData playerData;

    private List<Setting> settings;



    private const int padding = 50;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsCards = new List<Canvas>(0);
        PlayerData playerData = PlayerData.GetInstance();
        settings = playerData.GetSettings();
        LoadSettingsList();


    }

    private void OnSettingClick(int index)
    {
        Setting setting = settings[index];

        Debug.Log("Changed \"" + setting.name + "\" to " + (setting.enabled ? "disabled" : "enabled"));
        if(setting.enabled)
        {
            setting.enabled = false;
            Button toggleButton = settingsCards[index].GetComponentsInChildren<Button>()[0];
            
            RawImage buttonImage = toggleButton.transform.GetChild(0).GetComponent<RawImage>();
            buttonImage.texture = (setting.enabled ? enabledImage : disabledImage);
        }
        else
        {
            setting.enabled = true;
            Button toggleButton = settingsCards[index].GetComponentsInChildren<Button>()[0];
            RawImage buttonImage = toggleButton.transform.GetChild(0).GetComponent<RawImage>();
            buttonImage.texture = (setting.enabled ? enabledImage : disabledImage);
        }
        playerData.SavePlayer();
    }

    public void LoadSettingsList()
    {
        // Clear all mission cards from the screen
        foreach (Canvas missionCanvas in settingsCards)
        {
            Destroy(missionCanvas.gameObject);
        }
        settingsCards.Clear();

        // Resize itemContainer to fit all items
        float canvasHeight = settingCardPrefab.GetComponent<RectTransform>().rect.height;
        float newHeight = 1920.0f;
        int missionCount = settings.Count;
        if (missionCount > 7)
        {
            for (int i = 1; i < missionCount - 7; i++)
            {
                newHeight += padding + canvasHeight;
            }
        }
        itemContainer.sizeDelta = new Vector2(1080, newHeight);
        // Move container down after height change for proper alignment
        itemContainer.localPosition = new Vector2(0, -newHeight / 2);

        int row = 0;
        foreach (Setting setting in settings)
        {
            // Create new settingCanvas from prefab
            Canvas settingCanvas = Instantiate(settingCardPrefab, itemContainer);

            // Create variables for each gameObject in prefab
            TextMeshProUGUI settingName = settingCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[0];
            TextMeshProUGUI settingDescription = settingCanvas.GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1];
            Button toggleButton = settingCanvas.GetComponentsInChildren<Button>()[0];
            RawImage buttonImage = toggleButton.transform.GetChild(0).GetComponent<RawImage>();

            // Assign gameObjects with relevant information
            settingName.text = setting.name;
            settingDescription.text = setting.description;
            buttonImage.texture = (setting.enabled? enabledImage : disabledImage);
            int index = row;
            toggleButton.onClick.AddListener(() => OnSettingClick(index));

            // Change instance position
            settingCanvas.transform.localPosition = new Vector2(0, 240 -padding - (canvasHeight / 2) - (row * (canvasHeight + padding)) + newHeight / 2);

            settingsCards.Add(settingCanvas);
            row++;
        }

        // Add extra height screensize-1920 so container fits within the mask 
        itemContainer.sizeDelta = new Vector2(1080, newHeight + 480);
        // Move container down after height change for proper alignment
        itemContainer.localPosition = new Vector2(0, -newHeight / 2);
    }
}
