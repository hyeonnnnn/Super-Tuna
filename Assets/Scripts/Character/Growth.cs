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

    public static event Action<int, int> OnExpGaugeChanged;
    public static event Action<int> OnLevelChanged;

    public Growth()
    {
        expTable = new int[] { 0, 300, 1000 };
        MaxExp = expTable[expTable.Length - 1];
        MaxLevel = expTable.Length;
    }

    private void Start()
    {
        OnExpGaugeChanged?.Invoke(CurrentExp, MaxExp);
        OnLevelChanged?.Invoke(CurrentLevel);
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

    public void AddExp(int expAmount)
    {
        if (CurrentLevel >= MaxLevel)
        {
            return;
        }
        else
        {
            CurrentExp = Math.Min(MaxExp, CurrentExp + expAmount);
            OnExpGaugeChanged?.Invoke(CurrentExp, MaxExp);
            CheckLevelUp();
        }
    }

    private void CheckLevelUp()
    {
        if(CurrentExp >= expTable[CurrentLevel])
        {
            ApplyLevelUp();
        }
    }

    private void ApplyLevelUp()
    {
        CurrentLevel += 1;
        OnLevelChanged?.Invoke(CurrentLevel);
        ChangePrefab();
    }

    private void ChangePrefab()
    {
        characterPrefabs[characterPrefabsInx].SetActive(false);
        characterPrefabsInx++;
        characterPrefabs[characterPrefabsInx].SetActive(true);
        IncreaseScale();
    }

    private void IncreaseScale()
    {
        float scaleMultiplier = 1 + (CurrentLevel * 0.8f);
        transform.localScale = baseScale * scaleMultiplier;
    }
}
