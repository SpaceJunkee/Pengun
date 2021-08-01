using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerHealthManager : MonoBehaviour
{
    public int maxHealth = 3;
    public int minHealth = 0;

    public GameObject chargerChunkParticle, chargerBloodParticle;
    public SpriteRenderer[] spriteRenderer;
    public TimeManager timemanager;
    public Rigidbody2D rigidbody;

    public int currentHealth;
    private bool canBeHurt = true;

    private bool isDashing;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        isDashing = PlayerMovement.isDashing;
    }

    public void DecreaseHealth()
    {
        if (canBeHurt)
        {
            timemanager.StartSlowMotion(0.1f);
            timemanager.InvokeStopSlowMotion(0.02f);
            StartCoroutine("HurtFlashEffect");
            StartCoroutine("CanBeHurtAgain");
            rigidbody.AddForce(transform.up * 300, ForceMode2D.Impulse);

            currentHealth--;

            if (currentHealth == minHealth)
            {
                KillEnemy();
            }
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Player") && isDashing)
        {
            DecreaseHealth();
        }
       
    }

    private void OnTriggerStay2D(Collider2D collision)
    {      
        if (collision.gameObject.CompareTag("Player") && isDashing)
        {
            DecreaseHealth();
        }          
    }

    IEnumerator CanBeHurtAgain()
    {
        canBeHurt = false;
        yield return new WaitForSeconds(1f);
        canBeHurt = true;
    }

    public void KillEnemy()
    {
        Destroy(GameObject.Find("ChargerEnemy"));
        Instantiate(chargerChunkParticle, this.gameObject.transform.position, chargerChunkParticle.transform.rotation);
        Instantiate(chargerBloodParticle, this.gameObject.transform.position, chargerBloodParticle.transform.rotation);
    }

    IEnumerator HurtFlashEffect()
    {
        CameraShake.Instance.ShakeCamera(6f, 0.2f);

        if (currentHealth >= 1)
        {
            
            for(int x = 0; x < spriteRenderer.Length; x++)
            {
                spriteRenderer[x].enabled = false;
                yield return new WaitForSeconds(0.02f);
                spriteRenderer[x].enabled = true;
                yield return new WaitForSeconds(0.02f);
            }
            
        }
    }
}
