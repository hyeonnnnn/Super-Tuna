using System;
using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    private const int MaxHunger = 100;
    private const int BaseHungerDecreaseAmount = 5;
    private const float HungerDecreaseInterval = 1f;

    private int currentHunger;
    public int CurrentHunger
    {
        get { return currentHunger; }
        set
        {
            currentHunger = value;
            OnHungerChanged?.Invoke(currentHunger, MaxHunger);
        }
    }
    private int hungerDecreaseAmount = BaseHungerDecreaseAmount;

    public static event Action<int, int> OnHungerChanged;
    public static event Action<DyingReason> OnDeath;

    private void Start()
    {
        CurrentHunger = MaxHunger;
        InvokeRepeating(nameof(ReduceHungerOverTime), HungerDecreaseInterval, HungerDecreaseInterval);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            IncreaseHunger(5);
        }
    }

    private void ReduceHungerOverTime()
    {
        if(CurrentHunger > 0)
        {
            DecreaseHunger(hungerDecreaseAmount, DyingReason.Hunger);
        }
    }

    public void DecreaseHunger(int amount, DyingReason dyingReason)
    {
        if (CurrentHunger <= 0)
        {
            TriggerDeath(dyingReason);
            return;
        }

        CurrentHunger = CurrentHunger - amount > 0 ? CurrentHunger - amount : 0;

        if (CurrentHunger <= 0)
            TriggerDeath(dyingReason);
    }

    public void IncreaseHunger(int hunger)
    {
        CurrentHunger = CurrentHunger + hunger > MaxHunger ? MaxHunger : CurrentHunger + hunger;
    }
    
    public void AddHungerDecrease(int x)
    {
        hungerDecreaseAmount = x + BaseHungerDecreaseAmount;
    }

    public void TriggerDeath(DyingReason dyingReason)
    {
        Debug.Log("die");
        OnDeath?.Invoke(dyingReason);
    }
}