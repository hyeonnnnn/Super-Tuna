using UnityEngine;
using System;

public class Growth : MonoBehaviour
{
    private readonly int[] expTable;
    private readonly int MaxExp;
    private readonly int MaxLevel;

    public int CurrentExp { get; private set; } = 0;
    public int CurrentLevel { get; private set; } = 1;

    [SerializeField] private GameObject[] characterPrefabs;
    private int characterPrefabsInx = 0;
    private Vector3 baseScale = Vector3.one;

    public Growth()
    {
        expTable = new int[] { 0, 300, 1000 };
        MaxExp = expTable[expTable.Length - 1];
        MaxLevel = expTable.Length;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddExp(40);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            AddExp(80);
        }
    }

    // 경험치 증가
    public void AddExp(int expAmount)
    {
        if (CurrentLevel >= MaxLevel)
        {
            return;
        }
        else
        {
            CurrentExp = Math.Min(MaxExp, CurrentExp + expAmount);
            Debug.Log("Exp: " + CurrentExp);
            CheckLevelUp();
        }
    }

    // 레벨 업 조건 확인
    private void CheckLevelUp()
    {
        if(CurrentExp >= expTable[CurrentLevel])
        {
            ApplyLevelUp();
        }
    }

    // 레벨 업 적용
    private void ApplyLevelUp()
    {
        CurrentLevel += 1;
        ChangePrefab();
        Debug.Log("Level: " + CurrentLevel);
    }

    // 캐릭터 진화 (프리팹 변경)
    private void ChangePrefab()
    {
        characterPrefabs[characterPrefabsInx].SetActive(false);
        characterPrefabsInx++;
        characterPrefabs[characterPrefabsInx].SetActive(true);
        IncreaseScale();
    }

    // 캐릭터 크기 증가
    private void IncreaseScale()
    {
        float scaleMultiplier = 1 + (CurrentLevel * 0.8f);
        transform.localScale = baseScale * scaleMultiplier;
    }
}
