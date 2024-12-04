using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScene : MonoBehaviour
{

    public void ChangeScene(int nextIndex)
    {
        SceneManager.LoadScene(nextIndex);
    }

    public void ChangeScene(string nextName)
    {
        SceneManager.LoadScene(nextName);
    }
}
