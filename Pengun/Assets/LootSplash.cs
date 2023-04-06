using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSplash : MonoBehaviour
{
    public GameObject droplet;
    public GameObject[] nutsAndBolts;

    int gromDropCount;
    public int nutDropCount;
    public int boltDropCount;
    public int screwDropCount;

    public int minDropCount;
    public int maxDropCount;
    int originalDropCount;
    public float spread = 2.5f;
    public bool containsGrom = false;
    public bool containsLoot = false;

    private void Start()
    {
        originalDropCount = Random.Range(minDropCount, maxDropCount);
        gromDropCount = originalDropCount;
        
    }

    public void SummonGromAndLoot()
    {
        while(gromDropCount > 0 && containsGrom)
        {
            Vector3 pos = transform.position;
            gromDropCount -= 1;
            pos.x += spread * Random.value - spread / 2;
            pos.y += spread * Random.value - spread / 2;
            GameObject go = Instantiate(droplet, pos, Quaternion.identity);
            go.transform.position = pos;
        }

        if (containsLoot)
        {
            SummonLoot(nutDropCount, boltDropCount, screwDropCount);
        }
        
        ResetDropCount();
    }


    int nutDropLoopCount = 0;
    int boltDropLoopCount = 0;
    int screwDropLoopCount = 0;

    public int numberOfhits = 1;

    public float minForce = 25;
    public float maxForce = 35;
    public float upwardStartPos = 6;

    void SummonLoot(int nutDrop, int boltDrop, int screwDrop)
    {
        Vector3 pos = transform.position;
        pos.x += spread * Random.value - spread / 2;
        pos.y += spread * Random.value - spread / 2;

        foreach (GameObject loot in nutsAndBolts)
        {
            if (loot.CompareTag("Nut") && nutDropLoopCount < numberOfhits)
            {
                for (int i = 0; i < nutDrop; i++)
                {
                    GameObject go = Instantiate(loot, pos, Quaternion.identity);
                    go.transform.position = pos;
                    Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                    Vector2 upwardForce = new Vector2(Random.Range(-5f, 5f), upwardStartPos).normalized * Random.Range(minForce, maxForce);
                    rb.AddForce(upwardForce, ForceMode2D.Impulse);
                }

                nutDropLoopCount++;
            }
            else if (loot.CompareTag("Bolt") && boltDropLoopCount < numberOfhits)
            {
                for (int i = 0; i < boltDrop; i++)
                {
                    GameObject go = Instantiate(loot, pos, Quaternion.identity);
                    go.transform.position = pos;
                    Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                    Vector2 upwardForce = new Vector2(Random.Range(-5f, 5f), upwardStartPos).normalized * Random.Range(minForce, maxForce);
                    rb.AddForce(upwardForce, ForceMode2D.Impulse);
                }

                boltDropLoopCount++;
            }
            else if (loot.CompareTag("Screw") && screwDropLoopCount < numberOfhits)
            {
                for (int i = 0; i < screwDrop; i++)
                {
                    GameObject go = Instantiate(loot, pos, Quaternion.identity);
                    go.transform.position = pos;
                    Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
                    Vector2 upwardForce = new Vector2(Random.Range(-5f, 5f), upwardStartPos).normalized * Random.Range(minForce, maxForce);
                    rb.AddForce(upwardForce, ForceMode2D.Impulse);
                }

                screwDropLoopCount++;
            }


        }

    }

    void ResetDropCount()
    {
        originalDropCount = Random.Range(minDropCount, maxDropCount);
        gromDropCount = originalDropCount;
    }
}
