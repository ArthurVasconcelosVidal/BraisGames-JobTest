using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound{
    public AudioClip audioClip;
    public SoundList soundName;
    public float volume;
    public float pitch;
    public bool loop;

    public AudioSource AudioSource {get; set;}
}
