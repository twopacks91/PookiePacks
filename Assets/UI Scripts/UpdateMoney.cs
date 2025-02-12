using UnityEngine;
using TMPro;

public class UpdateMoney : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextMeshProUGUI text = this.GetComponent<TextMeshProUGUI>();
        PlayerData playerData = new PlayerData();
        text.text = '$' + playerData.GetMoney().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
