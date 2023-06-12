using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private float z;
    [SerializeField] private GameObject follower;
    // Start is called before the first frame update
    void Start()
    {
        this.z = transform.position.z;
        transform.position = ProjectToPlane(follower.transform.position, this.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 nextPosition = follower.transform.position;
        Vector3 curPosition = transform.position;
        Vector3 resPosition = Vector2.Lerp(curPosition, nextPosition, 0.8f);
        transform.position = ProjectToPlane(resPosition, this.z);
    }

    private Vector3 ProjectToPlane(Vector3 to, float z)
    {
        Vector3 result = to;
        result.z = z;
        return result;
    }
}
