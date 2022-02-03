using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncrease : MonoBehaviour
{
    public HealthManager healthManager;

    private void Start()
    {
        healthManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && healthManager.getArmouredHealth() != healthManager.maxHealth)
        {
            healthManager.AddToCurrentHealth();

            Destroy(this.gameObject, 0.1f);
        }

    }
}
