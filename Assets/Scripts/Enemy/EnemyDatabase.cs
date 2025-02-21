using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "ScriptableObjects/EnemyDatabase", order = 2)]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> enemies;
}
