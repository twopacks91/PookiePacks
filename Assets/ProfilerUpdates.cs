using UnityEngine;
using TMPro;

public class Profile : MonoBehaviour
{
    public PlayerData data;
    public TMP_Text UsernameTxt;

    public void Update()
    {
        UsernameTxt.text = "Username: " + data.GetUsername();
    }
}
