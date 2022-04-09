using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFlipper : MonoBehaviour
{
    public float originalDragAmount;
    public float jumpingDragAmount;
    public AreaEffector2D areaEffector2D;
    public ParticleSystem particleSystem;
    public bool isGravActive;
    public bool isGravTimed;
    private bool hasActivatedTimed = false;
    public float gravTimer = 2f;
    public Animator animator;

    private void Update()
    {
        if (isGravTimed && !isGravActive && !hasActivatedTimed)
        {
            hasActivatedTimed = true;
            ActivateTimedGrav();
        }
        
        if (isGravActive && !isGravTimed)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            particleSystem.Play();
        }
        else if(!isGravActive && !isGravTimed)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            particleSystem.Stop();
        }
    }

    void ActivateTimedGrav()
    {
        StartCoroutine("StartAndStopGrav");
    }

    IEnumerator StartAndStopGrav()
    {
        StartGrav();
        yield return new WaitForSeconds(gravTimer);
        StopGrav();
        yield return new WaitForSeconds(gravTimer);
        StartCoroutine("StartAndStopGrav");
    }

    void StartGrav()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        particleSystem.Play();
    }

    void StopGrav()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        particleSystem.Stop();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetButton("Jump") || Input.GetButtonDown("Jump"))
            {
                areaEffector2D.drag = jumpingDragAmount;
            }
            else
            {
                areaEffector2D.drag = originalDragAmount;
            }

            //Grav lift animation goes here
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            areaEffector2D.drag = originalDragAmount;
        }

    }
}
