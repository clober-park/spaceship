using Doozy.Runtime.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingPopup : MonoBehaviour
{
    [SerializeField] List<PersnalRank> persnalRanks;

    Ranking ranking = new Ranking();
    string SaveFileName = "Ranking.json";
    int maxrank = 10;
    public void ShowRanking()
    {
        var list = ranking.GetRankingList();
        for (int i = 0; i < list.Count; i++)
        {
            persnalRanks[i].SetRank(i + 1, list[i]);
            persnalRanks[i].ShowRank();
        }
    }
    public void HideRanking()
    {
        foreach(var v in persnalRanks)
        {
            v.HideRank();
        }
    }
}
