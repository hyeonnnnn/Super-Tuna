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
        for (int i = 0; i< rankingScore.Length; i++)
        {
            rankingScore[i].text = UserDataManager.Instance.GetUserData<UserRankingData>().GetRankScore(i).ToString();
        }
    }
}
