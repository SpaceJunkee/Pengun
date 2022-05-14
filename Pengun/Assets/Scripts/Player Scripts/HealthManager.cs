using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public KillPlayer killPlayer;

    //Upgrade health here
    public Image maxHealthImage;
    public Image mediumHealthImage;
    public Image lowHealthImage;
    public Image minHealthImage;
    public MeshRenderer meshRenderer;
    public ParticleSystem healthIncreaseParticles;
    public ParticleSystem hitParticles;

    public float maxHealth = 100f;
    private float mediumHealth = 75;
    private float lowHealth = 50;
    private float minHealth = 25;
    public float noHealth = 0;
    public float currentHealth;

    public bool canBeHurt = true;
    bool hasArmour = true;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {

        CheckIfCurrentHealthIsMax();

        /*if (Input.GetButtonDown("Melee"))
        {
            DecreaseHealth(3);
        }

        if (Input.GetButtonDown("Dash"))
        {
            IncreaseHealth(5);
        }*/

    }

    public void HurtPlayer(float damageAmount)
    {
        if (canBeHurt)
        {
            PerformHitEffect();
            canBeHurt = false;
            DecreaseHealth(damageAmount);
        }        

    }

    public void CanBeHurtAgain()
    {
        canBeHurt = true;
    }

    public void DecreaseHealth(float decreaseAmount)
    {
        currentHealth -= decreaseAmount;
        UpdateHealthBars();
         

        if (currentHealth <= noHealth)
        {
            killPlayer.InstantiateDeath();
        }
        else
        {
            Invoke("CanBeHurtAgain", 0.5f);
        }   
    }

    public void IncreaseHealth(float inceaseAmount)
    {
        UpdateHealthBars();
        currentHealth += inceaseAmount;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public void PerformHitEffect()
    {
        hitParticles.Play();
    }

    IEnumerator HurtFlashEffect()
    {
        CameraShake.Instance.ShakeCamera(6f, 5f, 0.2f);

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

    void CheckIfCurrentHealthIsMax()
    {
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth <= noHealth)
        {
            currentHealth = noHealth;
        }
    }

    void UpdateHealthBars()
    {
        if (currentHealth > mediumHealth && currentHealth <= maxHealth)
        {
            maxHealthImage.fillAmount = currentHealth / maxHealth;
        }
        else if (currentHealth > lowHealth && currentHealth <= mediumHealth)
        {
            maxHealthImage.fillAmount = 0;
            mediumHealthImage.fillAmount = currentHealth / maxHealth;
        }
        else if (currentHealth > minHealth && currentHealth <= lowHealth)
        {
            mediumHealthImage.fillAmount = 0;
            lowHealthImage.fillAmount = currentHealth / maxHealth;
        }
        else if (currentHealth > noHealth && currentHealth <= minHealth)
        {
            lowHealthImage.fillAmount = 0;
            minHealthImage.fillAmount = currentHealth / maxHealth;
        }
    }

}
