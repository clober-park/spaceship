using Doozy.Runtime.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordRanking : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rank;
    [SerializeField] TMP_InputField inputname;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] GameManager gm;

    public event Action RecordCompleteHandler;
    public event Action OutRankingHandler;

    PersonalInfo myinfo = new PersonalInfo();
    Ranking ranking = new Ranking();

    public void RecordRank()
    {
        myinfo.name = inputname.text;
        ranking.RecordRanking(myinfo);
    }
    public void Init()
    {
        myinfo.time = gm.gameTime;
        myinfo.score = gm.Score;
        if (ranking.InRanking(myinfo, out int rank))
        {
            RecordUI(rank);
        }
        else
        {
            OutRankingHandler?.Invoke();
        }
    }

    public void RecordUI(int rank)
    {
        this.rank.text = rank.ToString();
        time.text = string.Format("{0:0.#}", myinfo.time);
        score.text = myinfo.score.ToString();
    }

    public void OnclickRecord()
    {
        RecordRank();
        RecordCompleteHandler?.Invoke();
    }
}
