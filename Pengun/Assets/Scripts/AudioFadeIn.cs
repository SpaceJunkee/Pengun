using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioFadeIn
{

    static float musicVolume = 0.25f;

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = musicVolume;

        audioSource.volume = 0;


        while (audioSource.volume < musicVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = musicVolume;
    }

}
