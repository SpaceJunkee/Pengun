using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GromDroplet : MonoBehaviour
{
    GromEnergyBarController gromEnergyController;
    GromBucks gromBucks;

    private void Start()
    {
        gromEnergyController = GameObject.Find("GromEnergyBarController").GetComponent<GromEnergyBarController>();
        gromBucks = GameObject.Find("GromBucksController").GetComponent<GromBucks>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gromEnergyController.IncreaseGromEnergy(Random.Range(3, 6));
            gromBucks.AddToGromBucks(Random.Range(1, 10));
            Destroy(this.gameObject);
        }
    }
}
