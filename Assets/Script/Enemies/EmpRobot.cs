using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpRobot : EnemyBase
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hp = 3;
        moveSpeed = 2;
    }
}