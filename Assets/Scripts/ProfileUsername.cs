using UnityEngine;
using TMPro;

public class ProfileUsername : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerData playerData = PlayerData.GetInstance();
        TextMeshProUGUI text = this.GetComponent<TextMeshProUGUI>();
        text.text = "Username: " + playerData.GetUsername();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
