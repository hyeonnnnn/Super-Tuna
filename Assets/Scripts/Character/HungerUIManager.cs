using UnityEngine;
using UnityEngine.UI;

public class HungerUIManager : MonoBehaviour
{
    [SerializeField] private Slider hungerBarSlider;
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private PlayerHunger hungerSystem;

    private void Start()
    {
        InitializeUI();

        if (hungerSystem != null)
        {
            hungerSystem.OnHungerChanged += UpdateHungerUI;
            hungerSystem.OnDeath += HandleGameOverUI;
        }
    }

    private void OnDestroy()
    {
        if (hungerSystem != null)
        {
            hungerSystem.OnHungerChanged -= UpdateHungerUI;
            hungerSystem.OnDeath -= HandleGameOverUI;
        }
    }

    private void InitializeUI()
    {
        if (hungerBarSlider != null)
        {
            hungerBarSlider.maxValue = 1;
            UpdateHungerUI(1, 1);
        }
    }

    private void UpdateHungerUI(float currentHunger, float maxHunger)
    {
        if (hungerBarSlider != null)
        {
            hungerBarSlider.value = currentHunger / maxHunger;
        }
    }

    private void HandleGameOverUI()
    {
        if (hpBar != null) hpBar.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(true);
    }
}