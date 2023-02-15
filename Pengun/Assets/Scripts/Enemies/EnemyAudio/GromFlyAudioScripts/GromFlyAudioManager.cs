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
        }
    }
}
