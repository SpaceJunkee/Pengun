using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullets : Bullet
{
    // Start is called before the first frame update
    void Start()
    {
        speed = 50f;
        lifeTime = 0.6f;
        bulletDamage = 100;
        moveBullet();

    }
}

