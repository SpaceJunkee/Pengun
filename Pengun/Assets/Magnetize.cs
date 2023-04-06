using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetize : MonoBehaviour
{

    Transform target;
    Vector3 velocity = Vector3.zero;
    public float minModifier = 7;
    public float maxModifier = 11;
    public float followTimer1 = 1;
    public float followTimer2 = 5;

    TrailRenderer trailRenderer;
    Animator animator;


    bool isFollowing = false;
    public bool canMagnetize = false;

    private void Start()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
        animator = this.GetComponent<Animator>();
        target = GameObject.Find("Player").GetComponent<Transform>();
        InvokeRepeating("ReduceMaxMod", 0f, 0.25f);

        if (!canMagnetize)
        {
            trailRenderer.enabled = false;
            animator.enabled = false;
        }
    }

    private void FixedUpdate()
    {

        if (!isFollowing && canMagnetize)
        {
            StartCoroutine("StartFollowing");
        }

        if (isFollowing && canMagnetize)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, Time.deltaTime * Random.Range(minModifier, maxModifier));
        }
    }

    void ReduceMaxMod()
    {
        maxModifier -= 1f;
    }

    IEnumerator StartFollowing()
    {
        
        yield return new WaitForSeconds(Random.Range(followTimer1, followTimer2));
        trailRenderer.enabled = true;
        isFollowing = true;

    }
}
