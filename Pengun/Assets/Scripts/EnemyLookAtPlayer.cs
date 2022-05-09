using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{

    public static bool isFacingRight = false;
    bool isFlipped = false;
    public Transform player;
    EnemyPathfinding enemyPathfinding;

    private void Start()
    {
        enemyPathfinding = this.GetComponent < EnemyPathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyPathfinding.playerIsInRange)
        {
            LookAtPlayer();
        }

    }
    
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
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
}
