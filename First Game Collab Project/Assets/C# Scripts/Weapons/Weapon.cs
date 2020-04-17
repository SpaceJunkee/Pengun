using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform shootingPositionMachineGun;
    public Transform shootingPositionPistol;
    public Transform shootingPositionShotgun;
    public Transform shootingPositionChainGun;

    public GameObject machineGunBulletPrefab;
    public GameObject pistolBulletPrefab;
    public GameObject chainGunBulletPrefab;
    public GameObject[] shotgunBulletPrefab = new GameObject[6];
        
    private float coolDownTime = 0.1f;

    private float nextFireTime = 0;

    //Weapon switching
    private int currentWeapon = 1;
    

    // Update is called once per frame
    void Update()
    {
        WeaponChanger();

        switch (currentWeapon)
        {
            case 1:
                MachineGun();
                break;
            case 2:
                Pistol();
                break;
            case 3: 
                Shotgun();
                break;
            case 4: 
                ChainGun();
                break;
        }
        
           
    }

    private void Pistol()
    {
        coolDownTime = 0.5f;

        if (Time.time > nextFireTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ShootPistol();
                nextFireTime = Time.time + coolDownTime;
            }
        }
    }

    private void MachineGun()
    {
        coolDownTime = 0.1f;

        //Button to fire a bullet 
        if (Time.time > nextFireTime)
        {
            if (Input.GetButton("Fire1"))
            {
                ShootMachineGun();
                nextFireTime = Time.time + coolDownTime;
            }
        }
    }

    private void Shotgun()
    {
        coolDownTime = 0.8f;

        //Button to fire a bullet 
        if (Time.time > nextFireTime)
        {
            if (Input.GetButton("Fire1"))
            {
                ShootShotgun();
                nextFireTime = Time.time + coolDownTime;
            }
        }

    }

    private void ChainGun()
    {

        coolDownTime = 0.05f;
        

        //Button to fire a bullet 
        if (Time.time > nextFireTime)
        {
            if (Input.GetButton("Fire1"))
            {
                ShootChainGun();
                nextFireTime = Time.time + coolDownTime;
            }
        }


    }

    private void WeaponChanger()
    {
        if (Input.GetButtonDown("ChangeWeapon"))
        {
            currentWeapon++;
        }

        if(currentWeapon == 5)
        {
            currentWeapon = 1;
        }
    }

    void ShootMachineGun()
    {
        
        Instantiate(machineGunBulletPrefab, shootingPositionMachineGun.position, shootingPositionMachineGun.rotation);
       
    }

    void ShootPistol()
    {
        Instantiate(pistolBulletPrefab, shootingPositionPistol.position, shootingPositionPistol.rotation);
    }

    void ShootChainGun()
    {
        Instantiate(chainGunBulletPrefab, shootingPositionChainGun.position, shootingPositionChainGun.rotation);
    }

    void ShootShotgun()
    {
        for(int i = 0; i < shotgunBulletPrefab.Length; i++)
        {
            Instantiate(shotgunBulletPrefab[i], shootingPositionShotgun.position, shootingPositionShotgun.rotation);

        }
    }
}
