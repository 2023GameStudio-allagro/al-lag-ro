using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
	[SerializeField] private GameObject playerRef;
	[SerializeField] private Dictionary<string, GameObject> prefabs;

	public GameObject GetPrefab(string enemyType)
	{
		GameObject result;
		if (prefabs.TryGetValue(enemyType, out result)) return result;
		else return null;
	}
	public GameObject Make(GameObject prefab, int hp, float speed)
	{
		GameObject enemy = Instantiate(prefab);
		EnemyBase enemyBrain = enemy.GetComponent<EnemyBase>();
		enemyBrain.SetHP(hp);
		enemyBrain.SetSpeed(speed);
		enemyBrain.SetPlayer(playerRef);
		return enemy;
	}
}