using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Melee : MonoBehaviour
{
    //public AudioSource shotGunBlastSound;
    //public AudioSource shotGunReadySound;
    public Transform meleeAttackPoint;
    public Animator animator;

    Vector3[] secondAttackPositionsArray;

    public float attackRange1 = 0.5f;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayers;

    bool canAttack = true;
    public static bool isMelee = false;


    public Collider2D[] hitEnemies;



    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = this.GetComponent<PlayerMovement>();
    }
    void Update()
    {
        if (canAttack && !P_Shoot.isShooting && !PlayerMovement.isDashing && PlayerMovement.canMove && PlayerMovement.canUseInput)
        {
            if (Input.GetButtonDown("Melee"))
            {
                CameraShake.Instance.ShakeCamera(1f, 10f, 0.3f);
                //shotGunBlastSound.Play();

                animator.SetTrigger("Melee");

                //If attack does not hit an enemy, fire a second attack to give some leeway.
                if (!Attack(meleeAttackPoint.position))
                {
                    StartCoroutine("SecondAttack", secondAttackPositionsArray);
                    Debug.Log("Second");
                }



                StartCoroutine("CanAttackAgain");

            }
        }
    }



    bool Attack(Vector3 attack1Pos)
    {
        isMelee = true;
        hitEnemies = Physics2D.OverlapCircleAll(attack1Pos, attackRange1, enemyLayers);

        canAttack = false;

        //Set return true in foreach if wanting to only hit one enemy.
        bool hasEnemyBeenHit = false;

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<ChargerHealthManager>() != null)
            {
                if (ChargerCanAttackZone.isInAttackZone)
                    enemy.GetComponent<ChargerHealthManager>().DecreaseHealth(PlayerDamageController.damageOutput);
            }
            else
            {
                enemy.GetComponent<EnemyHealthManager>().DecreaseHealth(PlayerDamageController.damageOutput);
            }
            hasEnemyBeenHit = true;
        }

        return hasEnemyBeenHit;

    }


    private void OnDrawGizmosSelected()
    {
        if (meleeAttackPoint == null)
            return;

        Gizmos.DrawWireSphere(meleeAttackPoint.position, attackRange1);
    }

    IEnumerator CanAttackAgain()
    {
        yield return new WaitForSeconds(attackCooldown);
        //shotGunReadySound.Play();
        //readyToShootParticles.Play();
        canAttack = true;
        isMelee = false;
    }
}
