using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Melee : MonoBehaviour
{
    public Transform attackPoint1, attackPointUp, attackPointDown;
    public Animator animator;
    HurtKnockBack hurtKnock;

    //Delete when animations are in for up and down attack
    public SpriteRenderer up, down, idle;

    public float attackRange1 = 0.5f;
    public LayerMask enemyLayers;

    public float meleeCooldown;
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

    bool isApplyingUpforce = false;

    public float currentYVelocity;


    PlayerMovement playerMovement;
    P_Shoot pShoot;

    private void Start()
    {
        hurtKnock = this.GetComponent<HurtKnockBack>();
        playerMovement = this.GetComponent<PlayerMovement>();
        pShoot = this.GetComponent<P_Shoot>();
    }
    void Update()
    {
        currentYVelocity = playerMovement.getRigidbody2D().velocity.y;

        if (Input.GetAxis("Vertical") >= 0.75)
        {
            isLookingUp = true;
            isLookingDown = false;
/*            up.enabled = true;
            down.enabled = false;
            idle.enabled = false;*/
        } else if (Input.GetAxis("Vertical") <= -0.75 && !playerMovement.getIsGrounded())
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

        if (canAttack && !P_Shoot.isShooting && !PlayerMovement.isDashing && PlayerMovement.canMove && PlayerMovement.canUseButtonInput)
        {
            if (Input.GetButtonDown("Melee"))
            {
                CheckMeleeCount();
                animator.SetInteger("MeleeCount", meleeCount);
                meleeCount++;
                animator.SetTrigger("Melee");

                for (int i = 0; i < 5; i++)
                {
                    Invoke("InvokeAttack", i * 0.05f);
                }

                StartCoroutine("CanAttackAgain");

            }
        }
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
        if (currentYVelocity > 0 && playerMovement.isJumping)
        {
            isApplyingUpforce = true;
            playerMovement.getRigidbody2D().AddForce(Vector2.down * (downHitForce), ForceMode2D.Impulse);
        }
        else
        {
            isApplyingUpforce = true;
            playerMovement.getRigidbody2D().AddForce(Vector2.down * (downHitForce), ForceMode2D.Impulse);
        }

        StartCoroutine("ResetApplyUpForce");
    }


    //For hitting enemies below the player
    void ApplyUpwardForce()
    {
        if (currentYVelocity < 0 && !playerMovement.isJumping)
        {
            isApplyingUpforce = true;         
            playerMovement.getRigidbody2D().AddForce(Vector2.up * (upHitForce - currentYVelocity), ForceMode2D.Impulse);
        }
        else
        {
            isApplyingUpforce = true;
            playerMovement.getRigidbody2D().AddForce(Vector2.up * (upHitForce - (currentYVelocity * 2f)), ForceMode2D.Impulse);
        }

        StartCoroutine("ResetApplyUpForce");
    }

    IEnumerator ResetApplyUpForce()
    {
        yield return new WaitForSeconds(0.5f);
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
        canAttack = true;
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
