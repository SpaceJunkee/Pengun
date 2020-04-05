using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript1 : MonoBehaviour
{
    public int health = 100;
  

    //Enemy to take damage
    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            die();
        }
    }

    void die()
    {
        Destroy(gameObject);
    }
}
