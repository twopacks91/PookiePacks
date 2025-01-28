using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class LocationDebugging : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textBox;

    private LocationManager locationManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        locationManager = new LocationManager();
        if(locationManager.IsUserAtUClan())
        {
            textBox.text = "You are at UCLan, heres 500 dollar!";
        }
        else
        {
            textBox.text = "You either arent at uclan or my code is shit";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
