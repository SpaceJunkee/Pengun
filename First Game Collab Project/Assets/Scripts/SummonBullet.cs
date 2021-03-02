using UnityEngine;

public class SummonBullet : MonoBehaviour
{

    public float moveSpeed = 50f;

    public Rigidbody2D rigidbody;

    public PlayerMovement player;
    Vector2 movementDirection;
    public TimeManager timeManager;

    

    private void Start()
    {
        if (player != null)
        {
            movementDirection = (player.transform.position - transform.position).normalized * moveSpeed;
        }

        rigidbody.velocity = new Vector2(movementDirection.x, movementDirection.y);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("Parry"))
        {
            CameraShake.Instance.ShakeCamera(4f, 0.04f);
            timeManager.StartSlowMotion(0.1f);
            timeManager.Invoke("StopSlowMotion", 0.025f);
            Destroy(gameObject);
        }
    }

}
