using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteTapes : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] cassetteTapes;

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
        audioSource.clip = cassetteTapes[tapeNumber];
        audioSource.Play();
    }
}
