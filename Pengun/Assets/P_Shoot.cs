using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Shoot : MonoBehaviour
{
    public Animator animator;

    bool canAttack = true;
    public static bool isShooting = false;

    public float shootSpeed, shootTimer;
    public Transform shootPos;
    public GameObject bullet;
    public PlayerMovement playerMovement;
    public float pushBackForce;

    private void Start()
    {
        isShooting = false;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Shoot") && !isShooting && P_Melee.isMelee)
        {
            P_Melee.doesPlayerWantToShoot = true;
        }

        if (Input.GetButtonDown("Shoot") && !isShooting && !P_Melee.isMelee)
        {
            StartCoroutine(Shoot());
        }
    }

    public IEnumerator Shoot()
    {
        int direction()
        {
            if(!playerMovement.getPlayerFaceRight())
            {
                playerMovement.getRigidbody2D().transform.Translate(transform.right * pushBackForce);
                return -1;
            }
            else
            {
                playerMovement.getRigidbody2D().transform.Translate(-transform.right * pushBackForce);
                return +1;
            }
        }

        playerMovement.StopPlayer();

        isShooting = true;
        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * direction() * Time.fixedDeltaTime, 0f);
        newBullet.transform.localScale = new Vector2(newBullet.transform.localScale.x * direction(), newBullet.transform.localScale.y);

        animator.SetTrigger("Shoot");
        
        CameraShake.Instance.ShakeCamera(2f, 2f, 0.3f);
        yield return new WaitForSeconds(shootTimer);
        isShooting = false;
        P_Melee.doesPlayerWantToShoot = false;
        playerMovement.EnableMovement();
    }


}
