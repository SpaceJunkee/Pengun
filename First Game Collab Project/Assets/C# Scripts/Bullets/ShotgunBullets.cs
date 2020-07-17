using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullets : Bullet
{
    // Start is called before the first frame update
    void Start()
    {
        speed = 65f;
        lifeTime = 0.25f;
        bulletDamage = 40;
        moveBullet();

    }
}

