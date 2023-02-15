using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStepSoundEffects : MonoBehaviour
{
    //NEEDS FOOTSTEP VARIETY BASED ON MATERIAL PENG IS STANDING ON = METAL, CONCRETE, DIRT, ETC
    public AudioClip[] footStepClips;
    public AudioSource footStepAudioSource;
    PlayerMovement playermovement;
    public float sprintPitch;
    public float originalPitch;

    private void Start()
    {
        originalPitch = footStepAudioSource.pitch;
        playermovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (playermovement.getIsFastRunning())
        {
            footStepAudioSource.pitch = sprintPitch;
        }
        else
        {
            footStepAudioSource.pitch = originalPitch;
        }
    }

    void PlayFootStep()
    {
        footStepAudioSource.clip = footStepClips[Random.Range(0, footStepClips.Length)];
        footStepAudioSource.Play();
    }
}
