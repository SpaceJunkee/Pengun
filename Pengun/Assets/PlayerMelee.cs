using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public ParticleSystem shotGunBlastParticles;
    public ParticleSystem readyToShootParticles;
    public AudioSource shotGunBlastSound;
    public AudioSource shotGunReadySound;
    public Transform attackPoint1;
    public Transform attackPoint2;

    Vector3 secondAttackPointPos;
    Vector3 secondAttackPoint2Pos;
    float secondAttackOffset = 2.5f;
    Vector3[] secondAttackPositionsArray;

    public float attackRange1 = 0.5f;
    public float attackRange2 = 0.5f;
    public LayerMask enemyLayers;

    bool canAttack = true;
    public static bool isShooting = false;

    bool enemyHasBeenHit = false;

    public Collider2D[] hitEnemies;
    public Collider2D[] hitEnemiesPoint2;



    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = this.GetComponent<PlayerMovement>();
    }
    void Update()
    {
        if (canAttack && !PlayerMovement.isDashing && PlayerMovement.canMove && PlayerMovement.canUseInput)
        {
            if (Input.GetButtonDown("Melee"))
            {
                shotGunBlastParticles.Play();
                CameraShake.Instance.ShakeCamera(8f, 5f, 0.25f);
                shotGunBlastSound.Play();

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
        isShooting = true;
        hitEnemies = Physics2D.OverlapCircleAll(attack1Pos, attackRange1, enemyLayers);
        hitEnemiesPoint2 = Physics2D.OverlapCircleAll(attack2Pos, attackRange2, enemyLayers);

        SetSecondAttackPositions();

        canAttack = false;

        //Set return true in foreach if wanting to only hit one enemy.
        bool hasEnemyBeenHit = false;

        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.GetComponent<ChargerHealthManager>() != null)
            {
                if(ChargerCanAttackZone.isInAttackZone)
                enemy.GetComponent<ChargerHealthManager>().DecreaseHealth(PlayerDamageController.damageOutput);
            }
            else
            {
                    enemy.GetComponent<EnemyHealthManager>().DecreaseHealth(PlayerDamageController.damageOutput);
            }
            hasEnemyBeenHit = true;
        }

        foreach (Collider2D enemy in hitEnemiesPoint2)
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
        yield return new WaitForSeconds(0.75f);
        shotGunReadySound.Play();
        readyToShootParticles.Play();
        yield return new WaitForSeconds(0.2f);
        canAttack = true;
        isShooting = false;
    }
}
