using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum DyingReason
{
    Hunger,
    Enemy,
    Mine,
    Radiation
}

public class InGameUIController : MonoBehaviour
{
    private int currentTime_Second = 0;
    private int currentTime_Minute = 0;
    private float currentTime = 0;
    private bool isGameOver = false;

    //게임 플레이시 UI
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider HungerGauge;
    [SerializeField] private Slider ExpGauge;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider DashGauge;
    
    //게임 오버시 UI
    [SerializeField] private GameObject GameOverInfo;
    [SerializeField] private TextMeshProUGUI DyingReasonText;
    [SerializeField] private TextMeshProUGUI GameOverTimerText;

    public void Init()
    {

    }

    private void Start()
    {
        HungerSystem.OnHungerChanged += UpdateHungerGaugeUI;
        HungerSystem.OnDeath += GameOver;
        PlayerMove.OnDashGaugeChanged += UpdateDashGaugeUI;
        Growth.OnExpGaugeChanged += UpdateExphGaugeUI;
        Growth.OnLevelChanged += UpdateLevelTextUI;
    }

    private void Update()
    {
        if(!isGameOver)
            UpdateTimerTextUI();
        HandleInput();
    }

    private void OnDestroy()
    {
        HungerSystem.OnHungerChanged -= UpdateHungerGaugeUI;
        HungerSystem.OnDeath -= GameOver;
        PlayerMove.OnDashGaugeChanged -= UpdateDashGaugeUI;
        Growth.OnExpGaugeChanged -= UpdateExphGaugeUI;
        Growth.OnLevelChanged -= UpdateLevelTextUI;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.Play(AudioType.SFX, "ui_button_click");

            var frontUI = UIManager.Instance.GetFrontUI();
            if (frontUI != null)
            {
                frontUI.Close();
                Time.timeScale = 1f;
            }
            else
            {
                ShowQuitConfirmUI();
            }
        }
    }

    private void ShowQuitConfirmUI()
    {
        var data = new ConfirmUIData()
        {
            ConfirmType = EConfirmType.OK_CANCEL,
            TitleText = "Exit",
            DescriptionText = "Do you want to go Lobby?",
            OkButtonText = "Ok",
            CancelButtonText = "Cancel",
            ActionOnClickOkButton = () => SceneLoader.Instance.LoadScene(ESceneType.Lobby)
        };
        UIManager.Instance.OpenUI<ConfirmUI>(data);
    }

    public void OnClickInGameSettingButton()
    {
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<InGameSettingUI>(uiData);
    }

    public void UpdateHungerGaugeUI(int currentHunger, int maxHunger)
    {
        HungerGauge.value = ((float)currentHunger / maxHunger);
    }

    public void UpdateExphGaugeUI(int currentExp, int maxExp)
    {
        ExpGauge.value = ((float)currentExp / maxExp);
    }

    public void UpdateLevelTextUI(int currentLevel)
    {
        levelText.text = $"Level {currentLevel}";
    }

    public void UpdateDashGaugeUI(float currentDash,float maxDash)
    {
        DashGauge.value = (currentDash / maxDash);
    }

    public void UpdateTimerTextUI()
    {
        currentTime += Time.deltaTime;
        currentTime_Second = (int)currentTime % 60;
        currentTime_Minute = ((int)currentTime / 60) % 100;
        
        if (currentTime_Minute > 99)
        {
            currentTime_Minute = 99;
            currentTime_Second = 59;
        }

        timerText.text = $"{currentTime_Minute:D2} : {currentTime_Second:D2}";
    }

    public void SetGameOverTimerTextUI()
    {
        GameOverTimerText.text = $"{currentTime_Minute:D2} : {currentTime_Second:D2}";
    }

    public void GameOver(DyingReason dyingReason)
    {
        Time.timeScale = 0f;
        SetDyingReasonTextUI(dyingReason);
        SetGameOverTimerTextUI();
        GameOverInfo.SetActive(true);
    }

    public void SetDyingReasonTextUI(DyingReason dyingReason)
    {
        DyingReasonText.text = $"Super Tuna died by {dyingReason.ToString()}..";
    }

    public void OnClickGameOverLobby()
    {
        SceneLoader.Instance.LoadScene(ESceneType.Lobby);
    }

    public void OnCliCkGameOverRestart()
    {
        SceneLoader.Instance.ReloadScene();
    }
}
