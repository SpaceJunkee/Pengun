﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{

    private bool isGameEnded = false;
    public float restartDelay = 1f;
    public GameObject playerChunkParticle, playerBloodParticle;
    public PlayerMovement playerMovement;
    public MeshRenderer meshRenderer;
    public TimeManager timeManager;
    public static bool isDead = false;

    private void Awake()
    {
        isDead = false;
    }

    public void InstantiateDeath()
    {
        isDead = true;
        timeManager.StopSlowMotion();
        playerMovement.StopPlayer(true, true, true);
        Destroy(GameObject.Find("SPINEPENG"));
        Instantiate(playerChunkParticle, this.gameObject.transform.position, playerChunkParticle.transform.rotation);
        Instantiate(playerBloodParticle, this.gameObject.transform.position, playerBloodParticle.transform.rotation);
        Invoke("EndGame", 0.3f);
    }

    public void EndGame()
    {
        if (isGameEnded == false)
        {
            isGameEnded = true;
            Invoke("Restart", restartDelay);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//get active scene gets current scene.  
       // ResetEnvironmentStates(); //Resets environment states like buttons etc. ONLY USE IF NEEDING TO RESET ENVIRONMENT STATES ETC.
        
    }

    private void ResetEnvironmentStates()
    {
       // Button.isButtonPushed = false;
    }


}
