using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static Dictionary<string,AudioClip> audioDictionary;
    public static AudioSource audioClipSource;

    public static void RegisterAudioClip(string name, string url)
    {
        audioDictionary.Add(name,Resources.Load<AudioClip>(url));
    }

    void Awake()
    {
        audioDictionary = new Dictionary<string, AudioClip>();
        audioClipSource = GetComponent<AudioSource>();

        //  ¼ÓÒôÐ§
        RegisterAudioClip("Wind", "Audio/SoundFx/windSound");


        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public static void PlayAudioClip(string name)
    {
        AudioClip clip;
        if (audioDictionary.TryGetValue(name, out clip))
        {
            audioClipSource.PlayOneShot(clip);
        }
    }
}
