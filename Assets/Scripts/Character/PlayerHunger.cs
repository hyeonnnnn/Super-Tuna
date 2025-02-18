using System;
using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    private const float MaxHunger = 100f;
    private const float BaseHungerDecreaseAmount = 5f;
    private const float HungerDecreaseInterval = 1f;
    private const float RadioactiveHungerIncrease = 5f;

    [SerializeField]private float currentHunger;
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

        if (isRadioactive)
        {
            AddHungerDecrease(RadioactiveHungerIncrease);
        }
        else
        {
            hungerDecreaseAmount = BaseHungerDecreaseAmount;
        }
    }

    private void ReduceHungerOverTime()
    {
        if(currentHunger > 0)
        {
            DecreaseHunger(hungerDecreaseAmount);
        }
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
        hungerDecreaseAmount = Mathf.Max(BaseHungerDecreaseAmount + x, 10f);
    }

    public void TriggerDeath()
    {
        Debug.Log("die");
        OnDeath?.Invoke();
    }

    private void NotifyHungerChanged()
    {
        OnHungerChanged?.Invoke(currentHunger, MaxHunger);
    }
}