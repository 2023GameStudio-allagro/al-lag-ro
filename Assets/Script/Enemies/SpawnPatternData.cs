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

    public List<SpawnEnemyData> GetEnemyDataFromBeatIndex(int beatNo)
    {
        if(cachedIndices == null) BakeIndicesCache();

        // 예외구간 리턴
        if(beatNo < 0) return null;
        if(beatNo / 4 >= cachedIndices.Last() + segments.Last().measures) return null;

        // 누적 구간에서 해당 박자no를 찾아서 작거나 같은 누적구간 중 가장 큰 인덱스를 반환함.
        // cachedIndices는 이미 정렬되어 있음.
        int index = cachedIndices.BinarySearch(beatNo / 4);
        if(index < 0) index = ~index - 1;

        SpawnSegmentData segment = segments[index];
        int segmentBeatNo = beatNo - cachedIndices[index] * 4;
        return segment.patterns.Where(enemyData => segmentBeatNo % enemyData.interval == 0).ToList();
    }
    private void BakeIndicesCache()
    {
        cachedIndices = new List<int>();
        int sum = 0;
        cachedIndices.Add(0);
        for(int i=1; i<segments.Count; i++)
        {
            sum += segments[i].measures;
            cachedIndices.Add(sum);
        }
    }
}