using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public int maxHealth;
    public int minHealth = 0;

    public GameObject chargerChunkParticle, chargerBloodParticle;
    public SpriteRenderer[] spriteRenderer;
    public TimeManager timemanager;
    public Rigidbody2D rigidbody;
    public AudioSource deathSound;

    public int currentHealth;
    private bool canBeHurt = true;
    public bool isDead = false;
    private bool isDashing;

    private void Start()
    {
        timemanager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        deathSound = GameObject.Find("AudioManager").transform.GetChild(6).gameObject.GetComponent<AudioSource>();
        if (this.gameObject.CompareTag("Dumbo"))
        {
            maxHealth = 1;
        }

        currentHealth = maxHealth;
    }

    private void Update()
    {
        isDashing = PlayerMovement.isDashing;
    }

    public void DecreaseHealth(int damageAmount)
    {
        if (canBeHurt)
        {
            timemanager.StartSlowMotion(0.1f);
            timemanager.InvokeStopSlowMotion(0.02f);
            StartCoroutine("HurtFlashEffect");
            StartCoroutine("CanBeHurtAgain");
            rigidbody.AddForce(transform.up * 100, ForceMode2D.Impulse);

            currentHealth -= damageAmount;

            if (currentHealth <= minHealth)
            {
                KillEnemy();
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BloodWave"))
        {
            KillEnemy();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BloodWave"))
        {
            KillEnemy();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && isDashing)
        {
            DecreaseHealth(PlayerDamageController.damageOutput);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isDashing)
        {
            DecreaseHealth(PlayerDamageController.damageOutput);
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
        deathSound.Play();
        isDead = true;

        if (this.CompareTag("Grombie"))
        {
            Destroy(this.gameObject, 0.05f);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Instantiate(chargerChunkParticle, this.gameObject.transform.position, chargerChunkParticle.transform.rotation);
        Instantiate(chargerBloodParticle, this.gameObject.transform.position, chargerBloodParticle.transform.rotation);
    }

    IEnumerator HurtFlashEffect()
    {
        CameraShake.Instance.ShakeCamera(6f, 0.2f);

        if (currentHealth >= 1)
        {

            for (int x = 0; x < spriteRenderer.Length; x++)
            {
                spriteRenderer[x].enabled = false;
                yield return new WaitForSeconds(0.02f);
                spriteRenderer[x].enabled = true;
                yield return new WaitForSeconds(0.02f);
            }

        }
    }
}
