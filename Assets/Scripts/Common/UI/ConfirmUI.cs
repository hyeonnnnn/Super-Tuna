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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
