using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    //public static GameOverPanel instance;
    public Text txt_Score, txt_BestScore, txt_AddDiamondCount;
    public Button btn_Restart, btn_Rank, btn_Home;
    public Image img_New;
    public GameObject Btn_ad;
    private AdManager Ad_Manager;
    //private GpgsScript GooglePlayServices;
    private string bestScore = "Best score";
    private void Awake()
    {
        //instance = this;
        
        btn_Restart.onClick.AddListener(OnRestartButtonclick);
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Home.onClick.AddListener(OnHomeButtonClick);
        EventCenter.AddListener(EventDefine.ShowGameOverPanel, Show);

        Ad_Manager = GameObject.FindGameObjectWithTag("AdManager").GetComponent<AdManager>();
        //GooglePlayServices = GameObject.FindGameObjectWithTag("GpGs").GetComponent<GpgsScript>();
        gameObject.SetActive(false);

        DontDestroyOnLoad(this.gameObject);

    }
    private void Start()
    {


        int RandoomforAd = Random.Range(0, 2);
        if(RandoomforAd == 1)
        {
            if (!GameManager.Instance.vars.RemoveAds)
            {

                //----------------------------------------------------------------------------Advertisement INTERSITIAL_AD
                Ad_Manager.DisplayIntersitialAd();
                //UnityAdManager.ShowStandarAd();
                //----------------------------------------------------------------------------Advertisement INTERSITIAL_AD
                print("anounss");
            }

        }
        GameManager.Instance.GooglePlayServices.addScoreLeaderBoard(GameManager.Instance.GetGameScore());

        
        //Btn_ad.SetActive(true);       
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGameOverPanel, Show);
    }
    private void Show()
    {
        //Debug.Log("-------------------------------------------------------------------------------------------");
        //bestScore = GameManager.Instance.TextLenguaje[26];
        GameManager.Instance.changeLenguaje();

        //Debug.Log(GameManager.Instance.TextLenguaje.Count);
        if (GameManager.Instance.GetGameScore() > GameManager.Instance.GetBestScore())
        {
            img_New.gameObject.SetActive(true);
            txt_BestScore.text = bestScore + ":  " + GameManager.Instance.GetGameScore();
        }
        else
        {
            img_New.gameObject.SetActive(false);
            txt_BestScore.text = bestScore + ":  " + GameManager.Instance.GetBestScore();
        }
        GameManager.Instance.SaveScore(GameManager.Instance.GetGameScore());
        txt_Score.text = GameManager.Instance.GetGameScore().ToString();
        txt_AddDiamondCount.text = "+" + GameManager.Instance.GetGameDiamond().ToString();
        //更新总的钻石数量
        GameManager.Instance.UpdateAllDiamond(GameManager.Instance.GetGameDiamond());
        //Ad_Manager.DestroyBanner();
        gameObject.SetActive(true);
        
    }
    /// <summary>
    /// 再来一局按钮点击
    /// </summary>
    private void OnRestartButtonclick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameManager.Instance.changeLenguaje();
        GameData.IsAgainGame = true;
        //if(GameManager.Instance.MenuMusic.clip !=)
    }
    /// <summary>
    /// 排行榜按钮点击
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }
    /// <summary>
    /// 主界面按钮点击
    /// </summary>
    private void OnHomeButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = false;
        //GameManager.Instance.changeLenguaje();
    }
    public void ChangeLanguageGameOverPanel()
    {
        bestScore = GameManager.Instance.TextLenguaje[26];
        btn_Restart.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.TextLenguaje[27];
    }
    
}
