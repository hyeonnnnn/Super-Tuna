using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    [SerializeField] int maxMineCount;
    int currentSpawnedMineCount = 0;
    bool isMineSpawning = false;
    private float spawnCoolTime = 0.5f;

    [SerializeField] List<Mine> mineSpawnPoint = new List<Mine>();

    private void Awake()
    {
        Mine[] mines = GetComponentsInChildren<Mine>();

        for (int i = 0; i < mines.Length; i++)
        {
            mineSpawnPoint.Add(mines[i]);
            mines[i].enabled = false;
            //mines[i].explodedEvent += TryMineSpawn();
        }
    }

    private void Start()
    {
        int[] initMineSpawn = new int[maxMineCount];
        while (maxMineCount > currentSpawnedMineCount)
        {
            int randNum = Random.Range(0, mineSpawnPoint.Count);
            if(initMineSpawn.Contains(randNum))
            {
                continue;
            }
            initMineSpawn[currentSpawnedMineCount] = randNum;
            currentSpawnedMineCount++;
        }
        for (int i = 0; i < maxMineCount; i++)
        {
            mineSpawnPoint[i].enabled = true;
        }
    }

    private void TryMineSpawn()
    {
        currentSpawnedMineCount--;
        if(!isMineSpawning && maxMineCount > currentSpawnedMineCount)
        {
            isMineSpawning = true;
            StartCoroutine(SpawnMine());
        }
    }

    private IEnumerator SpawnMine()
    {
        while(isMineSpawning)
        {
            int randMine = Random.Range(0, mineSpawnPoint.Count);
            while(mineSpawnPoint[randMine].enabled == true || CheckMineInCamera(mineSpawnPoint[randMine].transform.position))
            {
                yield return new WaitForSeconds(spawnCoolTime);
                randMine = Random.Range(0, mineSpawnPoint.Count);
            }

            mineSpawnPoint[randMine].enabled = true;
            currentSpawnedMineCount++;
            if(currentSpawnedMineCount ==  maxMineCount)
            {
                isMineSpawning = false;
            }
        }
    }

    private bool CheckMineInCamera(Vector3 position)
    {
        Camera playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
        Vector3 screenPosition = playerCamera.WorldToViewportPoint(position);
        bool onScreen = screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.y > 0 && screenPosition.x < 1 && screenPosition.y < 1;
        
        return onScreen;
    }

}
