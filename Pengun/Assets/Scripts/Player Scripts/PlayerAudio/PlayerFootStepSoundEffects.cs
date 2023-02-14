using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStepSoundEffects : MonoBehaviour
{
    //NEEDS FOOTSTEP VARIETY BASED ON MATERIAL PENG IS STANDING ON = METAL, CONCRETE, DIRT, ETC
    public AudioClip[] footStepClips;
    public AudioSource footStepAudioSource;
    
   void PlayFootStep()
    {
        footStepAudioSource.clip = footStepClips[Random.Range(0, footStepClips.Length)];
        footStepAudioSource.Play();
    }
}
