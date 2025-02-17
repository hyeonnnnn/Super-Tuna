using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : MonoBehaviour
{
    private const float MaxHunger = 100f;
    private const float HungerDecreaseAmount = 5f;
    private const float HungerDecreaseInterval = 1f;

    [SerializeField] private Slider hungerBarSlider;
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private bool isRadioactive = false;

    private float currentHunger;

    public bool IsRadioactive
    {
        get => isRadioactive;
        set => isRadioactive = value;
    }

    private void Start()
    {
        currentHunger = MaxHunger;

        if (hungerBarSlider != null)
        {
            hungerBarSlider.maxValue = 1;
            UpdateHungerUI();
        }

        InvokeRepeating(nameof(ReduceHungerOverTime), HungerDecreaseInterval, HungerDecreaseInterval);
    }

    private void ReduceHungerOverTime()
    {
        DecreaseHunger(HungerDecreaseAmount);
    }

    public void DecreaseHunger(float amount)
    {
        if (currentHunger <= 0)
        {
            TriggerDeath();
            return;
        }

        if (isRadioactive) amount *= 2;

        currentHunger = Mathf.Max(currentHunger - amount, 0);
        UpdateHungerUI();
    }

    public void RecoverHunger(float amount)
    {
        currentHunger = Mathf.Min(currentHunger + amount, MaxHunger);
        UpdateHungerUI();
    }

    private void UpdateHungerUI()
    {
        if (hungerBarSlider != null)
        {
            hungerBarSlider.value = currentHunger / MaxHunger;
        }
    }

    private void TriggerDeath()
    {
        HandleGameOverUI();
    }

    private void HandleGameOverUI()
    {
        if (hpBar) hpBar.SetActive(false);
        if (gameOverUI) gameOverUI.SetActive(true);
    }
}