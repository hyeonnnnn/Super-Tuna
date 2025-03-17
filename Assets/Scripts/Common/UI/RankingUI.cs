using Palmmedia.ReportGenerator.Core.Parser.Filtering;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI[] rankingScore;

    public override void Init(Transform canvas)
    {
        base.Init(canvas);
        UpdateRankingText();
    }

    public void UpdateRankingText()
    {
        int survivedTime, survivedTime_Second, survivedTime_Minute;

        for (int i = 0; i< rankingScore.Length; i++)
        {
            survivedTime = UserDataManager.Instance.GetUserData<UserRankingData>().GetRankScore(i);
            
            survivedTime_Second = (int)survivedTime % 60;
            survivedTime_Minute = ((int)survivedTime / 60) % 100;

            if (survivedTime_Minute > 99)
            {
                survivedTime_Minute = 99;
                survivedTime_Second = 59;
            }

            rankingScore[i].text = $"{survivedTime_Minute:D2} : {survivedTime_Second:D2}";
        }
    }
}
