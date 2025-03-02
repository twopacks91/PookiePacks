using TMPro;
using UnityEngine;

public class AnotherUpdateMoney : MonoBehaviour
{
    // *** ADAPTED KAI's CODE!!!!!!!!! ***
    [SerializeField]
    private TextMeshProUGUI currencyText;

    public void UpdateMoneyFunction()
    {
        PlayerData playerData = PlayerData.GetInstance();
        currencyText.text = '$' + playerData.GetMoney().ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateMoneyFunction();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoneyFunction();
    }
}
