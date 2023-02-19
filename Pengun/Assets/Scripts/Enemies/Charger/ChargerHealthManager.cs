﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerHealthManager : MonoBehaviour
{
    public float maxHealth = 6;
    public float minHealth = 0;

    public GameObject chargerChunkParticle, chargerBloodParticle;
    GameObject parentObject;
    public SpriteRenderer[] spriteRenderer;
    public TimeManager timemanager;
    public Rigidbody2D rigidbody;

    public float currentHealth;
    private bool canBeHurt = true;

    private bool isDashing;

    private void Start()
    {
        currentHealth = maxHealth;
        parentObject = transform.parent.gameObject;
    }

    private void Update()
    {
        isDashing = PlayerMovement.isDashing;
    }

    public void DecreaseHealth(float damageAmount)
    {
        if (canBeHurt)
        {
            timemanager.StartSlowMotion(0.1f);
            timemanager.InvokeStopSlowMotion(0.02f);
            StartCoroutine("HurtFlashEffect");
            StartCoroutine("CanBeHurtAgain");
            rigidbody.AddForce(transform.up * 100, ForceMode2D.Impulse);

            Instantiate(chargerChunkParticle, this.gameObject.transform.position, chargerChunkParticle.transform.rotation);

            currentHealth -= damageAmount;

            if (currentHealth <= minHealth)
            {
                KillEnemy();
            }
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if ((collision.gameObject.CompareTag("Player") && isDashing))
        {
            DecreaseHealth(PlayerDamageController.dashDamageOutput);
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            DecreaseHealth(PlayerDamageController.pistolBulletDamage);
        }

        if (collision.gameObject.CompareTag("BloodWave"))
        {
            DecreaseHealth(PlayerDamageController.meleeDamageOutput *2);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {      
        if ((collision.gameObject.CompareTag("Player") && isDashing))
        {
            DecreaseHealth(PlayerDamageController.dashDamageOutput);
        }
    }

    IEnumerator CanBeHurtAgain()
    {
        canBeHurt = false;
        yield return new WaitForSeconds(0.25f);
        canBeHurt = true;
    }

    public void KillEnemy()
    {
        Destroy(parentObject);
        Instantiate(chargerChunkParticle, this.gameObject.transform.position, chargerChunkParticle.transform.rotation);
        Instantiate(chargerBloodParticle, this.gameObject.transform.position, chargerBloodParticle.transform.rotation);
    }

    IEnumerator HurtFlashEffect()
    {
        CameraShake.Instance.ShakeCamera(6f, 5f, 0.2f);

        if (currentHealth >= 1)
        {
            
            for(int x = 0; x < spriteRenderer.Length; x++)
            {
                spriteRenderer[x].enabled = false;
                yield return new WaitForSeconds(0.05f);
                spriteRenderer[x].enabled = true;
                yield return new WaitForSeconds(0.05f);
            }
            
        }
    }
}
