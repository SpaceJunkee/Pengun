using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoverableRoom : MonoBehaviour
{
    //Attach this script to either a breakable wall and make isDiscoverBreakable true or an invisble wall at the entrance with a trigger and set isDiscovereExplorable true
    public bool isDiscoverBreakable = false;
    public bool isDiscovereExplorable = false;
    public int hitCount = 1;
    
    public Light2D roomLight;
    bool canBeHit = true;

    private void Start()
    {
        roomLight.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isDiscovereExplorable)
        {
            roomLight.enabled = true;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //CHANGE THIS FOR DIFFERENT BULLET TYPES
        if (collision.gameObject.CompareTag("Bullet") && isDiscoverBreakable)
        {
            hitCount--;
            if (hitCount == 0)
            {
                if (roomLight != null)
                {
                    roomLight.enabled = true;
                }
                Destroy(this.gameObject);
            }
        }
    }

    //For melee only.
    public void BreakBarrierMelee()
    {   
        if(hitCount != 0 && canBeHit)
        {
            StartCoroutine("CanBeHitAgain");
        }
        else if(hitCount == 0)
        {
            if(roomLight != null)
            {
                roomLight.enabled = true;
            }           
            Destroy(this.gameObject);
        }
        
    }

    IEnumerator CanBeHitAgain()
    {
        canBeHit = false;
        hitCount--;
        yield return new WaitForSeconds(0.25f);
        canBeHit = true;
    }
}
