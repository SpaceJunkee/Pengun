using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLaserCollider : MonoBehaviour
{
    HealthManager healthManager;

    private void Start()
    {
        healthManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Hit");
            healthManager.DecreaseHealth(1);
        }
    }
}
