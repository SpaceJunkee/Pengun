using System.Collections;
using UnityEngine;

public class ChargerAI : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D rigidbody;
    public bool isFlipped = false;
    Vector2 targetPosition;
    float chargeForce;
    public Animator animator;
    bool isCharging = true;
    bool isFacingRight = false;

    public LayerMask groundLayers;
    public Transform groundCheck;
    bool isTouchingGround = true;

    RaycastHit2D hit;
    Vector2 spawnPosition;

    private void Start()
    {
        spawnPosition = new Vector2(rigidbody.position.x, rigidbody.position.y);
    }

    private void Update()
    {
        hit = Physics2D.Raycast(groundCheck.position, -transform.up, 1f, groundLayers);
    }

    private void FixedUpdate()
    {
        if(hit.collider == false)
        {
            isTouchingGround = false;
        }
        else
        {
            isTouchingGround = true;
        }
    }

    public void LookAtSpawnPosition()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > spawnPosition.x && isFlipped)
        {
            isFacingRight = false;
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < spawnPosition.x && !isFlipped)
        {
            isFacingRight = true;
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if(transform.position.x > player.position.x && isFlipped)
        {
            isFacingRight = false;
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            isFacingRight = true;
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void ChargeDelay(Vector2 targetPos, float chargingForce)
    {
        targetPosition = targetPos;
        chargeForce = chargingForce;
        StartCoroutine("DelayCharge");
    }

    IEnumerator DelayCharge()
    {
        yield return new WaitForSeconds(1f);

        if (!isFlipped)
        {
            rigidbody.AddForce(-targetPosition * chargeForce, ForceMode2D.Impulse);
        }
        else if (isFlipped)
        {
            rigidbody.AddForce(targetPosition * chargeForce, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(2f);
        animator.SetBool("IsFinishedCharging", true);
    }

    public void StopDelayCharge()
    {
        StopCoroutine("DelayCharge");
    }

    public bool GetIsFlipped()
    {
        return isFlipped;
    }

    public bool GetIsTouchingGround()
    {
        return isTouchingGround;
    }

    public Vector2 GetSpawnPosition()
    {
        return spawnPosition;
    }

}
