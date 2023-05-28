using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float stunDuration = 0f;

    private Rigidbody2D rigid;
    private JudgeLineCreator judgeSystem;

    public float moveSpeed = 5f;
    public bool stun
    {
        get
        {
            return stunDuration > 0f;
        }
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        judgeSystem = GetComponentInChildren<JudgeLineCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!stun)
        {
            InputCommand command = GetInputCommand();
            if(command != null) Attack(command);
        }
        else DiminishStunDuration();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 velocity;
        if(stun) velocity = Vector2.zero;
        else velocity = new Vector2(horizontal, vertical);

        Move(velocity);
    }

    InputCommand GetInputCommand()
    {
        if(Input.GetKeyDown(KeyCode.Q)) return new InputCommand(CommandKey.q);
        return null;
    }

    void Move(Vector2 velocity)
    {
        velocity = velocity.normalized * moveSpeed;
        rigid.velocity = velocity;
    }

    void Attack(InputCommand command)
    {
        Judgement judgement = judgeSystem.HitNote();
        if(judgement != Judgement.early) judgeSystem.RemoveCurrentNode();
        switch(judgement)
        {
            case Judgement.perfect:
                Debug.Log("perfect attacked!");
                break;
            case Judgement.good:
                Debug.Log("good attacked!");
                break;
            default:
                SetStun(Constants.STUN_DURATION);
                Debug.Log("missed!");
                break;
        }
    }

    void SetStun(float duration)
    {
        stunDuration = duration;
    }

    void DiminishStunDuration()
    {
        if(!stun) return;
        stunDuration -= Time.deltaTime;
        if(stunDuration < 0f) stunDuration = 0f;
    }
}