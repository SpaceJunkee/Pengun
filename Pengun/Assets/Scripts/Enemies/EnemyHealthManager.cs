using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public float maxHealth;
    public float minHealth = 0;

    public GameObject chargerChunkParticle, chargerBloodParticle, hitChunckParticle, gromDroplet;
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
    public float upknockBackForce = 30;

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
            StartCoroutine("CanBeHurtAgain");
            CameraShake.Instance.ShakeCamera(4f, 1f, 0.15f);
            //Summon hit chunck
            Instantiate(hitChunckParticle, this.gameObject.transform.position, hitChunckParticle.transform.rotation);
            Instantiate(gromDroplet, this.gameObject.transform.position, gromDroplet.transform.rotation);

            if (EnemyLookAtPlayer.isFacingRight)
            {
                rigidbody.AddForce(transform.right * knockBackForce, ForceMode2D.Impulse);
                rigidbody.AddForce(transform.up * upknockBackForce, ForceMode2D.Impulse);
            }
            else if (!EnemyLookAtPlayer.isFacingRight)
            {
                rigidbody.AddForce(transform.right * knockBackForce, ForceMode2D.Impulse);
                rigidbody.AddForce(transform.up * upknockBackForce, ForceMode2D.Impulse);
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

}
  