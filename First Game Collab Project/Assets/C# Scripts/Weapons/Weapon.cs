using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform shootingPositionMachineGun;
    public Transform shootingPositionPistol;
    public GameObject MachineGunBulletPrefab;
    public GameObject PistolBulletPrefab;
        
    private float coolDownTime = 0.1f;

    private float nextFireTime = 0;

    //Weapon switching
    private int currentWeapon = 1;
    

    // Update is called once per frame
    void Update()
    {
        WeaponChanger();


        if (currentWeapon == 1)
        {
            machineGun();
        }else if(currentWeapon == 2)
        {
            pistol();
        }
           
    }

    private void pistol()
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

    private void machineGun()
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

    private void WeaponChanger()
    {
        if (Input.GetButtonDown("ChangeWeapon"))
        {
            currentWeapon++;
        }

        if(currentWeapon == 3)
        {
            currentWeapon = 1;
        }
    }

    void ShootMachineGun()
    {
        
        Instantiate(MachineGunBulletPrefab, shootingPositionMachineGun.position, shootingPositionMachineGun.rotation);
       
    }

    void ShootPistol()
    {
        Instantiate(PistolBulletPrefab, shootingPositionPistol.position, shootingPositionPistol.rotation);
    }
}
