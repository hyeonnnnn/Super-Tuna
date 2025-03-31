using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    [SerializeField] int maxMineCount;
    [SerializeField] List<Mine> currentspawnedMine = new List<Mine>();
    [SerializeField] List<Mine> DisabledMinePoint = new List<Mine>();

    [SerializeField] Camera playerCamera;

    bool isMineSpawning = false;
    //private float spawnCoolTime = 0.5f;

    private void Start()
    {
        Mine[] mines = GetComponentsInChildren<Mine>();

        for (int i = 0; i < mines.Length; i++)
        {
            DisabledMinePoint.Add(mines[i]);
            mines[i].gameObject.SetActive(false);
        }

        int[] initMineSpawn = new int[maxMineCount];

        while (maxMineCount > currentspawnedMine.Count)
        {
            int randNum = Random.Range(0, DisabledMinePoint.Count);

            currentspawnedMine.Add(DisabledMinePoint[randNum]);
            DisabledMinePoint.RemoveAt(randNum);
        }
        for (int i = 0; i < maxMineCount; i++)
        {
            currentspawnedMine[i].ResetMine();
        }
    }

    private void TryMineSpawn(Mine mine)
    {
        currentspawnedMine.Remove(mine);
        DisabledMinePoint.Add(mine);

        if (!isMineSpawning && maxMineCount > currentspawnedMine.Count)
        {
            isMineSpawning = true;
            StartCoroutine(SpawnMine());
        }
    }

    private IEnumerator SpawnMine()
    {
        while(isMineSpawning)
        {
            int randMine = Random.Range(0, DisabledMinePoint.Count);

            while (CheckMineInCamera(DisabledMinePoint[randMine].transform.position))
            {
                yield return null;
                randMine = Random.Range(0, DisabledMinePoint.Count);
            }

            DisabledMinePoint[randMine].ResetMine();
            //Debug.Log(DisabledMinePoint[randMine]);
            currentspawnedMine.Add(DisabledMinePoint[randMine]);
            DisabledMinePoint.RemoveAt(randMine);

            if(currentspawnedMine.Count >= maxMineCount)
            {
                isMineSpawning = false;
            }
        }
    }

    private bool CheckMineInCamera(Vector3 position)
    {
        Vector3 screenPosition = playerCamera.WorldToViewportPoint(position);
        bool onScreen = screenPosition.z > -0.1f && screenPosition.x > -0.1f && screenPosition.y > -0.1f && screenPosition.x < 1.1f && screenPosition.y < 1.1f;
        
        return onScreen;
    }

}
