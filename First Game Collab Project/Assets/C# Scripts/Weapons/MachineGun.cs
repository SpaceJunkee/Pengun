using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public Transform shootingPosition;
    public GameObject bulletPrefab;
    private PlayerMovement pm;
        
    public float coolDownTime;

    private float nextFireTime = 0;

    // Update is called once per frame
    void Update()
    {
        //Button to fire a bullet (default square on ps controller)
        if (Time.time > nextFireTime)
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
                nextFireTime = Time.time + coolDownTime;
            }
        }
           
    }

    void Shoot()
    {
        
        Instantiate(bulletPrefab, shootingPosition.position, shootingPosition.rotation);
       
    }
}
