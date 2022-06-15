using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetize : MonoBehaviour
{

    Transform target;
    Vector3 velocity = Vector3.zero;
    public float minModifier = 7;
    public float maxModifier = 11;
    public float followTimer1 = 2;
    public float followTimer2 = 10;

    TrailRenderer trailRenderer;
    Animator animator;


    bool isFollowing = false;

    private void Start()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
        animator = this.GetComponent<Animator>();
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {

        if (!isFollowing)
        {
            StartCoroutine("StartFollowing");
        }

        if (isFollowing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, Time.deltaTime * Random.Range(minModifier, maxModifier));
        }
    }

    IEnumerator StartFollowing()
    {
        
        yield return new WaitForSeconds(Random.Range(followTimer1, followTimer2));
        trailRenderer.enabled = true;
        isFollowing = true;

    }
}
