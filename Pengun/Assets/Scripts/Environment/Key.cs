using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Transform followTransform;  // the transform of the player to follow
    KeyHolder playerKeyHolder;
    public float followDistance = 1f;  // distance to follow the player from
    public float followSpeed = 5f;     // speed to follow the player with
    public float hoverOffset = 0.2f;
    private bool isFollowingPlayer = false; // updated to start false
    public float hoverAmplitude = 0.1f;   // amplitude of the vertical bobbing
    public float hoverFrequency = 1f;  // frequency of the vertical bobbing

    private void Start()
    {
        followTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerKeyHolder = GameObject.FindGameObjectWithTag("Player").GetComponent<KeyHolder>();
    }

    void FixedUpdate()
    {
        if (isFollowingPlayer)
        {
            // calculate the position to follow the player from, with a vertical offset and bobbing
            float bobbingOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
            // calculate the position to follow the player from
            Vector3 followPos = followTransform.position + -followTransform.right * followDistance + Vector3.up * (hoverOffset + bobbingOffset);
            // smoothly move towards the follow position
            transform.position = Vector3.Lerp(transform.position, followPos, followSpeed * Time.deltaTime);
            // ensure the key is always in front of the player
            transform.position = new Vector3(transform.position.x, transform.position.y, followTransform.position.z - 0.1f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerKeyHolder.setIsHoldingKey(true);
            playerKeyHolder.setKeyGameObject(this.gameObject);
            // enable following the player when the key is picked up
            isFollowingPlayer = true;
            // disable the key's collider so it doesn't collide with the player
            GetComponent<Collider2D>().enabled = false;
        }

    }
}
