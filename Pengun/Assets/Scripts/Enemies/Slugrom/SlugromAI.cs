using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SlugromAI : MonoBehaviour
{
    public float attackRange;
    Rigidbody2D rigidbody;
    RigidbodyConstraints2D originalConstraints;
    public float leapForce = 1f;
    public float leapForceForward = 5f;
    public float freezeTime = 0.7f;
    bool currentlyAttacking;
    public Transform playerTarget;
    public Animator animator;
    MoveEnemyOnPoints slugMovement;
    public EnemyGroundCheck slugGroundCheck;
    public BoxCollider2D slugPlayerDetector;
    bool hasFlippedA = false;
    bool hasFlippedB = true;
    bool isMoving = true;
    public bool isTouchingTarget = false;
    CapsuleCollider2D capCollider;
    public float capColliderLength;
    public float capColliderHeight;
    Vector3 originalColliderSize;
    public ParticleSystem slugStink, slugStinkJump, slugStinkJumpSlime;

    private void Start()
    {
        capCollider = GetComponent<CapsuleCollider2D>();
        slugMovement = GetComponent<MoveEnemyOnPoints>();
        rigidbody = GetComponent<Rigidbody2D>();
        originalConstraints = rigidbody.constraints;
        originalColliderSize = capCollider.size;
        playerTarget = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        animator.SetBool("isGrounded", slugGroundCheck.isGrounded);
        animator.SetBool("isMoving", isMoving);
        FlipAnimation();
    }

    void FlipAnimation()
    {
        if (slugMovement.hasReachedTargetA && !hasFlippedA)
        {
            animator.SetTrigger("FlipRight");
            StartCoroutine("FreezeMovement");
            StartCoroutine("FlipColliderAfterTurn");
            hasFlippedA = true;
            hasFlippedB = false;
        }
        else if(slugMovement.hasReachedTargetB && !hasFlippedB)
        {
            animator.SetTrigger("FlipLeft");
            StartCoroutine("FreezeMovement");
            StartCoroutine("FlipColliderAfterTurn");
            hasFlippedA = false;
            hasFlippedB = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!currentlyAttacking && collision.tag == "Player" && slugGroundCheck.isGrounded && !isTouchingTarget)
        {
            AttackLeap();
            animator.SetTrigger("AttackLeap");
            currentlyAttacking = true;
        }
    }

    IEnumerator FreezeMovement()
    {
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        isMoving = false;
        capCollider.size = new Vector2(capColliderLength, capColliderHeight); ;
        yield return new WaitForSeconds(freezeTime);
        capCollider.size = originalColliderSize;
        isMoving = true;
        slugMovement.FlipMeshRenderer();     
        rigidbody.constraints = originalConstraints;
    }

    IEnumerator FlipColliderAfterTurn()
    {     
        yield return new WaitForSeconds(freezeTime+ 0.4f);
        FlipSlugCollider(hasFlippedA, hasFlippedB);
    }

    void FlipSlugCollider(bool left, bool right)
    {
        if (!left)
        {
            slugPlayerDetector.transform.localPosition = new Vector2(-8.9f, 0);
        }
        else if(!right)
        {
            slugPlayerDetector.transform.localPosition = new Vector2(0f, 0f);
        }
        
    }

    void AttackLeap()
    {
        slugStink.Pause();
        slugStinkJump.Play();
        slugStinkJumpSlime.Play();
        Vector2 dir = (transform.position - playerTarget.position).normalized;
        rigidbody.AddForce(-dir * leapForceForward, ForceMode2D.Impulse);
        rigidbody.AddForce(transform.up * leapForce, ForceMode2D.Impulse);
        Invoke("ResetAttack", 1f);
    }
    void ResetAttack()
    {
        slugStink.Play();
        currentlyAttacking = false;
    }

}
