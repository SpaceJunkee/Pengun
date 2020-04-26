﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantKillPlayer : MonoBehaviour
{
    //In unity go to File, Build Settings and you can see the number that corresponds to each scene. 
    //0 is the first etc.
    //public int respawn;

    private bool isGameEnded = false;
    public float restartDelay = 0.5f;

  

    //If object collides with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If the trap collides with player kill him
        if (other.CompareTag("Player"))
        { 
            EndGame();
            
        }
    }

    public void EndGame()
    {
        if(isGameEnded == false)
        {
            isGameEnded = true;
            Invoke("Restart", restartDelay);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//get active scene gets current scene. 
    }

  
}

/*Note: It might be better to use the integer in some cases. */