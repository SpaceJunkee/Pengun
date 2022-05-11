using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GromDroplet : MonoBehaviour
{
    GromEnergyBarController gromEnergyController;

    private void Start()
    {
        gromEnergyController = GameObject.Find("GromEnergyBarController").GetComponent<GromEnergyBarController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gromEnergyController.IncreaseGromEnergy(Random.Range(3, 6));
            Destroy(this.gameObject);
        }
    }
}
