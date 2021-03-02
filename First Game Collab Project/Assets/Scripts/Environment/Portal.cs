using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool isGameEnded = false;

    //If object collides with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the trap collides with player kill him
        if (collision.CompareTag("Player"))
        {
            EndGame();

        }
    }

    public void EndGame()
    {
        if (isGameEnded == false)
        {
            isGameEnded = true;
            Restart();
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//get active scene gets current scene.  
        ResetEnvironmentStates(); //Resets environment states like buttons etc.
    }

    private void ResetEnvironmentStates()
    {
        Button.isButtonPushed = false;
    }
}
