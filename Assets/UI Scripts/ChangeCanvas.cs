using UnityEngine;

public class Click : MonoBehaviour
{
    public Canvas nextCanvas;

    public void ChangeCanvas()
    {
        this.transform.parent.gameObject.SetActive(false);
        nextCanvas.gameObject.SetActive(true);
    }

}
