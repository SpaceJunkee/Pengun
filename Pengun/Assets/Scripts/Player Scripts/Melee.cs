using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public Transform attackPoint1, attackPointUp, attackPointDown, meleeParticleTransform;
    public Animator animator;
    HurtKnockBack hurtKnock;
    public ParticleSystem meleeParticleSwipe;

    //Delete when animations are in for up and down attack
    public SpriteRenderer up, down, idle;

    public float attackRange1 = 0.5f;
    public LayerMask enemyLayers;

    public float meleeCooldown;
    private float nextMeleeTime = 0;
    int meleeCount = 1;
    int meleeReset = 1;
    int maxMeleeCount = 3;
    public float upHitForce = 10;
    public float downHitForce = 10;
    public float forwardBackHitForce = 10;

    public float originalAttackTime;


    bool canAttack = true;
    public static bool isMelee = false;

    bool enemyHasBeenHit = false;

    public Collider2D[] hitEnemies;

    public float pushBackForce;
    bool isFacingRight;

    public static bool doesPlayerWantToShoot;

    bool isLookingUp;
    bool isLookingDown;

    public bool isApplyingUpforce = false;

    public float currentYVelocity;


    PlayerMovement playerMovement;
    Rigidbody2D playerRB;
    PistolAltFire pShoot;

    private void Start()
    {
        hurtKnock = this.GetComponent<HurtKnockBack>();
        playerMovement = this.GetComponent<PlayerMovement>();
        pShoot = this.GetComponent<PistolAltFire>();
        playerRB = this.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        currentYVelocity = playerMovement.getRigidbody2D().velocity.y;

        if (Input.GetAxis("Vertical") >= 0.7)
        {
            isLookingUp = true;
            isLookingDown = false;
/*            up.enabled = true;
            down.enabled = false;
            idle.enabled = false;*/
        } else if (Input.GetAxis("Vertical") <= -0.7 && !playerMovement.getIsGrounded())
        {
            isLookingUp = false;
            isLookingDown = true;
/*            up.enabled = false;
            down.enabled = true;
            idle.enabled = false;*/
        }
        else
        {
            isLookingUp = false;
            isLookingDown = false;
          /*  up.enabled = false;
            down.enabled = false;
            idle.enabled = true;*/

        }
        isFacingRight = playerMovement.getPlayerFaceRight();
        //Store input to remember if shoot was pressed while meleeing
        //Make pushback on hit?

        if (Time.time > nextMeleeTime)
        {
            canAttack = true;
        }

        if (canAttack && !PistolAltFire.isShooting && !PlayerMovement.isDashing && PlayerMovement.canMove && PlayerMovement.canUseButtonInput)
        {
            if (Input.GetButtonDown("Melee"))
            {
                CheckMeleeCount();
                //animator.SetInteger("MeleeCount", meleeCount);
                //meleeCount++;
                PerformMeleeAnimations(isLookingUp, isLookingDown);
                MeleeSwipeParticles(isLookingDown, isLookingUp);

                for (int i = 0; i < 6; i++)
                {
                    Invoke("InvokeAttack", i * 0.05f);
                }

                nextMeleeTime = Time.time + meleeCooldown;

                StartCoroutine("CanAttackAgain");

            }
        }
    }

    void PerformMeleeAnimations(bool isLookUp, bool isLookDown)
    {
        if (isLookUp)
        {
            animator.SetTrigger("MeleeUp");
        }
        else if (isLookDown)
        {
            animator.SetTrigger("MeleeDown");
        }
        else
        {
            animator.SetTrigger("Melee");
        }
    }

    void MeleeSwipeParticles(bool isLookingDown, bool isLookingUp)
    {
        if (isLookingDown)
        {
            meleeParticleTransform.localPosition = new Vector3(0, -0.5f, 0);
            meleeParticleTransform.localRotation = Quaternion.Euler(0, 180, 75);
        }
        else if (isLookingUp)
        {
            meleeParticleTransform.localPosition = new Vector3(0, 0.5f, 0);
            meleeParticleTransform.localRotation = Quaternion.Euler(0, 0, -75);
        }
        else
        {
            meleeParticleTransform.localPosition = new Vector3(0.95f, 0, 0);
            meleeParticleTransform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        meleeParticleSwipe.Play();
    }

    void InvokeAttack()
    {
        if (isLookingUp)
        {
            Attack(attackPointUp.position, false);
        }
        else if (isLookingDown && !playerMovement.getIsGrounded())
        {
            Attack(attackPointDown.position, true);
        }
        else
        {
            Attack(attackPoint1.position, false);
        }

    }

    void Attack(Vector3 attack1Pos, bool isLookingDown)
    {
        hitEnemies = Physics2D.OverlapCircleAll(attack1Pos, attackRange1, enemyLayers);
        canAttack = false;
        isMelee = true;

        bool hasEnemyBeenHit = false;
        bool hasHitDestructable = false;

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(hitEnemies.Length);
            if (enemy.GetComponent<DiscoverableRoom>() != null)
            {
                enemy.GetComponent<DiscoverableRoom>().BreakBarrierMelee();
            }
                if (enemy.GetComponent<ChargerHealthManager>() != null)
            {
                if (ChargerCanAttackZone.isInAttackZone)
                {
                    enemy.GetComponent<ChargerHealthManager>().DecreaseHealth(PlayerDamageController.meleeDamageOutput);
                }

            }
            else if (enemy.GetComponent<EnemyHealthManager>() != null)
            {
                enemy.GetComponent<EnemyHealthManager>().DecreaseHealth(PlayerDamageController.meleeDamageOutput);
            }
            else if (enemy.CompareTag("Destructable") && !hasHitDestructable)
            {
                enemy.GetComponent<BreackableObject>().TakeDamage(10);
                hasHitDestructable = true;
            }

            hasEnemyBeenHit = true;
        }

        ManageDownwardStriking(hasEnemyBeenHit);
        ManageUpwardStriking(hasEnemyBeenHit);
    }

    void ManageDownwardStriking(bool hasEnemyBeenHit)
    {
        if (isLookingDown && !hurtKnock.isCurrentlyKnockBacking && hasEnemyBeenHit && !PlayerMovement.hasJustLeftTheGroundAfterJumping && !playerMovement.getIsGrounded())
        {
            if (!isApplyingUpforce)
            {
                if (hitEnemies[0].GetComponent<EnemyHealthManager>() == null)
                {
                    if (hitEnemies[0].GetComponent<BreackableObject>().canApplyUpWardForceOnHit)
                    {
                        ApplyUpwardForce();
                    }
                }
                else if (hitEnemies[0].GetComponent<EnemyHealthManager>() != null)
                {
                    if (hitEnemies[0].GetComponent<EnemyHealthManager>().canApplyUpWardForceOnHit)
                        ApplyUpwardForce();
                }
            }
        }
    }

    void ManageUpwardStriking(bool hasEnemyBeenHit)
    {
        if (isLookingUp && !hurtKnock.isCurrentlyKnockBacking && hasEnemyBeenHit && !playerMovement.getIsGrounded())
        {
            if (hitEnemies[0].GetComponent<EnemyHealthManager>() == null)
            {
                if (hitEnemies[0].GetComponent<BreackableObject>().canApplyDownWardForceOnHit)
                {
                    ApplyDownWardForce();
                }
            }
            else if (hitEnemies[0].GetComponent<EnemyHealthManager>() != null)
            {
                if (hitEnemies[0].GetComponent<EnemyHealthManager>().canApplyDownWardForceOnHit)
                    ApplyDownWardForce();
            }
        }
    }

    //For hitting enemies above the player
    void ApplyDownWardForce()
    {
        Vector2 meleeForce = new Vector2(0, -downHitForce);
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0); // Zero out vertical velocity
        playerRB.AddForce(meleeForce, ForceMode2D.Impulse);
        StartCoroutine("ResetApplyUpForce");
    }


    //For hitting enemies below the player
    void ApplyUpwardForce()
    {
        isApplyingUpforce = true;
        Vector2 meleeForce = new Vector2(0, upHitForce);
        playerRB.velocity = new Vector2(playerRB.velocity.x, 0); // Zero out vertical velocity
        playerRB.AddForce(meleeForce, ForceMode2D.Impulse);
        StartCoroutine("ResetApplyUpForce");
    }

    IEnumerator ResetApplyUpForce()
    {
        yield return new WaitForSeconds(meleeCooldown);
        isApplyingUpforce = false;
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint1 == null)
            return;

        Gizmos.DrawWireSphere(attackPoint1.position, attackRange1);
        Gizmos.DrawWireSphere(attackPointUp.position, attackRange1);
        Gizmos.DrawWireSphere(attackPointDown.position, attackRange1);
    }

    IEnumerator CanAttackAgain()
    {
        yield return new WaitForSeconds(meleeCooldown);
        isMelee = false;
        
        if (doesPlayerWantToShoot)
        {
            StartCoroutine(pShoot.Shoot());
        }
        
        doesPlayerWantToShoot = false;
    }

    void CheckMeleeCount()
    {
        if(meleeCount > maxMeleeCount)
        {
            meleeCount = meleeReset;
        }
    }
}
