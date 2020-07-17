using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class BreackableCrate : MonoBehaviour
{

    [SerializeField]
    protected GameObject
        crateChunkParticle,
        crateDustParticle;

    public int health = 50;


    //Enemy to take damage
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            BreakCrate();
        }
    }

    private void BreakCrate()
    {
        Instantiate(crateChunkParticle, gameObject.transform.position, crateChunkParticle.transform.rotation);
        Instantiate(crateDustParticle, gameObject.transform.position, crateDustParticle.transform.rotation);
        Destroy(gameObject);
    }

}
