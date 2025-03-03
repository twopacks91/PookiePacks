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

    public void Awake()
    {
        List<Button> buttons = new List<Button>(0);
        Scene currentScene = gameObject.scene;

        // Find all canvases in the scene
        Canvas[] canvases = currentScene.GetRootGameObjects()
                                        .SelectMany(root => root.GetComponentsInChildren<Canvas>())
                                        .ToArray();
        foreach(Canvas canvas in canvases)
        {
            buttons.AddRange(GetAllComponents<Button>(canvas.transform));
        }
        
        if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            audioClip = initAudioClip;
            audioSource.clip = audioClip;
        }
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
}
