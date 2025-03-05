using UnityEngine;
using TMPro;

public class ProfileMoney : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerData playerData = PlayerData.GetInstance();
        TextMeshProUGUI text = this.GetComponent<TextMeshProUGUI>();
        text.text = "Currency: $" + playerData.GetMoney();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
