using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioFadeIn
{
    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 1f;

        audioSource.volume = 0;


        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }

}
