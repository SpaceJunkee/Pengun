using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Melee : MonoBehaviour
{
    public Transform attackPoint1;
    public Animator animator;

    public float attackRange1 = 0.5f;
    public LayerMask enemyLayers;

    public float meleeCooldown;
    int meleeCount = 1;
    int meleeReset = 1;
    int maxMeleeCount = 3;

    public float originalAttackTime;


    bool canAttack = true;
    public static bool isMelee = false;

    bool enemyHasBeenHit = false;

    public Collider2D[] hitEnemies;

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
                
                for(int i = 0; i < 5; i++)
                {
                    Invoke("InvokeAttack", i * 0.05f);
                }
       
                StartCoroutine("CanAttackAgain");

            }
        }
    }

    void InvokeAttack()
    {
        Attack(attackPoint1.position);
    }

    void Attack(Vector3 attack1Pos)
    {
        hitEnemies = Physics2D.OverlapCircleAll(attack1Pos, attackRange1, enemyLayers);
        canAttack = false;
        isMelee = true;

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

        Debug.Log(attack1Pos);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint1 == null)
            return;

        Gizmos.DrawWireSphere(attackPoint1.position, attackRange1);
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
