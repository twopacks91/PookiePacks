using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApplyThemeToCanvas : MonoBehaviour
{
    [SerializeField] 
    private Canvas givenCanvas;

    [SerializeField]
    private Texture2D background;

    //[SerializeField]
    //private List<>

    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();
    private List<RawImage> rawImageList = new List<RawImage>();
    private List<Image> imageList = new List<Image>();

    private Color darkModeTextColour = new Color(0.836989f, 0.8242257f, 0.9245283f);
    private Color darkModeImageColour = new Color(0.3018868f, 0.1181915f, 0.2277126f);
    private Color darkModeBackgroundImageColor = new Color(1.0f, 0.514151f, 0.8035929f);

    private Color lightModeTextColour = new Color(0.0f, 0.0f, 0.0f);
    private Color lightModeImageColour = new Color(1.0f, 1.0f, 1.0f);

    private PlayerData playerData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        playerData = PlayerData.GetInstance();

        FullRefresh();
        
    }

    // Re-searches all gameobjects for text and images then sets their theme
    public void FullRefresh()
    {
        textList.Clear();
        rawImageList.Clear();
        imageList.Clear();

        textList = GetAllComponents<TextMeshProUGUI>(givenCanvas.transform);
        rawImageList = GetAllComponents<RawImage>(givenCanvas.transform);
        imageList = GetAllComponents<Image>(givenCanvas.transform);

        Refresh();

    }

    // Changes UI elements in given canvas to match the theme in playerData
    public void Refresh()
    {
        bool useDarkMode = playerData.GetSettings()[0].enabled;

        if (useDarkMode)
        {
            foreach (TextMeshProUGUI text in textList)
            {
                text.color = darkModeTextColour;
            }
            foreach (RawImage image in rawImageList)
            {
                if (image.texture == background)
                {
                    image.color = darkModeBackgroundImageColor;
                }
                else
                {
                    image.color = darkModeImageColour;
                }

            }
            foreach (Image image in imageList)
            {
                image.color = darkModeImageColour;
            }
        }
        else
        {
            foreach (TextMeshProUGUI text in textList)
            {
                text.color = lightModeTextColour;
            }
            foreach (RawImage image in rawImageList)
            {
                if (image.texture == background)
                {
                    image.color = lightModeImageColour;
                }
                else
                {
                    image.color = lightModeImageColour;
                }
            }
            foreach (Image image in imageList)
            {
                image.color = lightModeImageColour;
            }
        }
    }


    private List<T> GetAllComponents<T>(Transform parent) where T : Component
    {
        List<T> resultList = new List<T>();

        foreach (Transform child in parent)
        {
            T component = child.GetComponent<T>();
            if (component != null)
            {
                resultList.Add(component);
            }

            // Recursively search in children
            resultList.AddRange(GetAllComponents<T>(child));
        }

        return resultList;
    }
}
