using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame, and not recommended.
    void Update()
    {
    }
    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 velocity = new Vector2(horizontal, vertical);
        velocity = velocity.normalized * moveSpeed;
        rigid.velocity = velocity;
    }

}

