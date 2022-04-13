using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Shoot : MonoBehaviour
{
    public Animator animator;

    bool canAttack = true;
    public static bool isShooting = false;
    public bool isPushingBack = false;

    public float shootSpeed, shootCoolDown, shootAgainCoolDown;
    public Transform shootPos;
    public GameObject bullet;
    public PlayerMovement playerMovement;
    public float pushBackForce;
    Rigidbody2D playerRigidBody;
    bool isFacingRight;
    bool canShootAgain = true;

    private void Start()
    {
        isShooting = false;
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerRigidBody = playerMovement.getRigidbody2D();
    }

    void ShootKnockBack()
    {
        if (!isFacingRight)
        {
            playerRigidBody.AddForce(Vector2.right * pushBackForce, ForceMode2D.Impulse);
        }
        else
        {
            playerRigidBody.AddForce(Vector2.left * pushBackForce, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Shoot") && !isShooting && P_Melee.isMelee && canShootAgain)
        {
            P_Melee.doesPlayerWantToShoot = true;
        }

        if (Input.GetButtonDown("Shoot") && !isShooting && !P_Melee.isMelee && !PlayerMovement.isDashing && canShootAgain)
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
                isFacingRight = false;
                ShootKnockBack();
                return -1;
            }
            else
            {
                isFacingRight = true;
                ShootKnockBack();
                return +1;
            }
        }
        animator.SetTrigger("Shoot");
        playerMovement.StopPlayer(true, false, true);
        isShooting = true;
        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * direction() * Time.fixedDeltaTime, 0f);
        newBullet.transform.localScale = new Vector2(newBullet.transform.localScale.x * direction(), newBullet.transform.localScale.y);

        
        CameraShake.Instance.ShakeCamera(2f, 2f, 0.3f);

        yield return new WaitForSeconds(shootCoolDown);

        isShooting = false;
        P_Melee.doesPlayerWantToShoot = false;
        playerMovement.EnableMovement();
        playerMovement.StopPlayer(true, false, true);
        canShootAgain = false;
        StartCoroutine(CanShootAgain());
    }

    IEnumerator CanShootAgain()
    {
        yield return new WaitForSeconds(shootAgainCoolDown);
        canShootAgain = true;
        playerMovement.EnableMovement();
    }

}
