using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageController : MonoBehaviour
{
    /*MAKE SURE TO ADD ALT FIRE DAMAGES FOR THIS TOO - CHECK ENEMYHEALTHMANAGER SCRIPT*/

    public static float meleeDamageOutput = 1f;
    public static float dashDamageOutput = 2f;

    //Bullet damage
    public static float pistolBulletDamage = 1f;
    public static float machineGunBulletDamage = 0.5f;
    public static float shotgunBulletDamage = 5f;


    public void setMeleeDamageOutput(float damageMultiplier)
    {
        meleeDamageOutput = damageMultiplier;
    }

    public void setPistolBulletDamageOutput(float damageMultiplier)
    {
        pistolBulletDamage = damageMultiplier;
    }
    public void setMachineGunBulletDamageOutput(float damageMultiplier)
    {
        machineGunBulletDamage = damageMultiplier;
    }
    public void setShotgunBulletDamageOutput(float damageMultiplier)
    {
        shotgunBulletDamage = damageMultiplier;
    }

    public void setDashDamageOutput(float damageMultiplier)
    {
        dashDamageOutput = damageMultiplier;
    }

    public float getMeleeDamageOutput()
    {
        return meleeDamageOutput;
    }

    public float getPistolBulletDamageOutput()
    {
        return pistolBulletDamage;
    }

    public float getMachineGunBulletDamageOutput()
    {
        return machineGunBulletDamage;
    }

    public float getShotgunBulletDamageOutput()
    {
        return shotgunBulletDamage;
    }

    public float getDashDamageOutput()
    {
        return dashDamageOutput;
    }
}
