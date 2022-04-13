using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyObjects : MonoBehaviour
{
    public List<GameObject> onDestroyObjectsList;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        foreach (GameObject spawnedObject in onDestroyObjectsList)
        {
            if (spawnedObject.layer == 22)
            {
                Destroy(spawnedObject);
            }
        }
    }

    private void Start()
    {
        
    }

}
