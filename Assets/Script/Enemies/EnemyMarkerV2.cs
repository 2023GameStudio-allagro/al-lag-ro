using System.Collections;
using System.Collections.Generic;

public class EnemyMarkerV2 : IEnumerable<AttackKey>, IEnemyMarker
{
    private int start;
    private int end;
    private AttackKey[] markers;
    public int hp
    {
        get
        {
            if(start > end) return 0;
            return end - start;
        }
    }
    public AttackKey peak
    {
        get
        {
            if(start >= end) throw new System.InvalidOperationException("Cannot call .peak on an empty queue");
            return markers[start];
        }
    }

    public EnemyMarkerV2(char enemyType)
    {
        markers = new AttackKey[1];
        markers[0] = GetMarker(enemyType);
        start = 0;
        end = 1;
    }

    public void Decrease(int amount)
    {
        if(amount < 0) return;
        if(hp <= 0) return;
        start++;
        if(amount > 1) end -= amount - 1;
    }

    public List<AttackKey> ToList()
    {
        List<AttackKey> result = new List<AttackKey>();
        for(int i=start; i<end; i++)
        {
            result.Add(markers[i]);
        }
        return result;
    }

    public IEnumerator<AttackKey> GetEnumerator()
    {
        for (int i=start; i<end; i++)
        {
            yield return markers[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private AttackKey GetMarker(char enemyType)
    {
        switch(enemyType)
        {
            case 'z' : return (AttackKey)(1 << 0);
            case 'x' : return (AttackKey)(1 << 1);
            case 'c' : return (AttackKey)(1 << 2);
            case 'v' : return (AttackKey)(1 << 3);
            default : return (AttackKey)(1 << UnityEngine.Random.Range(0, 4));
        }
    }
}