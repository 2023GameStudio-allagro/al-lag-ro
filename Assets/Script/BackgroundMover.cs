using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private int length = 32;
    // Update is called once per frame
    void Update()
    {
        float speed = Utils.GetBaseSpeed(MusicManager.Instance.bpm);
        Vector3 pos = transform.position;
        pos.x -= speed * Time.deltaTime;
        if(pos.x < -length) pos.x += length;
        transform.position = pos;
    }
}
