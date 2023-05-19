using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingJsonTest : MonoBehaviour
{
    Ranking ranking;
    [SerializeField]List<InputField> inputFields;
    [SerializeField]Text ranktext;
    // Start is called before the first frame update
    void Start()
    {
        ranking = GetComponent<Ranking>();
    }


    //public void OnclickSave()
    //{
    //    foreach (var inputField in inputFields)
    //    {
    //        if (inputField.text == "")
    //            return;
    //    }
    //    PersonalInfo info = new PersonalInfo();
    //    info.name = inputFields[0].text;
    //    info.time = int.Parse(inputFields[1].text);
    //    info.kill = int.Parse(inputFields[2].text);
    //    for(int i = 0; i < inputFields.Count; i++)
    //    {
    //        inputFields[i].text = "";
    //    }
    //    ranking.NewRecord(info);
    //    ranking.SaveJson();
    //}
    //public void OnclickLoad()
    //{
    //    ranking.LooadJson();
    //    List<PersonalInfo> list = ranking.RankingList;
    //    if (list == null)
    //        return;
    //    ranktext.text = "";
    //    int rank= 1;
    //    foreach (var info in list)
    //    {
    //        ranktext.text += string.Format("Rank : {0} Name : {1} Time : {2} Kill : {3}\n", rank, info.name, info.time, info.kill);
    //        rank++;
    //    }
    //}
}
