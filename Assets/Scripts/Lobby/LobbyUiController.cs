using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUiController : MonoBehaviour
{
    public void Init()
    {
        //
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.Play(AudioType.SFX, "ui_button_click");

            var frontUI = UIManager.Instance.GetFrontUI();
            if(frontUI != null)
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
            TitleText = "Quit",
            DescriptionText = "Do you want to quit game?",
            OkButtonText = "Quit",
            CancelButtonText = "Cancel",
            ActionOnClickOkButton = () => Application.Quit()
        };
        UIManager.Instance.OpenUI<ConfirmUI>(data);
    }

    public void OnClickSettingButton()
    {
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingUI>(uiData);
    }

    public void OnClickPlayButton()
    {
        SceneLoader.Instance.LoadScene(ESceneType.InGame);
    }

    public void OnClickRankingButton()
    {
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<RankingUI>(uiData);
    }

    public void OnClickGuideButton()
    {
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<GuideUI>(uiData);
    }

    public void OnClickExitButton()
    {
        ShowQuitConfirmUI();
    }
}
