
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody2D rigid;
    private Transform mainCamera;
    //private bool isCameraFollowing = true;

    //private void Start()
    //{
    //    rigid = GetComponent<Rigidbody2D>();
    //    mainCamera = Camera.main.transform;
    //    mainCamera.position = new Vector3(transform.position.x, mainCamera.position.y, mainCamera.position.z);
    //}
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main.transform;

        // Calculate the target X position for the player within the camera view
        float targetX = mainCamera.position.x + 16f;

        // Set the initial position of the player
        Vector3 initialPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = initialPosition;

        // Set the initial position of the camera
        Vector3 cameraPosition = new Vector3(targetX, mainCamera.position.y, mainCamera.position.z);
        mainCamera.position = cameraPosition;
    }

    private void FixedUpdate()
    {
        // Check if the player is at the upper or lower edge of the camera
        bool isTouchingUpperEdge = transform.position.y >= (mainCamera.position.y + Camera.main.orthographicSize);
        bool isTouchingLowerEdge = transform.position.y <= (mainCamera.position.y - Camera.main.orthographicSize);

        // Move the camera if the player is touching the upper or lower edge
        if (isTouchingUpperEdge || isTouchingLowerEdge)
        {
            mainCamera.position += Vector3.right * (moveSpeed * 32f * Time.deltaTime);
        }

        // Move the player vertically
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 velocity = new Vector2(0f, vertical);
        velocity = velocity.normalized * moveSpeed;
        rigid.velocity = velocity;

        // Update the player's position to be in the second column of the camera view
        Vector3 playerPosition = transform.position;
        playerPosition.x = mainCamera.position.x - 8f;
        transform.position = playerPosition;
    }
}