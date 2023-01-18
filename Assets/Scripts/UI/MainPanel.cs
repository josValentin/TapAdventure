using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    private Button btn_Start;
    private Button btn_Shop;
    private Button btn_Rank;

    private Button btn_Help;
    private Button btn_Store;
    private Button btn_Settings;
    private ManagerVars vars;


    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.ShowMainPanel, Show);
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);

        
        Init();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowMainPanel, Show);
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 皮肤更换，这里更换UI皮肤图片
    /// </summary>
    /// <param name="skinIndex"></param>
    private void ChangeSkin(int skinIndex)
    {
        btn_Shop.transform.GetChild(0).GetComponent<Image>().sprite =
            vars.skinSpriteList[skinIndex];
    }
    private void Start()
    {
        if (GameData.IsAgainGame)
        {
            EventCenter.Broadcast(EventDefine.ShowGamePanel);
            gameObject.SetActive(false);
        }
        Sound();
        Music();
        
        
        ChangeSkin(GameManager.Instance.GetCurrentSelectedSkin());

    }
    private void Init()
    {
        btn_Start = transform.Find("btn_Start").GetComponent<Button>();
        btn_Start.onClick.AddListener(OnStartButtonClick);
        btn_Shop = transform.Find("Btns/btn_Shop").GetComponent<Button>();
        btn_Shop.onClick.AddListener(OnShopButtonClick);
        btn_Rank = transform.Find("Btns/btn_Rank").GetComponent<Button>();
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Settings = transform.Find("Btns/btn_Settings").GetComponent<Button>();
        btn_Settings.onClick.AddListener(OnSettingsButtonClick);
        btn_Store = transform.Find("Btns/btn_Store").GetComponent<Button>();
        btn_Store.onClick.AddListener(OnStoreButtonClick);
        btn_Help = transform.Find("btn_Help").GetComponent<Button>();
        btn_Help.onClick.AddListener(OnResetButtonClick);
    }
    /// <summary>
    /// 开始按钮点击后调用此方法
    /// </summary>
    private void OnStartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        GameManager.Instance.IsGameStarted = true;
        EventCenter.Broadcast(EventDefine.ShowGamePanel);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 商店按钮点击
    /// </summary>
    private void OnShopButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ShowShopPanel);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 排行榜按钮点击
    /// </summary>
    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }
    private void OnStoreButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ShowStorePanel);
    }
    private void OnSettingsButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ShowSettingsPanel);
    }

    /// <summary>
    /// 音效按钮点击
    /// </summary>

    public void Sound()
    {

        EventCenter.Broadcast(EventDefine.IsMusicOn, GameManager.Instance.GetIsMusicOn());

        EventCenter.Broadcast(EventDefine.UpdateSliderBarSound, GameManager.Instance.GetSoundValue());
    }

    public void Music()
    {
        EventCenter.Broadcast(EventDefine.IsMainGameMusicOn, GameManager.Instance.GetIsMainGameMusicOn());
        EventCenter.Broadcast(EventDefine.UpdateSliderBarMusic, GameManager.Instance.GetMusicValue());
    }
    /// <summary>
    /// 重置按钮点击
    /// </summary>
    private void OnResetButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ShowHelpPanel);
    }
}
