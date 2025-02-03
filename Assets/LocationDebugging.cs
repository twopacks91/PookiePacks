using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;



public class LocationDebugging : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textBox;

    private LocationManager locationManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LocationManager locationManager = this.transform.AddComponent<LocationManager>();
        bool isAtUni = locationManager.IsUserAtUClan();
        Debug.Log("At uni:" + isAtUni.ToString());
        if(isAtUni)
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
