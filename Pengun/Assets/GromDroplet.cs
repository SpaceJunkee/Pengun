using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GromDroplet : MonoBehaviour
{
    GromEnergyBarController gromEnergyController;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    public AudioClip[] audioClips;
    ParticleSystem gromBackPackParticles;
    GameObject player;
    bool hasbeenCollected = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        gromEnergyController = GameObject.Find("GromEnergyBarController").GetComponent<GromEnergyBarController>();
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        gromBackPackParticles = player.transform.Find("GromBackPackParticles").GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasbeenCollected)
        {
            hasbeenCollected = true;
            gromEnergyController.IncreaseGromEnergy(Random.Range(3, 6));
            audioSource.Play();
            spriteRenderer.enabled = false;
            gromBackPackParticles.Play();
            Destroy(this.gameObject, 0.3f);
        }
    }
}
