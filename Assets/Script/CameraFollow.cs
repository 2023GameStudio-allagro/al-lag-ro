using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float cameraSpeed = 2f; // Speed at which the camera moves
    public float followThreshold = 0.1f; // Threshold for camera following

    private float targetY; // Y position to follow the player
    private bool shouldFollow = false; // Flag to determine if the camera should follow the player

    private void LateUpdate()
    {
        // Calculate the target Y position based on the player's position
        targetY = player.position.y;


        // Check if the player is at the top or bottom edge of the camera view
        float screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        float screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        float playerTop = player.position.y + player.localScale.y / 2;
        float playerBottom = player.position.y - player.localScale.y / 2;

        // 플레이어가 카메라 맨위나 맨아래 닿고 있으면
        if (playerTop >= screenTop || playerBottom <= screenBottom)
        {
            shouldFollow = true;
        }

        // Move the camera towards the target position only if shouldFollow is true
        if (shouldFollow)
        {
            // Move the camera vertically towards the target position
            Vector3 targetPosition = transform.position;
            targetPosition.y = Mathf.Lerp(targetPosition.y, targetY, Time.deltaTime * cameraSpeed);
            transform.position = targetPosition;

            if (playerTop < screenTop && playerBottom > screenBottom)
            {
                shouldFollow = false;

                // Find all the enemy game objects and stop them from following the camera
                EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();
                foreach (EnemyMovement enemy in enemies)
                {
                    enemy.StopFollowingCamera();
                }
            }
        }
    }
}