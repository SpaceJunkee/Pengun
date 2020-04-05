using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform shootingPosition;
    public GameObject bullerPrefab;

    // Update is called once per frame
    void Update()
    {
        //Every time fire button is pressed call shoot method.
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    //Allows weapon to fire
    void Shoot()
    {
        //Spawn bullet into world
        Instantiate(bullerPrefab, shootingPosition.position, shootingPosition.rotation);

    }
}
