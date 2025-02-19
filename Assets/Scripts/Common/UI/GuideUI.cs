using Palmmedia.ReportGenerator.Core.Parser.Filtering;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GuideUI : BaseUI
{
    public int currentPage = 1;
    public const int maxPage = 3;

    public int CurrentPage
    {
        get { return currentPage; }

        set
        {
            currentPage = value;

            PrevBtn.interactable = true; ;
            NextBtn.interactable = true;

            if(currentPage == 1)
                PrevBtn.interactable = false;
            
            if(currentPage == maxPage)
                NextBtn.interactable = false;
        }
    }

    [SerializeField] private Button PrevBtn;
    [SerializeField] private Button NextBtn;

    public override void Init(Transform canvas)
    {
        base.Init(canvas);
    }

    public void OnClickPrevBtn()
    {
        if (CurrentPage > 1)
            CurrentPage--;
    }   
    
    public void OnClickNextBtn()
    {
        if (CurrentPage < maxPage)
            CurrentPage++;
    }
}
