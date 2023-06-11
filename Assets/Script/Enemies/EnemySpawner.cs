using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	private List<SpawnPatternData> levelPatternsCache;
	private SpawnPatternData currentSpawnPattern;
	private EnemyFactory factory;

	void Awake()
	{
		factory = GetComponent<EnemyFactory>();
		levelPatternsCache = new List<SpawnPatternData>();
		foreach(string addr in Constants.SPAWN_PATTERN_RESOURCES)
		{
			levelPatternsCache.Add(JsonLoader.LoadJsonData<SpawnPatternData>(addr));
		}
	}

	void Start()
	{
		LoadStage(1);
	}

	public void OnBeatReceived(int beatNo)
	{
		List<SpawnEnemyData> enemyData = currentSpawnPattern.GetEnemyDataFromBeatIndex(beatNo);
		SpawnEnemy(enemyData);
	}

	public void LoadStage(int stageNo)
	{
		if(stageNo < 1) return;
		currentSpawnPattern = levelPatternsCache[stageNo-1];
	}

	private void SpawnEnemy(List<SpawnEnemyData> enemyData)
	{
		if(enemyData == null || enemyData.Count == 0) return;
		foreach(SpawnEnemyData eachEnemyData in enemyData)
		{
			GameObject enemyPrefab = factory.GetPrefab(eachEnemyData.type);
			if(enemyPrefab == null) continue;
			for(int i=0; i<eachEnemyData.amount; i++)
			{
				GameObject enemy = factory.Make(enemyPrefab, eachEnemyData.hp, eachEnemyData.speed);
				enemy.transform.position = GetSpawnPosition();
			}
		}
	}

	private Vector3 GetSpawnPosition()
	{
		float cameraHeight = Camera.main.orthographicSize * 2;
		float cameraWidth = cameraHeight * Camera.main.aspect;
		Bounds cameraBound = new Bounds
		(
			Camera.main.transform.position,
			new Vector3(cameraWidth, cameraHeight, 0f)
		);

		return Utils.GetRandomPositionOutBound(cameraBound, 1f);
	}
}