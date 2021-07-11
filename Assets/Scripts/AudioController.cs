using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public Dictionary<string,AudioClip> audioDictionary;
    public AudioSource audioClipSource;
    public static AudioController _instance;

    public void RegisterAudioClip(string name, string url)
    {
        audioDictionary.Add(name,Resources.Load<AudioClip>(url));
    }

    void Awake()
    {
        if (_instance == null) _instance = this;

        audioDictionary = new Dictionary<string, AudioClip>();
        audioClipSource = GetComponent<AudioSource>();

        //  º”“Ù–ß
        RegisterAudioClip("Wind", "Audio/SoundFx/windSound");
    }

    public bool PlayAudioClip(string name)
    {
        AudioClip clip;
        if (audioDictionary.TryGetValue(name, out clip))
        {
            audioClipSource.PlayOneShot(clip);
            return true;
        }
        return false;
    }
}
