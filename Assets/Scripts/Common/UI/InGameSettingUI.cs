using UnityEngine;
using UnityEngine.UI;

public class InGameSettingUI : BaseUI
{
    [SerializeField] private Slider slider;

    public override void Init(Transform Canvas)
    {
        base.Init(Canvas);
        Time.timeScale = 0f;
        slider.value = AudioManager.Instance.GetAllVolume();
    }

    public override void Close(bool isCloseAll = false)
    {
        base.Close(isCloseAll);
        Time.timeScale = 1f;
    }

    public void OnSoundSliderChanged()
    {
        AudioManager.Instance.SetAllVolume(slider.value);
    }

    public void OnClickBGMOnOffBtn()
    {
        AudioManager.Instance.ChangeBGMState();
    }

    public void OnClickSFXOnOffBtn()
    {
        AudioManager.Instance.ChangeSFXState();
    }

    public void OnClickGoToLobbyButton()
    {
        SceneLoader.Instance.LoadScene(ESceneType.Lobby);
    }
}
