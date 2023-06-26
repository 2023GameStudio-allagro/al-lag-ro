using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicRobot : EnemyBase
{
    private int yDirection = 1;
    private SlowzoneActivator activator;

    protected override void Awake()
    {
        base.Awake();
        activator = GameObject.Find(Constants.SLOW_ZONE_RUNNER)?.GetComponent<SlowzoneActivator>();
    }
    protected override void Start()
    {
        base.Start();
    }
    void FixedUpdate()
    {
        if (Random.Range(0f, 1f) < 0.025) yDirection = -yDirection;
        if (rigid.position.y > 4f) yDirection = -1;
        if (rigid.position.y < -4f) yDirection = 1;
        rigid.velocity = new Vector2(xVelocity, yDirection * 3f);
    }
    protected override void OnDead()
    {
        activator?.Activate();
        base.OnDead();
    }
}