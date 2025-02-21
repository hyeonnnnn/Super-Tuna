using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIController : MonoBehaviour
{
    private int currentTime_Second = 0;
    private int currentTime_Minute = 0;
    private float currentTime = 0;
    private bool isGameOver = false;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider HungerGauge;
    [SerializeField] private Slider DashGauge;

    private void Start()
    {
        HungerSystem.OnHungerChanged += UpdateHungerGaugeUI;
        PlayerMove.OnDashGaugeChanged += UpdateDashGaugeUI;
    }

    private void Update()
    {
        if(!isGameOver)
            UpdateTimerTextUI();
        HandleInput();
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
}
