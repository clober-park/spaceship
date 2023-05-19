using Fusion;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class PersonalInfo
{
    public string name;
    public float time;
    public int score;
}
public class Ranking : NetworkBehaviour
{
    [SerializeField] string SavePath = "C:\\Users\\YCS\\AppData\\LocalLow\\DefaultCompany\\My project\\saves";
    [SerializeField] string SaveFileName  = "\\Ranking.json";
    [SerializeField] string MultySaveFileName  = "\\MultyRanking.json";
    [SerializeField] int maxrank = 10;

    List<PersonalInfo> rankingList = new List<PersonalInfo>();
    void SaveJson()
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        RankingSorting();
        string json = JsonConvert.SerializeObject(rankingList);
        if (Runner.ActivePlayers.Count() > 1)
            File.WriteAllText(SavePath + MultySaveFileName, json);
        else
            File.WriteAllText(SavePath + SaveFileName, json);

    }
    void LooadJson()
    {
        string sfn;
        Runner = FindAnyObjectByType<NetworkRunner>();
        if (Runner.ActivePlayers.Count() > 1)
            sfn = MultySaveFileName;
        else
            sfn = SaveFileName;

        if (!File.Exists(SavePath + sfn))
            return;
        string json = File.ReadAllText(SavePath + sfn);
        rankingList = JsonConvert.DeserializeObject<List<PersonalInfo>>(json);
        RankingSorting();
    }
    void RankingSorting()
    {
        rankingList = rankingList.OrderByDescending(x => x.time)
                                  .ThenByDescending(x => x.score)
                                  .ToList();
    }

    public  void RecordRanking(PersonalInfo challenger, bool ismulty = false)
    {
        LooadJson();
        if (rankingList.Count < maxrank)
        {
            rankingList.Add(challenger);
        }
        else
        {
            List<PersonalInfo> newRank = new List<PersonalInfo>();
            foreach (var info in rankingList)
            {
                if (info.time < challenger.time ||
                    (info.time == challenger.time && info.score < challenger.score))
                {
                    newRank.Add(challenger);
                    challenger = info;
                    continue;
                }
                newRank.Add(info);

            }
            rankingList = newRank;
        }
        SaveJson();
    }

    public List<PersonalInfo> GetRankingList()
    {
        LooadJson();
        return rankingList;
    }

    public int GetChallengerRank(PersonalInfo challenger)
    {
        int rank = 1;
        LooadJson();
        foreach (var info in rankingList)
        {
            if (info.time < challenger.time ||
                (info.time == challenger.time && info.score < challenger.score))
            {
                break;
            }
            rank++;
        }
        return rank;
    }

    public bool InRanking( PersonalInfo challenger, out int rank)
    {
        rank = GetChallengerRank(challenger);
        return rank <= maxrank;
    }
}
