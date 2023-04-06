using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutsAndBolts : MonoBehaviour
{
    GromBucks gromBucks;
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    public AudioClip[] audioClips;
    ParticleSystem lootParticles;
    GameObject player;
    bool hasbeenCollected = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        gromBucks = GameObject.Find("GromBucksController").GetComponent<GromBucks>();
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        lootParticles = player.transform.Find("LootParticles").GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasbeenCollected)
        {
            hasbeenCollected = true;
            gromBucks.AddToGromBucks(SortCurrencyValues());
            audioSource.Play();
            spriteRenderer.enabled = false;
            lootParticles.Play();
            Destroy(this.gameObject, 0.3f);
        }
    }

    int SortCurrencyValues()
    {
        if (this.CompareTag("Nut"))
        {
            return 1;
        }
        else if (this.CompareTag("Bolt"))
        {
            return 5;
        }
        else if (this.CompareTag("Screw"))
        {
            return 3;
        }
        else
        {
            return 0;
        }
    }
}
