using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class HungerSystem : MonoBehaviour
{
    private const int MaxHunger = 100;
    private const int BaseHungerDecreaseAmount = 5;
    private const float HungerDecreaseInterval = 1f;

    [SerializeField] private Animator fishAnimator;

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
    public static event Action OnDeath;
    public static event Action<DyingReason> ShowResult;

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
        fishAnimator.SetTrigger("Dying");
        OnDeath?.Invoke();
        StartCoroutine(WaitForDeathAnimation(dyingReason));
    }

    public void ChangeAnimator(Animator fishAnimator)
    {
        this.fishAnimator = fishAnimator;
    }

    private IEnumerator WaitForDeathAnimation(DyingReason dyingReason)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => fishAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        ShowResult?.Invoke(dyingReason);
    }
}