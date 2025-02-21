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
    public event Action OnDeath;

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
            DecreaseHunger(hungerDecreaseAmount);
        }
    }

    public void DecreaseHunger(int amount)
    {
        if (CurrentHunger <= 0)
        {
            TriggerDeath();
            return;
        }

        CurrentHunger = CurrentHunger - amount > 0 ? CurrentHunger - amount : 0;

        if (CurrentHunger <= 0)
            TriggerDeath();
    }

    public void IncreaseHunger(int hunger)
    {
        CurrentHunger = CurrentHunger + hunger > MaxHunger ? MaxHunger : CurrentHunger + hunger;
    }
    
    public void AddHungerDecrease(int x)
    {
        hungerDecreaseAmount = x + BaseHungerDecreaseAmount;
    }

    public void TriggerDeath()
    {
        Debug.Log("die");
        OnDeath?.Invoke();
    }
}