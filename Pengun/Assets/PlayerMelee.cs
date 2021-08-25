using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public ParticleSystem shotGunBlastParticles;
    public ParticleSystem readyToShootParticles;
    public AudioSource shotGunBlastSound;
    public AudioSource shotGunReadySound;
    public Transform attackPoint;
    public Transform attackPoint2;
    public float attackRange1 = 0.5f;
    public float attackRange2 = 0.5f;
    public LayerMask enemyLayers;

    bool canAttack = true;
    public static bool isShooting = false;

    bool enemyHasBeenHit = false;

    void Update()
    {
        if (canAttack && !PlayerMovement.isDashing)
        {
            if (Input.GetButtonDown("Melee"))
            {
                shotGunBlastParticles.Play();
                CameraShake.Instance.ShakeCamera(8f, 0.25f);
                shotGunBlastSound.Play();

                //If attack does not hit an enemy, fire a second attack to give some leeway.
                if (!Attack())
                {
                    Invoke("Attack", 0.25f);
                    Debug.Log("SecondAttack");
                }

                StartCoroutine("CanAttackAgain");

            }
        }
          
    }

     bool Attack()
    {
        isShooting = true;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange1, enemyLayers);
        Collider2D[] hitEnemiesPoint2 = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange2, enemyLayers);

        canAttack = false;

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
            return true;
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
            return true;
        }
        
        return false;
        
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange1);
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
