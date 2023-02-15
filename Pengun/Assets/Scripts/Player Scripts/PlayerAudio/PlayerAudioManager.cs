using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    GameObject playerAudioManager;
    public AudioSource[] playerAudioSources;
    /*Audio sources array values
     0 = Dash
     1 = Jump
     2 = SprintJump
     3 = Landed
     4 = Melee
    */

    void Start()
    {
        playerAudioManager = GameObject.Find("PlayerAudioManager");
    }

    public void PlayAudioSource(string audioSourceArrayType)
    {
        switch (audioSourceArrayType)
        {
            case "Dash":
                playerAudioSources[0].Play();
                break;
            case "Jump":
                playerAudioSources[1].Play();
                break;
            case "SprintJump":
                playerAudioSources[2].Play();
                break;
            case "Landed":
                playerAudioSources[3].Play();
                break;
            case "Melee":
                playerAudioSources[4].Play();
                break;
        }
    }

}
