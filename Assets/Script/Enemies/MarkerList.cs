using System.Collections;
using System.Collections.Generic;

public class MarkerList : IEnumerable<AttackKey>, IEnemyMarker
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

    public MarkerList(int maxHP)
    {
        markers = new AttackKey[maxHP];
        for(int i=0; i<maxHP; i++)
        {
            markers[i] = (AttackKey)(1 << UnityEngine.Random.Range(0, 4));
        }
        start = 0;
        end = maxHP;
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
}