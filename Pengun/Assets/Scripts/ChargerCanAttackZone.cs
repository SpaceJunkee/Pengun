using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerCanAttackZone : MonoBehaviour
{ 
    public static bool isInAttackZone = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInAttackZone = true;
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInAttackZone = false;
        }
    }
}
