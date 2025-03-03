using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance;
    
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip initAudioClip;

    private static AudioClip audioClip;

    

    public void Awake()
    {
        List<Button> buttons = new List<Button>(0);

        // Find all root canvases
        Canvas[] canvases = gameObject.scene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<Canvas>()).ToArray();

        // Add their buttons to a list
        foreach(Canvas canvas in canvases)
        {
            buttons.AddRange(GetAllComponents<Button>(canvas.transform));
        }
        
        // Ensure the same AudioManager persists across all scenes
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Give new instance an audio source
            audioSource = gameObject.AddComponent<AudioSource>();
            audioClip = initAudioClip;
            audioSource.clip = audioClip;
        }

        // Make persistent audio manager add on click listeners to all buttons
        Instance.AddListeners(buttons);
    }

    private void AddListeners(List<Button> buttons)
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => PlayAudio());
        }
    }

    public void PlayAudio()
    {
        StartCoroutine(PlayAudioAndWait());
    }

    private IEnumerator PlayAudioAndWait()
    {
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        if(audioSource != this.GetComponent<AudioSource>())
        {
            Destroy(this.gameObject);

        }
    }

    private List<T> GetAllComponents<T>(Transform parent) where T : Component
    {
        List<T> resultList = new List<T>();
        foreach (Transform child in parent)
        {
            T component = child.GetComponent<T>();
            if (component != null)
            {
                resultList.Add(component);
            }

            // Recursively search in children
            resultList.AddRange(GetAllComponents<T>(child));
        }
        return resultList;
    }
}
