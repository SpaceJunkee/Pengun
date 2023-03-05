using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyUnlock : MonoBehaviour
{
    KeyHolder playerKeyHolder;
    public GameObject[] objectsToUnlock;
    public float keyTimeToKill = 0.2f;

    private void Start()
    {
        playerKeyHolder = GameObject.FindGameObjectWithTag("Player").GetComponent<KeyHolder>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerKeyHolder.getIsHoldingKey())
            {
                Destroy(playerKeyHolder.getKeyGameObject(), keyTimeToKill);
                Unlock();
            }
        }
    }

    public void Unlock()
    {
        foreach(GameObject gameObject in objectsToUnlock)
        {
            if (gameObject.CompareTag("JumpPad"))
            {
                gameObject.GetComponent<CircleCollider2D>().enabled = true;
            }
        }
    }
}
