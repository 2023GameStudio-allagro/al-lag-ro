using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    bool CanDamage(AttackKey keys);
    void Damage(int power);
}