using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBullets : Bullet
{
    // Start is called before the first frame update
    void Start()
    {
        speed = 50f;
        lifeTime = 0.5f;
        bulletDamage = 30;
        moveBullet();
       
    }
}
