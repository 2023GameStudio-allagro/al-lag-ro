

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float cameraSpeed = 2f;
    public Vector3 offset;
    public Transform spawnParentTransform;
    public float edgeMargin = 0.1f;

    private bool isCameraFollowing = false;

    private void Start()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }

    private void LateUpdate()
    {
        if (isCameraFollowing && player != null)
        {
            Vector3 targetPosition = player.position + offset;
            targetPosition.x = transform.position.x; // Lock the camera's X position

            // Calculate the desired position for the camera
            Vector3 desiredPosition = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);

            // Check if the player is at the edge of the camera view
            float playerTop = targetPosition.y + player.GetComponent<SpriteRenderer>().bounds.extents.y;
            float playerBottom = targetPosition.y - player.GetComponent<SpriteRenderer>().bounds.extents.y;
            float cameraTop = desiredPosition.y + Camera.main.orthographicSize;
            float cameraBottom = desiredPosition.y - Camera.main.orthographicSize;

            if (playerTop >= cameraTop - edgeMargin || playerBottom <= cameraBottom + edgeMargin)
            {
                spawnParentTransform.position += new Vector3(cameraSpeed * Time.deltaTime, 0f, 0f);
            }
            else
            {
                // Move the camera to the desired position
                transform.position = desiredPosition;
            }
        }
        else
        {
            // Automatically move the camera to the right
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        }
    }

    public void StartFollowing()
    {
        isCameraFollowing = true;
    }

    public bool IsCameraFollowing()
    {
        return isCameraFollowing;
    }
}