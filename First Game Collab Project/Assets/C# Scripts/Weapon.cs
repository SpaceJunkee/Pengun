using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform shootingPosition;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        //Button to fire a bullet (default square on ps controller)
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, shootingPosition.position, shootingPosition.rotation);
    }
}
