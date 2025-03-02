using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;



public class Profile : MonoBehaviour
{
    public PlayerData data;
    public TMP_Text UsernameTxt;

    public void Start()
    {
        data = PlayerData.GetInstance();
        UsernameTxt.text = "Username: " + data.GetUsername();
    }

    public static Profile Instance;


    [Space]
    [Header("Profile Picture Update Data")]
    public GameObject profileUpdatePanel;
    public Image profileImage;
    public TMP_InputField urlInputField;

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void OpenCloseProfileUpdatePanel()
    {
        profileUpdatePanel.SetActive(!profileUpdatePanel.activeSelf);
    }



    public void LoadProfileImage()
    {
        StartCoroutine((LoadProfileImageIE(urlInputField.text)));
    }

    public IEnumerator LoadProfileImageIE(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());


            profileImage.sprite = sprite;
            profileUpdatePanel.SetActive(false);

        }

    }

    
}
