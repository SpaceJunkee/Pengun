using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GromFlyAudioManager : MonoBehaviour
{
    GameObject gromFlyAudioManager;
    public AudioSource[] gromFlyAudioSources;
    /*Audio sources array values
     0 = Alert
     1 = Chase
     2 = Death
     3 = Hit
     4 = HitMelee
    */

    void Start()
    {
        gromFlyAudioManager = transform.Find("GromFlyAudioManager").gameObject;
        for (int i = 0; i < gromFlyAudioSources.Length; i++)
        {
            gromFlyAudioSources[i] = gromFlyAudioManager.transform.GetChild(i).GetComponent<AudioSource>();
        }
    }

    public void PlayAudioSource(string audioSourceArrayType)
    {
        switch (audioSourceArrayType)
        {
            case "Alert":
                gromFlyAudioSources[0].Play();
                break;
            case "Chase":
                gromFlyAudioSources[1].Play();
                break;
            case "Death":
                gromFlyAudioSources[2].Play();
                break;
            case "Hit":
                gromFlyAudioSources[3].Play();
                break;
            case "HitMelee":
                gromFlyAudioSources[4].Play();
                break;
        }
    }

    public void StopAudioSource(string audioSourceArrayType)
    {
        switch (audioSourceArrayType)
        {
            case "Alert":
                gromFlyAudioSources[0].Stop();
                break;
            case "Chase":
                gromFlyAudioSources[1].Stop();
                break;
            case "Death":
                gromFlyAudioSources[2].Stop();
                break;
            case "Hit":
                gromFlyAudioSources[3].Stop();
                break;
            case "HitMelee":
                gromFlyAudioSources[4].Stop();
                break;
        }
    }
}
