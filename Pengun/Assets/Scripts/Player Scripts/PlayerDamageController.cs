using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageController : MonoBehaviour
{
    public static float meleeDamageOutput = 1f;
    public static float gunDamageOutput = 2f;
    public static float dashDamageOutput = 2f;


    public void setMeleeDamageOutput(float damageMultiplier)
    {
        meleeDamageOutput = damageMultiplier;
    }

    public void setGunDamageOutput(float damageMultiplier)
    {
        gunDamageOutput = damageMultiplier;
    }

    public void setDashDamageOutput(float damageMultiplier)
    {
        dashDamageOutput = damageMultiplier;
    }

    public float getMeleeDamageOutput()
    {
        return meleeDamageOutput;
    }

    public float getGunDamageOutput()
    {
        return gunDamageOutput;
    }

    public float getDashDamageOutput()
    {
        return dashDamageOutput;
    }
}
