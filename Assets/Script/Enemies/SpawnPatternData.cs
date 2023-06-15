using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class SpawnEnemyData
{

    public string type;
    public int hp;
    public float speed;
    public int amount;
    public int interval;

    public bool followCamera = true;

    public int beatIndex { get; internal set; }
}

[Serializable]
public class SpawnSegmentData
{
    public int measures;
    public List<SpawnEnemyData> patterns;
}

[Serializable]
public class SpawnPatternData
{
    private List<int> cachedIndices;
    public List<SpawnSegmentData> segments;



    public List<SpawnEnemyData> GetEnemyDataFromCurrentBeat(int currentBeat)
    {
        List<SpawnEnemyData> enemyData = new List<SpawnEnemyData>();

        foreach (SpawnSegmentData segment in segments)
        {
            foreach (SpawnEnemyData enemy in segment.patterns)
            {
                if (enemy.beatIndex == currentBeat)
                {
                    enemyData.Add(enemy);
                }
            }
        }

        return enemyData;
    }




    public List<SpawnEnemyData> GetEnemyDataFromBeatIndex(int beatNo)
    {
        if (cachedIndices == null) BakeIndicesCache();

        // ���ܱ��� ����
        if (beatNo < 0) return null;
        if (beatNo / 4 >= cachedIndices.Last() + segments.Last().measures) return null;

        // ���� �������� �ش� ����no�� ã�Ƽ� �۰ų� ���� �������� �� ���� ū �ε����� ��ȯ��.
        // cachedIndices�� �̹� ���ĵǾ� ����.
        int index = cachedIndices.BinarySearch(beatNo / 4);
        if (index < 0) index = ~index - 1;

        SpawnSegmentData segment = segments[index];
        int segmentBeatNo = beatNo - cachedIndices[index] * 4;
        return segment.patterns.Where(enemyData => segmentBeatNo % enemyData.interval == 0).ToList();
    }
    private void BakeIndicesCache()
    {
        cachedIndices = new List<int>();
        int sum = 0;
        cachedIndices.Add(0);
        for (int i = 1; i < segments.Count; i++)
        {
            sum += segments[i].measures;
            cachedIndices.Add(sum);
        }
    }
}