using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageController : MonoBehaviour
{
    public static int damageOutput = 1;

    public void setDamageOutput(int damageMultiplier)
    {
        damageOutput = damageMultiplier;
    }

    public int getDamageOutput()
    {
        return damageOutput;
    }
}
