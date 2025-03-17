using System;
using System.Collections.Generic;
using UnityEngine;

public class UserRankingData : IUserData
{
    private static readonly int RankCount = 10;
    public List<int> SavedRanking = new List<int>(new int[RankCount]);

    public void SetDefaultData()
    {
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
            for (int i = 0; i < RankCount; i++)
            {
                string key = "Ranking" + (i + 1);
                SavedRanking[i]= PlayerPrefs.GetInt(key, 0);
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
            for (int i = 0; i < RankCount; i++)
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

    public bool SaveData(int survivedTime)
    {
        bool result = false;
        try
        {
            SavedRanking.Add(survivedTime);
            SavedRanking.Sort((a, b) => b.CompareTo(a));
            SavedRanking.RemoveAt(SavedRanking.Count - 1);

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
}
