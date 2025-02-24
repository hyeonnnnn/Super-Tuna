using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public string enemyType;
    public int level;
    public float sightRange;
    public float speed;
    public int exp;
    public int hungerValue;
}
