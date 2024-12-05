using UnityEngine;
using TMPro;

public class Profile : MonoBehaviour
{
    public PlayerData data;
    public TMP_Text UsernameTxt;

    public void Start()
    {
        data = new PlayerData();
        UsernameTxt.text = "Username: " + data.GetUsername();
    }
}
