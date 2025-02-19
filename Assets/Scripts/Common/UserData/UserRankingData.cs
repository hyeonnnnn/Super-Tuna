using System;
using System.Collections.Generic;
using UnityEngine;

public class UserRankingData : IUserData
{
    private const int RankCount = 10;
    public int [] SavedRanking = new int[RankCount];
    
    public void SetDefaultData()
    {
        SavedRanking = new int[RankCount];

        for(int i = 0; i < RankCount; i++)
        {
            SavedRanking[i] = 0;
        }
    }

    public bool LoadData()
    {
        bool result = false;

        try
        {
            for (int i = 0; i <= RankCount; i++)
            {
                string key = "Ranking" + (i + 1);
                SavedRanking[i] = PlayerPrefs.GetInt(key, 0);
            }

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
            for (int i = 0; i <= RankCount; i++)
            {
                string key = "Ranking" + (i + 1);
                PlayerPrefs.SetInt(key, SavedRanking[i]);
                PlayerPrefs.Save();

                result = true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return result;
    }

    public int GetRankScore(int rank) { return SavedRanking[rank]; }
    
    //오류 체크용
    public void SetTempData()
    {
        for (int i = 0; i < RankCount;i++)
        {
            SavedRanking[i] = i;
        }
    }
}
