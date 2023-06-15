using UnityEngine;
using System.Collections.Generic;

public interface IEnemyMarker
{
    public int hp {get;}
    public AttackKey peak {get;}
    public void Decrease(int amount);
    public List<AttackKey> ToList();
}
