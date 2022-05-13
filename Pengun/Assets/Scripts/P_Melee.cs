using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Melee : MonoBehaviour
{
    public Transform attackPoint1;
    public Transform attackPoint2;
    public Animator animator;

    Vector3 secondAttackPointPos;
    Vector3 secondAttackPoint2Pos;
    float secondAttackOffset = 2.5f;
    Vector3[] secondAttackPositionsArray;

    public float attackRange1 = 0.5f;
    public float attackRange2 = 0.5f;
    public LayerMask enemyLayers;

    public float meleeCooldown;
    int meleeCount = 1;
    int meleeReset = 1;
    int maxMeleeCount = 3;


    bool canAttack = true;
    public static bool isMelee = false;

    bool enemyHasBeenHit = false;

    public Collider2D[] hitEnemies;
    public Collider2D[] hitEnemiesPoint2;

    public float pushBackForce;
    bool isFacingRight;

    public static bool doesPlayerWantToShoot;


    PlayerMovement playerMovement;
    P_Shoot pShoot;

    private void Start()
    {
        playerMovement = this.GetComponent<PlayerMovement>();
        pShoot = this.GetComponent<P_Shoot>();
    }
    void Update()
    {

      //Reset melee combo timer

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

                //If attack does not hit an enemy, fire a second attack to give some leeway.
                if (!Attack(attackPoint1.position, attackPoint2.position))
                {
                    StartCoroutine("SecondAttack", secondAttackPositionsArray);
                    Debug.Log("Second");
                }            
                
                StartCoroutine("CanAttackAgain");

            }
        }
    }



     bool Attack(Vector3 attack1Pos, Vector3 attack2Pos)
    {
        isMelee = true;
        hitEnemies = Physics2D.OverlapCircleAll(attack1Pos, attackRange1, enemyLayers);
        hitEnemiesPoint2 = Physics2D.OverlapCircleAll(attack2Pos, attackRange2, enemyLayers);

        SetSecondAttackPositions();

        canAttack = false;

        //Set return true in foreach if wanting to only hit one enemy.
        bool hasEnemyBeenHit = false;
        bool hasHitDestructable = false;

        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.GetComponent<ChargerHealthManager>() != null)
            {
                if (ChargerCanAttackZone.isInAttackZone)
                {
                    enemy.GetComponent<ChargerHealthManager>().DecreaseHealth(PlayerDamageController.meleeDamageOutput);
                }
                
            }
            else if(enemy.GetComponent<EnemyHealthManager>() != null)
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

        foreach (Collider2D enemy in hitEnemiesPoint2)
        {
            if (enemy.GetComponent<ChargerHealthManager>() != null)
            {
                if (ChargerCanAttackZone.isInAttackZone)
                {
                    enemy.GetComponent<ChargerHealthManager>().DecreaseHealth(PlayerDamageController.meleeDamageOutput);
                }


            }
            else if(enemy.GetComponent<EnemyHealthManager>() != null)
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


        
        return hasEnemyBeenHit;
        
    }

    void SetSecondAttackPositions()
    {

        secondAttackPositionsArray = new Vector3[2];

        if (playerMovement.getPlayerFaceRight())
        {
            secondAttackPointPos = new Vector3(attackPoint1.position.x + secondAttackOffset, attackPoint1.position.y, attackPoint1.position.z);
            secondAttackPoint2Pos = new Vector3(attackPoint2.position.x + secondAttackOffset, attackPoint2.position.y, attackPoint2.position.z);
        }
        else if (!playerMovement.getPlayerFaceRight())
        {
            secondAttackPointPos = new Vector3(attackPoint1.position.x + -secondAttackOffset, attackPoint1.position.y, attackPoint1.position.z);
            secondAttackPoint2Pos = new Vector3(attackPoint2.position.x + -secondAttackOffset, attackPoint2.position.y, attackPoint2.position.z);
        }

        secondAttackPositionsArray[0] = secondAttackPointPos;
        secondAttackPositionsArray[1] = secondAttackPoint2Pos;
    }

    IEnumerator SecondAttack(Vector3[] attack1and2)
    {
        yield return new WaitForSeconds(0.25f);

        Attack(attack1and2[0], attack1and2[1]);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint1 == null)
            return;

        Gizmos.DrawWireSphere(attackPoint1.position, attackRange1);
        Gizmos.DrawWireSphere(attackPoint2.position, attackRange2);
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
