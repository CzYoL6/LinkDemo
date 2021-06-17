using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

//for custom class, add this to make it visible in Unity
[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string name;
    [Range(.1f, 3f)]
    public float pitch;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

}
