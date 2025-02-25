using UnityEngine;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
    [SerializeField] private Slider slider;

    public override void Init(Transform Canvas)
    {
        base.Init(Canvas);
        slider.value = AudioManager.Instance.GetAllVolume();
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
}
