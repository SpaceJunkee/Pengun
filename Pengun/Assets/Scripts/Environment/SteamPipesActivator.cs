﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamPipesActivator : MonoBehaviour
{

    public ParticleSystem particleSystem;
    public int damageAmount = 15;

    private void Start()
    {
        InvokeRepeating("ActivateSteam", 4f, 4f);
        InvokeRepeating("StopStream", 8f, 8f);
    }

    private void ActivateSteam()
    {
        particleSystem.Play();
    }

    private void StopStream()
    {
        particleSystem.Stop();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<HealthManager>().HurtPlayer(damageAmount);
        }
    }

}
