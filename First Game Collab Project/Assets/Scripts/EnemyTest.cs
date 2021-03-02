using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public GameObject projectile;

    public float fireRate;
    public float nextFireTime;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fireRate = 1f;
        nextFireTime = Time.time;
    }

    private void Update()
    {
        CheckIfTimeToFire();
    }

    private void CheckIfTimeToFire()
    {
        if (Time.time > nextFireTime)
        {
            if (player != null)
            {
                Invoke("SpawnProjectile", 2f);

            }

            nextFireTime = Time.time + fireRate;
        }
    }

    private void SpawnProjectile()
    {
        Instantiate(projectile, transform.position, Quaternion.identity);

    }



}

