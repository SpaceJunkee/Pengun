using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableGround : MonoBehaviour
{
    public float breakTime;
    public bool isKeyTriggerTrap = false;
    public bool hasCollectedKey = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isKeyTriggerTrap)
        {
            if (collision.gameObject.CompareTag("Player") && hasCollectedKey)
            {
                Destroy(this.gameObject, breakTime);
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Destroy(this.gameObject, breakTime);
            }
        }

    }

    public void setHasCollectedKey(bool newHasColllectedKey)
    {
        hasCollectedKey = newHasColllectedKey;
    }
}
