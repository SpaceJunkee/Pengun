﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncrease : MonoBehaviour
{
    public HealthManager healthManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            healthManager.AddToCurrentHealth();

            Destroy(this.gameObject, 0.1f);
        }

    }
}
