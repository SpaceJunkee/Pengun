﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    public Transform attackPos;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int meleeDamage = 150;
    public float attackRate = 2f;
    public float meleeCooldown = 0f;

 
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= meleeCooldown)
        {
            if (Input.GetButtonDown("Melee"))
            {
                //play anim
                Attack();
                meleeCooldown = Time.time + 0.6f / attackRate;
            }
        }
        
    }

    private void Attack()
    {
        //Play Animation
        //anim.SetTrigger("Melee");
        //Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayers);
        //Apply damage
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<BreackableCrate>().TakeDamage(meleeDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPos == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
