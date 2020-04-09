using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform shootingPosition;
    public GameObject bulletPrefab;

    public float coolDownTime;

    private float nextFireTime = 0;

    // Update is called once per frame
    void Update()
    {
        //Button to fire a bullet (default square on ps controller)
        if (Time.time > nextFireTime)
        {
            if (Input.GetButtonDown("Fire1"))
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
