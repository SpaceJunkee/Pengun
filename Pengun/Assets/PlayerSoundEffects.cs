using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{

    public AudioClip[] footStepClips;
    public AudioSource foorStepAudioSource;
    
   void PlayFootStep1()
    {
        foorStepAudioSource.clip = footStepClips[Random.Range(0, footStepClips.Length)];
        foorStepAudioSource.Play();
    }

    void PlayFootStep2()
    {
        foorStepAudioSource.clip = footStepClips[Random.Range(0, footStepClips.Length)];
        foorStepAudioSource.Play();
    }
}
