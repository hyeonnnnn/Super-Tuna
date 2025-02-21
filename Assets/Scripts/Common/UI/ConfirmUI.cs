using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EConfirmType
{
    OK,
    OK_CANCEL
}

public class ConfirmUIData : BaseUIData
{
    public EConfirmType ConfirmType;
    public string TitleText;
    public string DescriptionText;
    public string OkButtonText;
    public Action ActionOnClickOkButton;
    public string CancelButtonText;
    public Action ActionOnClickCancelButton;
}

public class ConfirmUI : BaseUI
{
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI DescriptionText;
    public Button OkButton;
    public Button CancelButton;
    public TextMeshProUGUI OkButtonText;
    public TextMeshProUGUI CancelButtonText;

    private ConfirmUIData _confirmUiData;
    private Action _actionOnClickOkButton;
    private Action _actionOnClickCancelButton;

    public override void SetData(BaseUIData data)
    {
        base.SetData(data);
        Time.timeScale = 0f;

        _confirmUiData = data as ConfirmUIData;

        TitleText.text = _confirmUiData.TitleText;
        DescriptionText.text = _confirmUiData.DescriptionText;
        OkButtonText.text = _confirmUiData.OkButtonText;
        _actionOnClickOkButton = _confirmUiData.ActionOnClickOkButton;
        CancelButtonText.text = _confirmUiData.CancelButtonText;
        _actionOnClickCancelButton = _confirmUiData.ActionOnClickCancelButton;

        OkButton.gameObject.SetActive(true);
        if (_confirmUiData.ConfirmType == EConfirmType.OK_CANCEL)
        {
            CancelButton.gameObject.SetActive(true);
        }
        else
        {
            CancelButton.gameObject.SetActive(false);
        }
    }

    public void ProcessOk()
    {
        _actionOnClickOkButton?.Invoke();
        _actionOnClickOkButton = null;

        Time.timeScale = 1f;
        Close();
    }

    public void ProcessCancel()
    {
        _actionOnClickCancelButton?.Invoke();
        _actionOnClickCancelButton = null;

        Time.timeScale = 1f;
        Close();
    }
}
