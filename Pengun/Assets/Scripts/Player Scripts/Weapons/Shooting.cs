using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject pistolProjectile;
    public GameObject machineGunProjectile;
    public GameObject shotgunProjectile;
    public Transform spawnPoint;
    public Transform upSpawnPoint;
    public Transform downSpawnPoint;
    GromEnergyBarController gromEnergyBarController;
    public Animator animator;

    //Active weapon booleans
    [SerializeField]
    bool isPistolSelected, isMachineGunSelected, isShotgunSelected = false;

    //Pistol variables
    public float pistolFireRate = 0.5f;
    public float pistolProjectileSpeed = 10f;
    private float pistolNextFire = 0.0f;
    public float pistolRotationAngle;
    public float pistolPushBackForce = 1f;
    public float pistolForceAmount = 10;
    public float pistolGromEnergyCost = 1;

    [Space]
    //MachineGun Variables
    public float machineGunFireRate = 0.5f;
    public float machineGunProjectileSpeed = 10f;
    private float machineGunNextFire = 0.0f;
    public float machineGunRotationAngle;
    public float machineGunPushBackForce = 1f;
    public float machineGunForceAmount = 10;
    public float machineGunGromEnergyCost = 1;
    [Space]
    //ShotgunVariables
    public float shotgunFireRate = 0.5f;
    public float shotgunProjectileSpeed = 10f;
    private float shotgunNextFire = 0.0f;
    public float shotgunRotationAngle;
    public float shotgunPushBackForce = 1f;
    public float shotgunForceAmount = 10;
    public float shotgunGromEnergyCost = 1;

    public Vector2 upDirection = Vector2.up;
    public Vector2 downDirection = Vector2.down;
    Rigidbody2D playerRigidBody;
    PlayerMovement playerMovement;

    public float deadzone = 0.65f;
    
    public bool isShooting = false;
    public bool isShootingDown =  false;
    public bool canShoot = true;


    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerRigidBody = playerMovement.getRigidbody2D();
        gromEnergyBarController = GameObject.Find("GromEnergyBarController").GetComponent<GromEnergyBarController>();
    }

    void Update()
    {
        if (isPistolSelected && !isMachineGunSelected && !isShotgunSelected)
        {
            PistolShooting();
        }
        else if(!isPistolSelected && isMachineGunSelected && !isShotgunSelected)
        {
            MachineGunShooting();
        }
        else if(!isPistolSelected && !isMachineGunSelected && isShotgunSelected)
        {
            ShotgunShooting();
        }
       
    }

    void PistolShooting()
    {
        HandleShootingMechanics(pistolForceAmount, pistolGromEnergyCost, pistolNextFire, pistolFireRate, pistolProjectile, pistolProjectileSpeed, pistolRotationAngle);
    }

    void MachineGunShooting()
    {
        HandleShootingMechanics(machineGunForceAmount, machineGunGromEnergyCost, machineGunNextFire, machineGunFireRate, machineGunProjectile, machineGunProjectileSpeed, machineGunRotationAngle);
    }

    void ShotgunShooting()
    {
        HandleShootingMechanics(shotgunForceAmount, shotgunGromEnergyCost, shotgunNextFire, shotgunFireRate, shotgunProjectile, shotgunProjectileSpeed, shotgunRotationAngle);
    }
    bool inputType;

    void HandleShootingMechanics(float forceAmount, float gromEnergyCost, float nextFire, float fireRate, GameObject projectile, float projectileSpeed, float rotationAngle)
    {
        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 shootingForce = new Vector2(0, forceAmount);
        float inputY = Input.GetAxisRaw("Vertical");
        bool pistolShotgunInput = Input.GetButtonDown("Fire1");
        bool machineGunInput = Input.GetButton("Fire1");
        Debug.Log(inputType);

        //Fire up down left right
        if (gromEnergyBarController.currentGromEnergy >= gromEnergyCost)
        {

            if((isShotgunSelected || isPistolSelected) && !isMachineGunSelected)
            {
                inputType = pistolShotgunInput;
            }
            else if (isMachineGunSelected && !isShotgunSelected && !isPistolSelected)
            {
                inputType = machineGunInput;
            }

            if (inputY > deadzone && Time.time > nextFire && inputType && canShoot)
            {
                //ANIMATE UP SHOOTING AND CHANGE BASED ON WEAPON
                nextFire = Time.time + fireRate;
                GameObject newProjectile = Instantiate(projectile, upSpawnPoint.position, upSpawnPoint.rotation) as GameObject;
                StartCoroutine(WaitAndShoot(fireRate, gromEnergyCost));
                newProjectile.transform.localEulerAngles = new Vector3(0, 0, rotationAngle);
                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                rb.velocity = upDirection * projectileSpeed;
                isShooting = true;
                canShoot = false;
                isShootingDown = false;

                HandleUpShootingAnimations();
            }
            else if (inputDirection.y < 0 && Time.time > nextFire && inputType && (playerMovement.isJumping || playerMovement.inAirTime < 0) && canShoot)
            {
                //ANIMATE DOWN SHOOTING AND CHANGE BASED ON WEAPON
                nextFire = Time.time + fireRate;
                GameObject newProjectile = Instantiate(projectile, downSpawnPoint.position, downSpawnPoint.rotation) as GameObject;
                StartCoroutine(WaitAndShoot(fireRate, gromEnergyCost));
                newProjectile.transform.localEulerAngles = new Vector3(0, 0, -rotationAngle);
                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                rb.velocity = downDirection * projectileSpeed;

                playerRigidBody.velocity = new Vector2(rb.velocity.x, 0);
                playerRigidBody.AddForce(shootingForce, ForceMode2D.Impulse);
                isShootingDown = true;
                canShoot = false;

                HandleDownShootingAnimations();
            }
            else if (inputType && Time.time > nextFire && canShoot)
            {
                //ANIMATE STRAIGHT SHOOTING AND CHANGE BASED ON WEAPON
                nextFire = Time.time + fireRate;
                GameObject newProjectile = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
                StartCoroutine(WaitAndShoot(fireRate, gromEnergyCost));
                Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
                rb.velocity = newProjectile.transform.right * projectileSpeed;
                isShooting = true;
                canShoot = false;
                isShootingDown = false;

                HandleStraightShootingAnimations();

            }
        }
    }

    IEnumerator WaitAndShoot(float fireRate, float gromEnergyCost)
    {
        yield return new WaitForSeconds(fireRate);
        gromEnergyBarController.DecreaseGromEnergy(gromEnergyCost);
        canShoot = true;
        isShooting = false;
        isShootingDown = false;
    }

    public void SelectActiveWeapon(string weaponType)
    {

        switch (weaponType)
        {
            case "Pistol":
                // Apply pistol variables
                isPistolSelected = true;
                isMachineGunSelected = false;
                isShotgunSelected = false;
                break;
            case "MachineGun":
                // Apply machine gun variables 
                isPistolSelected = false;
                isMachineGunSelected = true;
                isShotgunSelected = false;
                break;
            case "Shotgun":
                // Apply shotgun variables
                isPistolSelected = false;
                isMachineGunSelected = false;
                isShotgunSelected = true;
                break;
        }
        //Select which weapon is active and change the variables accordingly.
        //Change int gromEnergyCost variable to match weapon. 
        //May want to change force for each weapon so a shotgun blasts you in the air where as a machine gun makes you hover.
    }

    public void SelectActiveAltFire(string altFireType)
    {
        //Change alt fire based on weapon type.

        switch (altFireType)
        {
            case "PistolAlt":
                // Apply pistol variables
                break;
            case "MachineGunAlt":
                // Apply machine gun variables 
                break;
            case "ShotgunAlt":
                // Apply shotgun variables
                break;
        }
    }

    private void HandleStraightShootingAnimations()
    {
        if (isPistolSelected && !isMachineGunSelected && !isShotgunSelected)
        {
            animator.SetTrigger("ShootPistol");
        }
        else if (!isPistolSelected && isMachineGunSelected && !isShotgunSelected)
        {
            animator.SetTrigger("ShootMachineGun");
        }
        else if (!isPistolSelected && !isMachineGunSelected && isShotgunSelected)
        {
            animator.SetTrigger("ShootShotgun");
        }
    }

    private void HandleUpShootingAnimations()
    {
        if (isPistolSelected && !isMachineGunSelected && !isShotgunSelected)
        {
            animator.SetTrigger("ShootPistolUp");
        }
        else if (!isPistolSelected && isMachineGunSelected && !isShotgunSelected)
        {
            animator.SetTrigger("ShootMachineGun");
        }
        else if (!isPistolSelected && !isMachineGunSelected && isShotgunSelected)
        {
            animator.SetTrigger("ShootShotgun");
        }
    }

    private void HandleDownShootingAnimations()
    {

    }
}
