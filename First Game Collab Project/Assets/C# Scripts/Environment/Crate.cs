using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField]
    protected GameObject
       crateSmashParticle,
       crateSmashDust;

    public void BreakCrate()
    {
        Instantiate(crateSmashParticle, gameObject.transform.position, crateSmashParticle.transform.rotation);
        Instantiate(crateSmashDust, gameObject.transform.position, crateSmashDust.transform.rotation);
        Destroy(gameObject);
    }
}
