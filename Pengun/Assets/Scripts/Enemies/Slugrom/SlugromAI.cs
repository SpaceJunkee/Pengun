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
    bool currentlyAttacking;
    public Transform playerTarget;
    public Animator animator;
    MoveEnemyOnPoints slugMovement;

    private void Start()
    {
        slugMovement = GetComponent<MoveEnemyOnPoints>();
        rigidbody = GetComponent<Rigidbody2D>();
        originalConstraints = rigidbody.constraints;
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!currentlyAttacking && collision.tag == "Player")
        {
            AttackLeap();
            currentlyAttacking = true;
            animator.SetTrigger("AttackLeap");
        }
    }

    void AttackLeap()
    {
        Vector2 dir = (transform.position - playerTarget.position).normalized;
        rigidbody.AddForce(-dir * leapForceForward, ForceMode2D.Impulse);
        rigidbody.AddForce(transform.up * leapForce, ForceMode2D.Impulse);
        Invoke("ResetAttack", 1f);
    }
    void ResetAttack()
    {
        currentlyAttacking = false;
    }

}
