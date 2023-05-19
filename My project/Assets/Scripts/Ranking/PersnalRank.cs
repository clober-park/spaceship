using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersnalRank : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rank;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI score;
    UIView uiview;

    private void Start()
    {
        uiview = GetComponent<UIView>();    
    }

    public void SetRank(int num, PersonalInfo info)
    {
        rank.text = num.ToString();
        name.text = info.name;
        time.text = string.Format("{0:0.#}", info.time);
        score.text = info.score.ToString();
    }

    public void ShowRank()
    {
        uiview.Show();
    }
    public void HideRank()
    {
        uiview.Hide();
    }
}
