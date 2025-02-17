using System;
using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    private const float MaxHunger = 100f;
    private const float BaseHungerDecreaseAmount = 5f;
    private const float HungerDecreaseInterval = 1f;

    private float currentHunger;
    private float hungerDecreaseAmount = BaseHungerDecreaseAmount;
    [SerializeField] private bool isRadioactive = false;

    public event Action<float, float> OnHungerChanged;
    public event Action OnDeath;

    private void Start()
    {
        currentHunger = MaxHunger;
        InvokeRepeating(nameof(ReduceHungerOverTime), HungerDecreaseInterval, HungerDecreaseInterval);
        NotifyHungerChanged();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            IncreaseHunger(5f);
        }
    }

    private void ReduceHungerOverTime()
    {
        DecreaseHunger(isRadioactive ? hungerDecreaseAmount * 2 : hungerDecreaseAmount);
    }

    public void DecreaseHunger(float amount)
    {
        if (currentHunger <= 0)
        {
            TriggerDeath();
            return;
        }

        currentHunger = Mathf.Max(currentHunger - amount, 0);
        NotifyHungerChanged();

        if (currentHunger <= 0)
        {
            TriggerDeath();
        }
    }

    public void IncreaseHunger(float hunger)
    {
        currentHunger = Mathf.Min(currentHunger + hunger, MaxHunger);
        NotifyHungerChanged();
    }

    public void AddHungerDecrease(float x)
    {
        currentHunger = Mathf.Max(currentHunger - x, 0);
    }

    public void TriggerDeath()
    {
        OnDeath?.Invoke();
    }

    private void NotifyHungerChanged()
    {
        OnHungerChanged?.Invoke(currentHunger, MaxHunger);
    }
}