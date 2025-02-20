using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUiController : MonoBehaviour
{
    public void Init()
    {
        //
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.Instance.Play(AudioType.SFX, "ui_button_click");

            var frontUI = UIManager.Instance.GetFrontUI();
            if(frontUI = null)
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
        //var data = new ConfirmUIData()
        {

        }
    }

    public void OnClickSettingButton()
    {
        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingUI>(uiData);
    }

    public void OnClickPlayButton()
    {
        Debug.Log("∞‘¿” æ¿ ¿Ãµø");
        //SceneManager.LoadScene("GameScene");
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
        Application.Quit();
    }
}
