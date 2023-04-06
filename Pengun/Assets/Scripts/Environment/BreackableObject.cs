using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class BreackableObject : MonoBehaviour
{

    [SerializeField]
    protected GameObject breakingChunkParticle;
    LootSplash lootSplash;

    public int health = 10;
    public bool isBreakable = true;
    bool canBeHurt = true;

    //Handle force applied when player melee attacks them
    public bool canApplyDownWardForceOnHit;
    public bool canApplyUpWardForceOnHit;

    public AudioClip[] breakableClips; 
    AudioSource audioSource;
    Collider2D collider;
    SpriteRenderer spriteRenderer;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        lootSplash = GetComponent<LootSplash>();

        if (this.gameObject.layer == 14)//Crate
        {           
            audioSource.clip = breakableClips[Random.Range(0, breakableClips.Length)];
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBreakable)
        {
            if (collision.gameObject.CompareTag("DashColliderChecker") && PlayerMovement.isDashing ||
                collision.gameObject.CompareTag("Player") && PlayerMovement.isLongFall ||
                collision.gameObject.CompareTag("PistolBullet") ||
                collision.gameObject.CompareTag("MachineGunBullet") ||
                collision.gameObject.CompareTag("ShotgunBullet")
                )
            {
                if (collision.gameObject.CompareTag("DashColliderChecker")){
                    Debug.Log("DashCol");
                }
                CameraShake.Instance.ShakeCamera(4f, 5f, 0.2f);
                BreakObject();
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isBreakable)
        {
            if (collision.gameObject.CompareTag("DashColliderChecker") && PlayerMovement.isDashing)
            {
                if (collision.gameObject.CompareTag("DashColliderChecker"))
                {
                    Debug.Log("DashCol");
                }
                CameraShake.Instance.ShakeCamera(4f, 5f, 0.2f);
                BreakObject();
            }
        }
       
    }

    //On Trigger Objects here


    //Enemy to take damage
    public void TakeDamage(int damage)
    {
        if (canBeHurt)
        {
            StartCoroutine("CanBeHurtAgain");
            CameraShake.Instance.ShakeCamera(4f, 5f, 0.2f);
            health -= damage;

            if (health <= 0)
            {
                BreakObject();
            }

            audioSource.clip = breakableClips[Random.Range(0, breakableClips.Length)];
            audioSource.Play();
            lootSplash.SummonGromAndLoot();
        }
        
    }

    IEnumerator CanBeHurtAgain()
    {
        canBeHurt = false;
        yield return new WaitForSeconds(0.25f);
        canBeHurt = true;
    }

    public void BreakObject()
    {
        Instantiate(breakingChunkParticle, gameObject.transform.position, breakingChunkParticle.transform.rotation);

        lootSplash.SummonGromAndLoot();
        audioSource.Play();
        collider.enabled = false;
        spriteRenderer.enabled = false;
        Destroy(gameObject, 1f);
    }

}
