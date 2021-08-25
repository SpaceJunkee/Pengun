using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteTapes : MonoBehaviour
{
    public AudioSource[] audioSources;

    public AudioClip[] cassetteTapes;

    public AudioSource tapeSwitchSound;

    private void Start()
    {
        SwapTape(0);
    }

    public void ChangeToBaseTrackLeft()
    {
        Debug.Log("InLeft");
        SwapTape(0);
    }

    public void ChangeToTrackRight()
    {
        Debug.Log("InRight");
        SwapTape(1);
    }

    public void ChangeToTrackUp()
    {
        Debug.Log("InUp");
        SwapTape(2);
    }

    public void ChangeToTrackDown()
    {
        Debug.Log("InDown");
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

        PlayTapeChangeSound();

        audioSources[tapeNumber].mute = false;
    }

    void PlayTapeChangeSound()
    {
        tapeSwitchSound.Play();
        //PauseTapes();
        //Invoke("UnPauseTapes", tapeSwitchSound.clip.length - 0.4f);
    }

    void PauseTapes()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Pause();
        }
    }

    void UnPauseTapes()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].UnPause();
        }
    }

   

}
