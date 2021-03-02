using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject gate;
    

    private void Start()
    {
        gate = GameObject.Find("GateClose");
        rb = GetComponent<Rigidbody2D>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            gate.transform.Translate(Vector3.down * 14f, Space.World);
            Invoke("DropPlatform", 0.7f);
            Destroy(gameObject, 2.45f);
        }
    }

    private void DropPlatform()
    {
        rb.isKinematic = false;
    }
}
