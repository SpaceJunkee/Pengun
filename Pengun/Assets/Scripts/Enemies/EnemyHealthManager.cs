using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    //EnemyAudioReferences Add to PlayEnemyHurtAudio() method for different enemies
    GromFlyAudioManager gromFlyAudioManager;

    /*MAKE SURE TO ADD ALT FIRE DAMAGES FOR THIS TOO - CHECK PLAYERDAMAGECONTROLLER SCRIPT*/
    public float maxHealth;
    public float minHealth = 0;
    public float currentHealth;
    public GameObject chargerChunkParticle, chargerBloodParticle, hitChunckParticle, gromDroplet;
    public GameObject enemyHurtAudioObject;
    public SpriteRenderer[] spriteRenderer;
    MeshRenderer meshRenderer;
    public TimeManager timemanager;
    public Rigidbody2D rigidbody;
    public float canBeHurtAgainTimer = 0.25f;

    //Every enemy needs this
    LootSplash lootSplash;

    private bool canBeHurt = true;
    public bool isDead = false;
    private bool isDashing;

    public float knockBackForce = 30;
    public float upknockBackForce = 30;

    //Handle force applied when player melee attacks them
    public bool canApplyDownWardForceOnHit;
    public bool canApplyUpWardForceOnHit;

    public bool isMovePointEnemy = false;

    public EnemyGroundCheck enemyGroundCheck;

    Rigidbody2D player;

    private void Start()
    {
        gromFlyAudioManager = this.GetComponent<GromFlyAudioManager>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        lootSplash = this.GetComponent<LootSplash>();
        timemanager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        isDashing = PlayerMovement.isDashing;
    }

    public void DecreaseHealth(float damageAmount, bool isMeleeHit)
    {
        if (canBeHurt)
        {
            StartCoroutine("CanBeHurtAgain");
            CameraShake.Instance.ShakeCamera(4f, 1f, 0.15f);
            //Summon hit chunck
            Instantiate(hitChunckParticle, this.gameObject.transform.position, hitChunckParticle.transform.rotation);
            Instantiate(gromDroplet, this.gameObject.transform.position, gromDroplet.transform.rotation);

            if (isMeleeHit)
            {
                PlayEnemyHurtAudio(this.gameObject.name, true);
            }
            else
            {
                PlayEnemyHurtAudio(this.gameObject.name, false);
            }
            

            if (!isMovePointEnemy)
            {
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
            }
            else
            {
                this.GetComponent<MoveEnemyOnPoints>().isMoving = false;

                if (enemyGroundCheck.isGrounded)
                {
                    rigidbody.AddForce(transform.up * upknockBackForce, ForceMode2D.Impulse);
                }
                
                StartCoroutine("MovePointEnemyAfterHit");
            }

            currentHealth -= damageAmount;

            if (currentHealth <= minHealth)
            {
                KillEnemy();
            }
        }

    }

    IEnumerator MovePointEnemyAfterHit()
    {
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<MoveEnemyOnPoints>().isMoving = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        string bulletType = collision.gameObject.tag;

        switch (bulletType)
        {
            case "PistolBullet":
                // Apply pistol variables
                DecreaseHealth(PlayerDamageController.pistolBulletDamage, false);
                break;
            case "MachineGunBullet":
                // Apply MG variables
                DecreaseHealth(PlayerDamageController.machineGunBulletDamage, false);
                break;
            case "ShotgunBullet":
                // Apply Shotgun variables
                DecreaseHealth(PlayerDamageController.shotgunBulletDamage, false);
                break;

        }

    }

    IEnumerator CanBeHurtAgain()
    {
        canBeHurt = false;
        yield return new WaitForSeconds(canBeHurtAgainTimer);
        canBeHurt = true;
    }

    public void KillEnemy()
    {
        isDead = true;
        lootSplash.summonDrop();

        // Disable the collider and art components or particles
        DisableAllColliders(this.gameObject);
        DisableParticleSystemsRecursively(transform);
        if (meshRenderer != null) meshRenderer.enabled = false;

        // Call any additional death methods here

        // Delay the destruction of the enemy object
        StartCoroutine(DestroyEnemyWithDelay(1f));
    }

    IEnumerator DestroyEnemyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destroy the enemy object
        Destroy(gameObject);
    }

    public void SetKnockBackForce(float knockBack)
    {
        knockBackForce = knockBack;
    }

    //Disable any particle systems attached to object. 
    void DisableParticleSystemsRecursively(Transform transform)
    {
        foreach (Transform childTransform in transform)
        {
            // Recursively disable particle systems in child objects
            DisableParticleSystemsRecursively(childTransform);

            // Disable any ParticleSystem components in this object
            ParticleSystem particleSystem = childTransform.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
                particleSystem.Clear();
                particleSystem.gameObject.SetActive(false);
            }
        }
    }

    void DisableAllColliders(GameObject gameObject)
    {
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            if(collider != null)
            {
                collider.enabled = false;
            }           
        }
    }

    void PlayEnemyHurtAudio(string enemyName, bool playMeleeHitSound)
    {
        if (playMeleeHitSound)
        {
            switch (enemyName)
            {
                case "GromFly":
                    gromFlyAudioManager.PlayAudioSource("HitMelee");
                    break;
            }
        }
        else
        {
            switch (enemyName)
            {
                case "GromFly":
                    gromFlyAudioManager.PlayAudioSource("Hit");
                    break;
            }
        }

    }

}
  