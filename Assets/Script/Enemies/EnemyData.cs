using UnityEngine;

[CreateAssetMenu(fileName = "enemy", menuName = "Data/Enemy Data", order = 2)]
public class EnemyData : ScriptableObject
{
    public char identifier;
    public float speed;
    public GameObject prefab;
    public GameObject[] variations;
}
