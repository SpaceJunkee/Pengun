using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteTapes : MonoBehaviour
{
    public AudioSource[] audioSources;

    public AudioClip[] cassetteTapes;

    private void Start()
    {
        SwapTape(0);
    }

    public void ChangeToBaseTrackLeft()
    {
        SwapTape(0);
    }

    public void ChangeToTrackRight()
    {
        SwapTape(1);
    }

    public void ChangeToTrackUp()
    {
        SwapTape(2);
    }

    public void ChangeToTrackDown()
    {
        SwapTape(3);
    }

    void SwapTape(int tapeNumber)
    {
        /*audioSources.clip = cassetteTapes[tapeNumber];
        audioSources.Play();*/

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].mute = true;
        }

        audioSources[tapeNumber].mute = false;
    }
}
