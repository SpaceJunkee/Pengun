using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOtherObjectRemoval : MonoBehaviour
{
    public GameObject removeThisGameObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(removeThisGameObject);
        }
    }
}
