using UnityEngine;

public class CameraFollowNonPlayer : MonoBehaviour
{

    public float cameraSpeed = 2f; // Speed at which the camera moves
    public float followThreshold = 0.1f; // Threshold for camera following

    private float targetY; // Y position to follow the player
    private void LateUpdate()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y = Mathf.Lerp(targetPosition.y, targetY, Time.deltaTime * cameraSpeed);
        transform.position = targetPosition;
    }
}