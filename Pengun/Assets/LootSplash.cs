using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSplash : MonoBehaviour
{
    public GameObject droplet;
    int dropCount;
    public int minDropCount;
    public int maxDropCount;
    int originalDropCount;
    public float spread = 2.5f;
    public bool containsLoot = false;

    private void Start()
    {
        originalDropCount = Random.Range(minDropCount, maxDropCount);
        dropCount = originalDropCount;

        if (!containsLoot)
        {
            droplet = null;
        }
    }

    public void summonDrop()
    {
        while(dropCount > 0)
        {
            Vector3 pos = transform.position;
            dropCount -= 1;
            pos.x += spread * Random.value - spread / 2;
            pos.y += spread * Random.value - spread / 2;
            GameObject go = Instantiate(droplet, this.gameObject.transform.position, Quaternion.identity);
            go.transform.position = pos;
        }

        ResetDropCount();
    }

    void ResetDropCount()
    {
        originalDropCount = Random.Range(minDropCount, maxDropCount);
        dropCount = originalDropCount;
    }
}
