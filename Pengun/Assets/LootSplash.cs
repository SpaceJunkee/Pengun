using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSplash : MonoBehaviour
{
    public GameObject droplet;
    public int dropCount;
    float spread = 2.5f;

    private void OnDestroy()
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
    }
}
