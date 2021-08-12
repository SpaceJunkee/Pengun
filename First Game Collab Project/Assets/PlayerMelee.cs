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

    void Update()
    {
        if (canAttack)
        {
            if (Input.GetButtonDown("Melee"))
            {
                shotGunBlastParticles.Play();
                CameraShake.Instance.ShakeCamera(8f, 0.25f);
                shotGunBlastSound.Play();

                Attack();
            }
        }
          
    }

    void Attack()
    {
        isShooting = true;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange1, enemyLayers);
        Collider2D[] hitEnemiesPoint2 = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange2, enemyLayers);

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
            
            Debug.Log("Hit");
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

            Debug.Log("Hit2");
        }
        canAttack = false;

        StartCoroutine("CanAttackAgain");

        
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
        //yield return new WaitForSeconds(0.2f);
        canAttack = true;
        isShooting = false;
    }
}
