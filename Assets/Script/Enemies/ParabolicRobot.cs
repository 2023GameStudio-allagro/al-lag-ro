using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicRobot : EnemyBase
{
    private float xVelocity;
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
        if(Random.Range(0f, 1f) < 0.025) yDirection = -yDirection;
        if(rigid.position.y > 4f) yDirection = -1;
        if(rigid.position.y < -4f) yDirection = 1;
        rigid.velocity = new Vector2(xVelocity, yDirection * 3f);
    }
    protected override void OnDead()
    {
        base.OnDead();
        // 귀찮아요... 이 패턴 안티패턴인 건 아는데 나중에 리팩터링할예정
        // Player player = GameObject.Find("player").GetComponent<Player>();
        // player.AttackAll();
        activator?.Activate();
    }
    public override void SetSpeed(float multiplier)
    {
        xVelocity = -multiplier * Utils.GetBaseSpeed(MusicManager.Instance.bpm);
        rigid.velocity = new Vector2(xVelocity, 0f);
    }
}