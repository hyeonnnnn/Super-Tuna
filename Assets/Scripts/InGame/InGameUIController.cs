using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

    private static readonly float fadeSpeed = 0.5f;
    private static readonly float minAlpha = 0f;
    private static readonly float maxAlpha = 40f/255f;

    #region GamePlayUIs
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider HungerGauge;
    [SerializeField] private Slider ExpGauge;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider DashGauge;
    [SerializeField] private Image RadiationEffect;
    [SerializeField] private TextMeshProUGUI mapText;
    [SerializeField] private Animator MapInfoUI;
    #endregion

    #region GameEndUIs
    [SerializeField] private GameObject GameOverInfo;
    [SerializeField] private TextMeshProUGUI DyingReasonText;
    [SerializeField] private TextMeshProUGUI GameOverTimerText;
    #endregion

    private Coroutine RadiationEffectAlphaChange;
    private Coroutine Fading;

    public void Init()
    {

    }

    private void Start()
    {
        HungerSystem.OnHungerChanged += UpdateHungerGaugeUI;
        HungerSystem.ShowResult += GameOver;
        PlayerMove.OnDashGaugeChanged += UpdateDashGaugeUI;
        Growth.OnExpGaugeChanged += UpdateExphGaugeUI;
        Growth.OnLevelChanged += UpdateLevelTextUI;
        RadiationSystem.OnRadiationEnter += RadiationUIChanged;
        DepthTracker.OnLayerChanged += ShowMapText;
    }

    private void Update()
    {
        if (!isGameOver)
            UpdateTimerTextUI();
        HandleInput();
    }

    private void OnDestroy()
    {
        HungerSystem.OnHungerChanged -= UpdateHungerGaugeUI;
        HungerSystem.ShowResult -= GameOver;
        PlayerMove.OnDashGaugeChanged -= UpdateDashGaugeUI;
        Growth.OnExpGaugeChanged -= UpdateExphGaugeUI;
        Growth.OnLevelChanged -= UpdateLevelTextUI;
        RadiationSystem.OnRadiationEnter -= RadiationUIChanged;
        DepthTracker.OnLayerChanged -= ShowMapText;
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

    public void UpdateDashGaugeUI(float currentDash, float maxDash)
    {
        DashGauge.value = (currentDash / maxDash);
    }

    public void RadiationUIChanged(bool OnRadiation)
    {
        if (OnRadiation)
        {
            if (RadiationEffectAlphaChange == null)
            {
                RadiationEffectAlphaChange = StartCoroutine(ChangeRadiationEffectAlpha());
            }
        }
        else
        {
            if (RadiationEffectAlphaChange != null)
            {
                StopCoroutine(RadiationEffectAlphaChange);
                StopCoroutine(Fading);
                RadiationEffectAlphaChange = null;
            }

            Color color = RadiationEffect.color;
            color.a = 0f / 255f;
            RadiationEffect.color = color;
        }
    }

    IEnumerator ChangeRadiationEffectAlpha()
    {
        Color color = RadiationEffect.color;

        while (true)
        {
            yield return Fading = StartCoroutine(FadeTo(maxAlpha)); // ���� �����
            yield return Fading = StartCoroutine(FadeTo(minAlpha)); // ���� ��ο���
        }
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        Color color = RadiationEffect.color;
        float startAlpha = color.a;
        float time = 0f;

        while (Mathf.Abs(color.a - targetAlpha) > 0.01f)
        {
            time += Time.deltaTime * fadeSpeed;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, time);
            RadiationEffect.color = color;
            yield return null;
        }

        color.a = targetAlpha; // ��Ȯ�� �� ����
        RadiationEffect.color = color;
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
        UserDataManager.Instance.GetUserData<UserRankingData>().SaveData((int)currentTime);
        SetGameOverTimerTextUI();
        GameOverInfo.SetActive(true);
    }

    public void SetDyingReasonTextUI(DyingReason dyingReason)
    {
        string reasonText = dyingReason switch
        {
            DyingReason.Hunger => "배고픔",
            DyingReason.Enemy => "적에게 의해",
            DyingReason.Mine => "지뢰에 의해",
            DyingReason.Radiation => "방사능에 의해",
            _ => "알 수 없는 이유"
        };
    
        DyingReasonText.text = $"슈퍼 참치는 {reasonText} 죽었습니다..";
    }


    public void OnClickGameOverLobby()
    {
        SceneLoader.Instance.LoadSceneAsync(ESceneType.Lobby);
    }

    public void OnCliCkGameOverRestart()
    {
        SceneLoader.Instance.ReloadScene();
    }

    public void ShowMapText(string currentMap)
    {
        if (!IsAnimationPlaying(MapInfoUI, "MapUI_ComingDown"))
        {
            mapText.text = currentMap;
            MapInfoUI.Play("MapUI_ComingDown",0, 0f);
        }
    }

    bool IsAnimationPlaying(Animator animator, string animationName)
    {
        if (animator == null) return false;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1.0f;
    }
}
