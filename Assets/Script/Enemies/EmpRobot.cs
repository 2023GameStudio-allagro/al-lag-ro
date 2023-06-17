using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpRobot : EnemyBase
{
    public GameObject deadAnimationPrefab;
    public GameObject boomPrefab;
    private float xVelocity;
    private int yDirection = 1;
    private bool isAlive = true; // Variable to track if the enemy is alive

    protected override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            if (rigid.position.y > 4f) yDirection = -1;
            if (rigid.position.y < -4f) yDirection = 1;
            rigid.velocity = new Vector2(xVelocity, yDirection * 5f);
        }
        else
        {
            //rigid.rotation += 400f * Time.fixedDeltaTime;
        }
    }

    protected override void OnDead()
    {
        base.OnDead();

        isAlive = false; // Set the enemy as dead

        GetComponent<SpriteRenderer>().enabled = false;
        Vector3 enemyPosition = transform.position;
        GameObject deadAnimationObject = Instantiate(deadAnimationPrefab, enemyPosition, Quaternion.identity);
        Instantiate(boomPrefab, enemyPosition, Quaternion.identity);

        Vector2 targetPosition = new Vector2(18f, 1f);
        deadAnimationObject.GetComponent<DeadEffectController>().targetPosition = targetPosition;

        Destroy(deadAnimationObject, 1.0f);

        Destroy(gameObject, 1.0f); // Destroy the enemy after a delay
    }

    public override void SetSpeed(float multiplier)
    {
        xVelocity = -multiplier * Utils.GetBaseSpeed(MusicManager.Instance.bpm);
        rigid.velocity = new Vector2(xVelocity, 0f);
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EmpRobot : EnemyBase
//{
//    private float xVelocity;
//    private int yDirection = 1;

//    protected override void Start()
//    {
//        base.Start();
//    }
//    void FixedUpdate()
//    {
//        if(rigid.position.y > 4f) yDirection = -1;
//        if(rigid.position.y < -4f) yDirection = 1;
//        rigid.velocity = new Vector2(xVelocity, yDirection * 5f);
//    }
//    protected override void OnDead()
//    {
//        base.OnDead();
//        // 귀찮아요... 이 패턴 안티패턴인 건 아는데 나중에 리팩터링할예정
//        Player player = GameObject.Find("player").GetComponent<Player>();
//        player.AttackAll();
//    }
//    public override void SetSpeed(float multiplier)
//    {
//        xVelocity = -multiplier * Utils.GetBaseSpeed(MusicManager.Instance.bpm);
//        rigid.velocity = new Vector2(xVelocity, 0f);
//    }
//}