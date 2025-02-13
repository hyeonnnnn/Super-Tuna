using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : MonoBehaviour
{
    public float currentHunger;
    public float maxHunger = 100f;
    public float hungerDecreaseAmount = 5f;
    public float hungerDecreaseInterval = 1f; 
    public Slider hungerBarSlider;
    public GameObject hpBar;
    public bool radioactivity = false;
    public GameObject gameOverUI;
    
    private void Start()
    {
        currentHunger = maxHunger;
        InvokeRepeating(nameof(DecreaseHungerOverTime), hungerDecreaseInterval, hungerDecreaseInterval);
    }

    private void DecreaseHungerOverTime()
    {
        DecreaseHunger(hungerDecreaseAmount);
    }

    public void DecreaseHunger(float amount)
    {
        if (currentHunger <= 0)
        {
            TriggerDeath();
            return;
        }

        if (radioactivity)
        {
            amount *= 2;
        }

        currentHunger = Mathf.Max(currentHunger - amount, 0);
        UpdateHungerUI();
    }

    public void RecoverHunger(float amount)
    {
        currentHunger = Mathf.Min(currentHunger + amount, maxHunger);
        UpdateHungerUI();
    }

    private void UpdateHungerUI()
    {
        if (hungerBarSlider != null)
        {
            hungerBarSlider.value = currentHunger / maxHunger;
        }
    }

    private void TriggerDeath()
    {
        if (hpBar != null)
        {
            hpBar.SetActive(false);
        }
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }
}