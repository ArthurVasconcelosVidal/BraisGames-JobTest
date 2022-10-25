using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour{
    public static AudioManager instance;
    [SerializeField] Sound[] sounds;
    void Awake() {
        SingletonPattern();
        AudioStartConfig();
    }

    void SingletonPattern(){
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start() {
        PlayAudio(SoundList.MainTheme);    
    }

    void AudioStartConfig(){
        foreach (var item in sounds){
            item.AudioSource = gameObject.AddComponent<AudioSource>();
            item.AudioSource.clip = item.audioClip;
            item.AudioSource.pitch = item.pitch;
            item.AudioSource.loop = item.loop;
            item.AudioSource.volume = item.volume;
        }
    }

    public void PlayAudio(SoundList soundName){
        Sound sound = Array.Find(sounds, sound => sound.soundName == soundName );
        if(sound != null) sound.AudioSource.Play();
    }

    public void PlayAudio(AudioSource audioSource) => audioSource.Play();

    public void StopAudio(SoundList soundName){
        Sound sound = Array.Find(sounds, sound => sound.soundName == soundName );
        if(sound != null) sound.AudioSource.Stop();
    }

    public void StopAudio(AudioSource audioSource) => audioSource.Stop();

    public Sound GetSound(SoundList soundName){
        Sound sound = Array.Find(sounds, sound => sound.soundName == soundName );
        return sound;
    }
}
