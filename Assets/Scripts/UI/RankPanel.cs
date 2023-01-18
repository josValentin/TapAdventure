using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Michsky.UI.ModernUIPack;

public class RankPanel : MonoBehaviour
{
    //public static RankPanel instance;
    private Button btn_Close;
    public Text[] txt_Scores;
    private GameObject go_ScoreList;
    public ButtonManagerBasic btn_leaderboard;
    private string WorldRank = "World Rank";
    private void Awake()
    {
        //instance = this;
        EventCenter.AddListener(EventDefine.ShowRankPanel, Show);
        btn_Close = transform.Find("btn_Close").GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);
        go_ScoreList = transform.Find("ScoreList").gameObject;

        btn_Close.GetComponent<Image>().color = new Color(btn_Close.GetComponent<Image>().
            color.r, btn_Close.GetComponent<Image>().color.g, btn_Close.GetComponent<Image>().color.b, 0);
        go_ScoreList.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
        //DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankPanel, Show);
    }
    private void Show()
    {

        gameObject.SetActive(true);
        btn_leaderboard.buttonText = WorldRank;
        btn_leaderboard.normalText.text = WorldRank;
        btn_Close.GetComponent<Image>().DOColor(new Color(btn_Close.GetComponent<Image>().
            color.r, btn_Close.GetComponent<Image>().color.g,
            btn_Close.GetComponent<Image>().color.b, 0.3f), 0.3f);
        go_ScoreList.transform.DOScale(Vector3.one, 0.3f);

        int[] arr = GameManager.Instance.GetScoreArr();
        for (int i = 0; i < arr.Length; i++)
        {
            txt_Scores[i].text = arr[i].ToString();
        }
    }
    private void OnCloseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        btn_Close.GetComponent<Image>().DOColor(new Color(btn_Close.GetComponent<Image>().
            color.r, btn_Close.GetComponent<Image>().color.g,
            btn_Close.GetComponent<Image>().color.b, 0), 0.3f);
        go_ScoreList.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void ChangeLanguageRankPanel()
    {
        WorldRank = GameManager.Instance.TextLenguaje[5];
    }
}
