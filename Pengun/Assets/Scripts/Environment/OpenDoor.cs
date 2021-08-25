using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    GameObject door;
    GameObject enemy;

    private void Start()
    {
        door = GameObject.Find("Door");
        enemy = GameObject.Find("MadMan");
    }

    private void Update()
    {
        if (enemy == null)
        {
            OpenDoorOnEnemyDeath();
        }
    }

    private void OpenDoorOnEnemyDeath()
    {
        door.transform.Translate(Vector3.up * 10f, Space.World);
    }
}
