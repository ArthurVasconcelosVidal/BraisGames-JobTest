using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundWhenCollide : MonoBehaviour{
    [SerializeField] SoundList soundName;
    AudioManager AudioManager { get => AudioManager.instance; }
    void OnCollisionEnter(Collision other) {
            AudioManager.PlayAudio(soundName);
    }
 
}
