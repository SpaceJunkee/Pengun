using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBullets : MonoBehaviour
{

    public float speed = 20f;
    public float lifeTimeOfBullet;
    public Rigidbody2D rigidBody;
    public int bulletDamage = 40;

    // Start is called before the first frame update
    void Start()
    {
        moveBullet();
       
    }
    void Update()
    {
        lifeTimeOfBullet -= Time.deltaTime;
        if (lifeTimeOfBullet <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemy = GameObject.FindWithTag("Enemy");
        GameObject enemyFound = collision.gameObject;
        BreackableCrate crate = collision.GetComponent<BreackableCrate>();

        Debug.Log(collision.gameObject.name + " : " + gameObject.name + " : " + Time.time);

        if (collision.gameObject.name == "Basic")
        {
            enemyFound.GetComponentInParent<BasicEnemy>().TakeDamage(bulletDamage);
        }

        if (crate != null)
        {
            crate.TakeDamage(bulletDamage);
        }

        destroyBullet();
    }

    void moveBullet()
    {
        //Use right instead of forward as we dont want to use z axis in 2d game
        rigidBody.transform.Rotate(0, 0, Random.Range(-2.5f, 2.5f));
        rigidBody.velocity = transform.right * speed;

    }

    void destroyBullet()
    {
        Destroy(gameObject);
    }
}
