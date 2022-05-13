using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public float maxHealth;
    public float minHealth = 0;

    public GameObject chargerChunkParticle, chargerBloodParticle;
    public SpriteRenderer[] spriteRenderer;
    public MeshRenderer meshRenderer;
    public TimeManager timemanager;
    public Rigidbody2D rigidbody;
    public AudioSource deathSound;
    LootSplash lootSplash;

    public float currentHealth;
    private bool canBeHurt = true;
    public bool isDead = false;
    private bool isDashing;

    public float knockBackForce = 30;

    private void Start()
    {
        lootSplash = this.GetComponent<LootSplash>();
        timemanager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        deathSound = GameObject.Find("AudioManager").transform.GetChild(6).gameObject.GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        isDashing = PlayerMovement.isDashing;
    }

    public void DecreaseHealth(float damageAmount)
    {
        if (canBeHurt)
        {
            lootSplash.summonDrop();
            //timemanager.StartSlowMotion(0.1f);
            //timemanager.InvokeStopSlowMotion(0.02f);
            StartCoroutine("HurtFlashEffect");
            StartCoroutine("CanBeHurtAgain");

            if (EnemyLookAtPlayer.isFacingRight)
            {
                rigidbody.AddForce(transform.right * knockBackForce, ForceMode2D.Impulse);
            }
            else if (!EnemyLookAtPlayer.isFacingRight)
            {
                rigidbody.AddForce(transform.right * knockBackForce, ForceMode2D.Impulse);
            }


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
            DecreaseHealth(PlayerDamageController.dashDamageOutput);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            DecreaseHealth(PlayerDamageController.gunDamageOutput);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isDashing)
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
        deathSound.Play();
        isDead = true;
        lootSplash.summonDrop();
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
        CameraShake.Instance.ShakeCamera(6f, 5f, 0.2f);

        if (currentHealth >= 1)
        {

            for (int i = 0; i < 5; i++)
            {
                meshRenderer.enabled = false;
                yield return new WaitForSeconds(0.03f);
                meshRenderer.enabled = true;
                yield return new WaitForSeconds(0.03f);
            }

        }
    }
}
