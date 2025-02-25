using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public enum EnemyType
{
    SmallFish, 
    SeaHorse,
    Turtle, 
    Dolphin, 
    Ray, 
    Shark
}

public class EnemySpawnTable : MonoBehaviour
{
    List<EnemyType> enemyTypeList = new List<EnemyType>();
    List<List<GameObject>> enemyPrefabList = new List<List<GameObject>>();
    
    readonly int enemyTypeCount = Enum.GetValues(typeof(EnemyType)).Length;


    private void Awake()
    {
        for (int i = 0; i < enemyTypeCount; i++)
        {
            enemyTypeList.Add((EnemyType)i);
        }
    }

    public GameObject GetRandomEnemyPrefab()
    {
        int randType = UnityEngine.Random.Range(0, enemyTypeCount);
        int randNum = UnityEngine.Random.Range(0, enemyPrefabList[randType].Count);
        return enemyPrefabList[randType][randNum];
    }

    public GameObject GetRandomEnemyPrefab(EnemyType enemyType)
    {
        int randNum =  UnityEngine.Random.Range(0, enemyPrefabList[(int)enemyType].Count);
        return enemyPrefabList[(int)enemyType][randNum];
    }
}
