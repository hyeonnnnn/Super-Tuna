using UnityEngine;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
    [SerializeField] private Slider slider;

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
}
