using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool shouldFollowCamera = false;// spawn된 후에 바로 false
    float moveSpeed = 2f;
    // Update is called once per frame
    private void Update()
    {
        MoveLeft();
        if (shouldFollowCamera)
        {
        }
    }

    // Call this method to stop the enemy from following the camera
    public void StopFollowingCamera()
    {
        shouldFollowCamera = false;
    }
    private void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }
}