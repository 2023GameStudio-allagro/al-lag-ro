using UnityEngine;

public class DeadEffectController : MonoBehaviour
{
    public float spinSpeed = 700f; // Adjust the spinning speed as desired
    public float moveSpeed = 5f; // Adjust the movement speed as desired
    public Vector2 targetPosition; // Set the target position in the Inspector or via script

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // Calculate the direction and distance to the target position
        Vector2 direction = (targetPosition - rb.position).normalized;
        float distance = Vector2.Distance(rb.position, targetPosition);

        // Move towards the target position
        rb.velocity = direction * moveSpeed;

        // Check if the target position has been reached
        if (distance < 0.1f)
        {
            // Destroy the dead effect prefab
            Destroy(gameObject);
        }
    }
}