using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectile;
    public Transform spawnPoint;
    public Transform upSpawnPoint;
    public Transform downSpawnPoint;
    GromEnergyBarController gromEnergyBarController;
    public float fireRate = 0.5f;
    public float projectileSpeed = 10f;
    private float nextFire = 0.0f;
    public float rotationAngle;
    public Vector2 upDirection = Vector2.up;
    public Vector2 downDirection = Vector2.down;
    Rigidbody2D playerRigidBody;
    PlayerMovement playerMovement;
    public float pushBackForce = 1f;
    public float forceAmount = 10;
    public float deadzone = 0.65f;
    
    public bool isShooting = false;
    public bool isShootingDown =  false;
    public bool canShoot = true;

    int gromEnergyCost = 1;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerRigidBody = playerMovement.getRigidbody2D();
        gromEnergyBarController = GameObject.Find("GromEnergyBarController").GetComponent<GromEnergyBarController>();
    }

    void Update()
    {
        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 shootingForce = new Vector2(0, forceAmount);
        float inputY = Input.GetAxisRaw("Vertical");

        //Fire up down left right
        if(gromEnergyBarController.currentGromEnergy >= gromEnergyCost)
        {
            if (inputY > deadzone && Time.time > nextFire && Input.GetButton("Fire1") && canShoot)
            {
                nextFire = Time.time + fireRate;
                GameObject newProjectile = Instantiate(projectile, upSpawnPoint.position, upSpawnPoint.rotation) as GameObject;
                StartCoroutine(WaitAndShoot());
                newProjectile.transform.localEulerAngles = new Vector3(0, 0, rotationAngle);
                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                rb.velocity = upDirection * projectileSpeed;
                isShooting = true;
                canShoot = false;
                isShootingDown = false;
            }
            else if (inputDirection.y < 0 && Time.time > nextFire && Input.GetButton("Fire1") && (playerMovement.isJumping || playerMovement.inAirTime < 0) && canShoot)
            {
                nextFire = Time.time + fireRate;
                GameObject newProjectile = Instantiate(projectile, downSpawnPoint.position, downSpawnPoint.rotation) as GameObject;
                StartCoroutine(WaitAndShoot());
                newProjectile.transform.localEulerAngles = new Vector3(0, 0, -rotationAngle);
                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                rb.velocity = downDirection * projectileSpeed;

                playerRigidBody.velocity = new Vector2(rb.velocity.x, 0);
                playerRigidBody.AddForce(shootingForce, ForceMode2D.Impulse);
                isShootingDown = true;
                canShoot = false;
            }
            else if (Input.GetButton("Fire1") && Time.time > nextFire && canShoot)
            {
                nextFire = Time.time + fireRate;
                GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
                StartCoroutine(WaitAndShoot());
                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                rb.velocity = newProjectile.transform.right * projectileSpeed;
                isShooting = true;
                canShoot = false;
                isShootingDown = false;
            }
        }
    }

    IEnumerator WaitAndShoot()
    {
        yield return new WaitForSeconds(fireRate);
        gromEnergyBarController.DecreaseGromEnergy(gromEnergyCost);
        canShoot = true;
        isShooting = false;
        isShootingDown = false;
    }

    public void GetActiveWeapon()
    {
        //Select which weapon is active and change the variables accordingly.
        //Change int gromEnergyCost variable to match weapon. 
        //May want to change force for each weapon so a shotgun blasts you in the air where as a machine gun makes you hover.
    }
}
