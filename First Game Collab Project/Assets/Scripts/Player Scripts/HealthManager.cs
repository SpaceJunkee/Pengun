using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public KillPlayer killPlayer;
    public GameObject maxHealthImage;
    public GameObject mediumHealthImage;
    public GameObject lowHealthImage;
    public MeshRenderer meshRenderer;
    public ParticleSystem healthIncreaseParticles;

    public int maxHealth = 3;
    private int mediumHealth = 2;
    private int lowHealth = 1;
    public int minHealth = 0;
    public int currentHealth;

    public bool canBeHurt = true;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void HurtPlayer()
    {
        if (canBeHurt)
        {
            canBeHurt = false;
            DecreaseHealth();
        }        

    }

    public void CanBeHurtAgain()
    {
        canBeHurt = true;
    }

    public void DecreaseHealth()
    {

        StartCoroutine("HurtFlashEffect");

        if (currentHealth == maxHealth)
        {
            currentHealth = mediumHealth;
            maxHealthImage.SetActive(false);
        }
        else if(currentHealth == mediumHealth)
        {
            currentHealth = lowHealth;
            mediumHealthImage.SetActive(false);
        }
        else if(currentHealth == lowHealth)
        {
            currentHealth = minHealth;
            lowHealthImage.SetActive(false);
        }

        if (currentHealth == minHealth)
        {
            killPlayer.InstantiateDeath();
        }
        else
        {
            Invoke("CanBeHurtAgain", 1.5f);
        }   
    }

    public void AddToCurrentHealth()
    {
        if (currentHealth != maxHealth)
        {
            healthIncreaseParticles.Play();
            currentHealth = maxHealth;
            maxHealthImage.SetActive(true);
            mediumHealthImage.SetActive(true);
        }

    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    IEnumerator HurtFlashEffect()
    {
        CameraShake.Instance.ShakeCamera(6f, 0.2f);

        if (currentHealth >= 1)
        {
            for (int i = 0; i < 8; i++)
            {
                meshRenderer.enabled = false;
                yield return new WaitForSeconds(0.07f);
                meshRenderer.enabled = true;
                yield return new WaitForSeconds(0.07f);
            }
        }
    }

}
