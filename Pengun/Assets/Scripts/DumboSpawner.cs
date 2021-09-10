using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumboSpawner : MonoBehaviour
{
    public static int numberOfDumbosSpawned = 0;
    int maxNumberOfDumbos = 3;
    bool isDestroyed = false;
    public GameObject dumbo;

    private void Start()
    {
        for(int i = 0; i < maxNumberOfDumbos; i++)
        {
            Invoke("SpawnDumbo", 0f);
            numberOfDumbosSpawned++;
        }
    }

    private void Update()
    {
        if(numberOfDumbosSpawned < maxNumberOfDumbos && !isDestroyed)
        {
            for (int i = numberOfDumbosSpawned; i < maxNumberOfDumbos; i++)
            {
                Invoke("SpawnDumbo", 2f);
                numberOfDumbosSpawned++;
            }
        }
    }

    void SpawnDumbo()
    {
        Instantiate(dumbo, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }

    public static void RemoveADumbo()
    {
        numberOfDumbosSpawned--;
    }
}
