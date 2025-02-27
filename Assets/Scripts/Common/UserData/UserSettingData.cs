using System;
using UnityEngine;

public class UserSettingData : IUserData
{
    public bool IsBGMEnable { get; set; }
    public bool IsSFXEnable { get; set; }
    public float CurrentVolume { get; set; }

    public void SetDefaultData()
    {
        IsBGMEnable = true;
        IsSFXEnable = true;
        CurrentVolume = 0.5f;
    }

    public bool LoadData()
    {
        bool result = false;
        try
        {
            IsBGMEnable = (PlayerPrefs.GetInt(nameof(IsBGMEnable)) == 1) ? true : false;
            IsSFXEnable = (PlayerPrefs.GetInt(nameof(IsSFXEnable)) == 1) ? true : false;
            CurrentVolume = (PlayerPrefs.GetFloat("CurrentVolume"));

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return result;
    }

    public bool SaveData()
    {
        bool result = false;
        try
        {
            PlayerPrefs.SetInt(nameof(IsBGMEnable), IsBGMEnable ? 1 : 0);
            PlayerPrefs.SetInt(nameof(IsSFXEnable), IsBGMEnable ? 1 : 0);
            PlayerPrefs.SetFloat("CurrentVolume", CurrentVolume);

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return result;
    }
}
