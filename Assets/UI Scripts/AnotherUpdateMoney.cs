using TMPro;
using UnityEngine;

public class AnotherUpdateMoney : MonoBehaviour
{
    // *** ADAPTED KAI's CODE!!!!!!!!! ***
    [SerializeField]
    private TextMeshProUGUI currencyText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerData playerData = PlayerData.GetInstance();
        currencyText.text = '$' + playerData.GetMoney().ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
