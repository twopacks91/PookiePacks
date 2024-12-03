using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public bool fakeLoad;
    private Slider loadingBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingBar = this.transform.Find("LoadingBar").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!fakeLoad)
        {
            SceneManager.LoadScene(1);
            return;
        }
        if(loadingBar.value < 0.85)
        {
            loadingBar.value += Random.Range(0.01f,0.8f)*Time.deltaTime;
        }
        else if(loadingBar.value >=0.85)
        {
            loadingBar.value += Random.Range(0.001f,0.08f)*Time.deltaTime;
        }
        if(loadingBar.value >= 1)
        {
            SceneManager.LoadScene("Login");
        }
        
    }
}
