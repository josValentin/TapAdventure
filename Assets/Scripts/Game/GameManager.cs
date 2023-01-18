using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using DG.Tweening;
using UnityEngine.Networking;
using System;
using System.Collections;
using NiobiumStudios;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameData data;
    public ManagerVars vars;
    [HideInInspector]
    public GpgsScript GooglePlayServices;

    //
    [SerializeField]
    private CanvasScaler _mainCanvasScaler;
    [SerializeField]
    private RectTransform _CharactersInStore;
    [SerializeField]
    private RectTransform _MainBtns;

    public bool IsGameStarted { get; set; }

    public bool IsGameOver { get; set; }
    public bool IsPause { get; set; }

    public bool PlayerIsMove { get; set; }

    private int gameScore;
    private int gameDiamond;


    private bool isFirstGame;
    private bool isMusicOn;
    private bool isMainGameMusicOn;
    private float SoundValue;
    private float MusicValue;


    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    private int LanguageIndex;

    private float ComparedSaveCount;

    public AudioSource MenuMusic;

    public string GameVersion;

    public bool TryLoadFromCloud = false;

    [HideInInspector]
    public bool RemoveAds;

    public DailyRewardsInterface RewardsInterface;

    //
    [SerializeField]
    private RankPanel _rankPanel;
    [SerializeField]
    private HelpPanel _helpPanel;
    [SerializeField]
    private GameOverPanel _gameOverPanel;
    //Lenguajes

    [HideInInspector]
    public List<string> TextLenguaje = new List<string>();

    [Header("Select languaje")]
    public List<string> French = new List<string>();
    public List<string> Turkish = new List<string>();
    public List<string> Deutsch = new List<string>();
    public List<string> English = new List<string>();
    public List<string> Spanish = new List<string>();

    [HideInInspector]
    public bool SetLenguageFirstTime = false;

    public void changeLenguaje()
    {
        switch (LenguajeDropDown.instance.lenguaje)
        {
            case LenguajeDropDown.Lenguajes.French:
                TextLenguaje = French;
                Debug.Log("Lenguaje changed to French");
                break;
            case LenguajeDropDown.Lenguajes.Turkish:
                TextLenguaje = Turkish;
                Debug.Log("Lenguaje changed to Turkish");

                break;
            case LenguajeDropDown.Lenguajes.Deutsch:
                TextLenguaje = Deutsch;
                Debug.Log("Lenguaje changed to Deutsch");

                break;
            case LenguajeDropDown.Lenguajes.English:
                TextLenguaje = English;
                Debug.Log("Lenguaje changed to English");

                break;
            case LenguajeDropDown.Lenguajes.Spanish:
                TextLenguaje = Spanish;
                Debug.Log("Lenguaje changed to Spanish");

                break;
            default:
                break;
        }

        SettingsPanel.instance.changeLanguajeSettingsPanel();
        ShopPanel.instance.ChangeLanguageShopPanel();
        _rankPanel.ChangeLanguageRankPanel();
        StorePanel.instance.ChangeLanguageStorePanel();
        _helpPanel.ChangeLanguageHelpPanel();



        _gameOverPanel.ChangeLanguageGameOverPanel();
        if(RewardsInterface != null)
        {
            RewardsInterface.ChangeLanguageDailyRewardsInterface();

        }
        ResetPanel.instance.ChangeLanguageResetPanel();
        Hint.instance.ChangeLanguageHintPanel();
    }

    public void setLanguageIndex(int index)
    {
        LanguageIndex = index;
    }

    public int getLanguageIndex()
    {
        return LanguageIndex;
    }

    public void RedirectToUpdate()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.Mobile3B.TapAdventure");
    }

    public GameData GetGameData()
    {
        return data;
    }
    public void SetGameData(GameData data)
    {
        this.data = data;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        vars = ManagerVars.GetManagerVars();
        Instance = this;
        EventCenter.AddListener(EventDefine.AddScore, AddGameScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddGameDiamond);
        GooglePlayServices = GameObject.FindGameObjectWithTag("GpGs").GetComponent<GpgsScript>();

        if (GameData.IsAgainGame)
        {
            IsGameStarted = true;
        }
        InitGameData();

        screenInch();
    }


    private void screenInch()
    {
        float aspect = (float)Screen.height / (float)Screen.width; // Portrait
                                                                   //aspect = (float)Screen.width / (float)Screen.height; // Landscape
        Debug.Log("Aspect Ratio:" + aspect);
        if (aspect >= 1.87)
        {
            Debug.Log("19.5:9"); // iPhone X                  
        }
        else if (aspect >= 1.74)  // 16:9
        {
            Debug.Log("16:9");
        }
        else if (aspect > 1.6)// 5:3
            Debug.Log("5:3");
        else if (Math.Abs(aspect - 1.6) < Mathf.Epsilon)// 16:10
            Debug.Log("16:10");
        else if (aspect >= 1.5)// 3:2
            Debug.Log("3:2");
        else
        { // 4:3
            Debug.Log("4:3 or other");
            _mainCanvasScaler.matchWidthOrHeight = 1;
            _CharactersInStore.anchoredPosition = new Vector2(80, _CharactersInStore.anchoredPosition.y);
            _MainBtns.sizeDelta = new Vector2( 580 ,_MainBtns.sizeDelta.y);
        }
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddGameScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddGameDiamond);
    }

    public void SaveScore(int score)//60
    {
        List<int> list = bestScoreArr.ToList();
        //从大到小排序list
        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        //50 20 10
        int index = -1;
        for (int i = 0; i < bestScoreArr.Length; i++)
        {
            if (score > bestScoreArr[i])
            {
                index = i;
            }
        }
        if (index == -1) return;

        for (int i = bestScoreArr.Length - 1; i > index; i--)
        {
            bestScoreArr[i] = bestScoreArr[i - 1];
        }
        bestScoreArr[index] = score;

        Save();
    }

    public int GetBestScore()
    {
        return bestScoreArr.Max();
    }

    public int[] GetScoreArr()
    {
        List<int> list = bestScoreArr.ToList();
        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        return bestScoreArr;
    }

    private void PlayerMove()
    {
        PlayerIsMove = true;
    }

    private void AddGameScore()
    {
        int multyply = 1;
        if (PlayerController.Instance.WhichPlayer == 12)
        {
            multyply = 2;
        }
        gameScore += 1 * multyply;
        EventCenter.Broadcast(EventDefine.UpdateScoreText, gameScore);
        if (IsGameStarted == false || IsGameOver || IsPause)
        {
            return;
        } 

    }

    public int GetGameScore()
    {
        return gameScore;
    }

    private void AddGameDiamond()
    {
        int multyply = 1;
        if (PlayerController.Instance.WhichPlayer == 8 || PlayerController.Instance.WhichPlayer == 9)
        {
            multyply = 2;
        }
        //
        if (GetGameScore() <= 150)
        {

            gameDiamond += 1 * multyply;
        } else if (GetGameScore() > 150 && GetGameScore() <= 500)
        {

            gameDiamond += 2 * multyply;
        } else if(GetGameScore() > 500){

            gameDiamond += 5 * multyply;
        }



        EventCenter.Broadcast(EventDefine.UpdateDiamondText, gameDiamond);
    }

    public void restDiamond(int ToRest)
    {
        gameDiamond -= ToRest;
        EventCenter.Broadcast(EventDefine.UpdateDiamondText, gameDiamond);

    }

    public void adddiamondForAd()
    {
        diamondCount += 25;
        Save();
    }

    public void AddDiamonForBuy(int Amount)
    {
        diamondCount += Amount;
        Save();   
    }

    public int GetGameDiamond()
    {
        return gameDiamond;
    }

    public bool GetSkinUnlocked(int index)
    {
        return skinUnlocked[index];
    }

    public void SetSkinUnloacked(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }

    public int GetAllDiamond()
    {
        return diamondCount;
    }

    public float GetComapreSaveCount()
    {
        return ComparedSaveCount;
    }

    public void UpdateAllDiamond(int value)
    {
        diamondCount += value;
        Save();
    }
    
    public void SetSelectedSkin(int index)
    {
        selectSkin = index;
        Save();
    }

    public int GetCurrentSelectedSkin()
    {
        return selectSkin;
    }

    public void SetIsMusicOn(bool value)
    {
        isMusicOn = value;
        Save();
    }

    public void SetIsGameMainMusicOn(bool value)
    {
        isMainGameMusicOn = value;
        Save();
    }

    public void SetSoundValue(float value)
    {
        SoundValue = value;
        Save();
    }

    public void SetMusicValue(float value)
    {
        MusicValue = value;
        Save();
    }

    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }

    public bool GetIsMainGameMusicOn()
    {
        return isMainGameMusicOn;
    }

    public float GetSoundValue()
    {
        return SoundValue;
    }

    public float GetMusicValue()
    {
        return MusicValue;
    }

    private void InitGameData()
    {
        Read();
        if (data != null)
        {
            isFirstGame = data.GetIsFirstGame();
        }
        else
        {
            isFirstGame = true;
            RewardsInterface.CreateTimer();
        }
        if (isFirstGame)
        {
            isFirstGame = false;

            isMusicOn = true;
            isMainGameMusicOn = true;
            MusicValue = 0.5f;
            SoundValue = 0.5f;

            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSpriteList.Count];
            skinUnlocked[0] = true;
            skinUnlocked[1] = true;

            ComparedSaveCount = 0.000f;

            diamondCount = 0;

            LanguageIndex = 0;
            SetLenguageFirstTime = true;

            LenguajeDropDown.instance.InitLanguage(LanguageIndex);
            GameManager.Instance.changeLenguaje();
            data = new GameData();
        }
        else
        {
            isMusicOn = data.GetIsMusicOn();
            isMainGameMusicOn = data.GetIsMusicGameOn();
            
            SoundValue = data.GetSoundValue();
            MusicValue = data.GetMusicValue();

            bestScoreArr = data.GetBestScoreArr();
            selectSkin = data.GetSelectSkin();
            skinUnlocked = data.GetSkinUnlocked();
            diamondCount = data.GetDiamondCount();

            LanguageIndex = data.GetLanguageIndex();
            SetLenguageFirstTime = false;
            LenguajeDropDown.instance.InitLanguage(LanguageIndex);
            GameManager.Instance.changeLenguaje();
            ComparedSaveCount = data.GetCompareSaveCount();
            if (!skinUnlocked[1])
            {
                skinUnlocked[1] = true;
            }
            
            if (skinUnlocked.Length != vars.skinSpriteList.Count)
            {
                bool[] ToReplace = new bool[vars.skinSpriteList.Count +1];
                for (int i = 0; i < skinUnlocked.Length; i++)
                {
                    ToReplace.SetValue(skinUnlocked[i], i);
                }
                print("Characters Updated");
                skinUnlocked = ToReplace;
            }
        }
    }

    public void CheckForLoadingData()
    {
        CloudSaveTest.instance.CheckLoadFromTheCloud();
    }

    private void Save()
    {
        ComparedSaveCount += 0.0001f;
        Debug.Log(ComparedSaveCount);
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.SetBestScoreArr(bestScoreArr);
                data.SetDiamondCount(diamondCount);

                data.SetLanguageIndex(LanguageIndex);

                data.SetIsFirstGame(isFirstGame);
                data.SetIsMusicOn(isMusicOn);
                data.SetIsMusicMenuGameOn(isMainGameMusicOn);
                data.SetSoundValue(SoundValue);
                data.SetMusicValue(MusicValue);

                data.SetCompareSaveCount(ComparedSaveCount);

                data.SetSelectSkin(selectSkin);
                data.SetSkinUnlocked(skinUnlocked);
                bf.Serialize(fs, data);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

        if (Social.localUser.authenticated)
        {
            CloudSaveTest.instance.SaveInCloud();
        }
    }

    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open))
            {
                data = (GameData)bf.Deserialize(fs);
            }
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
    }

    public void ResetData()
    {
        isFirstGame = false;
        isMusicOn = true;
        SoundValue = 0.5f;
        MusicValue = 0.5f;

        isMainGameMusicOn = true;
        bestScoreArr = new int[3];
        selectSkin = 0;
        skinUnlocked = new bool[vars.skinSpriteList.Count];
        skinUnlocked[0] = true;
        skinUnlocked[1] = true;
        diamondCount = 0;

        Save();
    }

    private void OnApplicationQuit()
    {
        Save();
        vars.RemoveAds = false;
        vars.OnStart = true;
    }

    private bool ChangeMusic = false;
    [HideInInspector]
    public bool Fall = false;
    private void Update()
    {
        if (!Fall)
        {
            if(IsGameStarted && MenuMusic.clip != vars.Music[1])
            {

                MenuMusic.volume -= 0.002f;
                if(MenuMusic.volume <= 0)
                {
                    ChangeMusic = true;

                }
            }

            if (ChangeMusic)
            {
                MenuMusic.clip = vars.Music[1];
                MenuMusic.Play();
                MenuMusic.DOFade(GetMusicValue(), 4.6f);
                ChangeMusic = false;
            }

            if(IsGameStarted && MenuMusic.clip != vars.Music[1] && GameData.IsAgainGame)
            {
                ChangeMusic = true;
                Debug.Log("Music Changed On Try Again");
            }
        }
        else
        {
            MenuMusic.DOFade(0f, 1f);
        }

        if (TryLoadFromCloud)
        {
            isMusicOn = data.GetIsMusicOn();
            isMainGameMusicOn = data.GetIsMusicGameOn();

            SoundValue = data.GetSoundValue();
            MusicValue = data.GetMusicValue();

            bestScoreArr = data.GetBestScoreArr();
            selectSkin = data.GetSelectSkin();
            skinUnlocked = data.GetSkinUnlocked();
            diamondCount = data.GetDiamondCount();

            LanguageIndex = data.GetLanguageIndex();

            ComparedSaveCount = data.GetCompareSaveCount();
            if (!skinUnlocked[1])
            {
                skinUnlocked[1] = true;
            }

            if (skinUnlocked.Length != vars.skinSpriteList.Count)
            {
                bool[] ToReplace = new bool[vars.skinSpriteList.Count + 1];
                for (int i = 0; i < skinUnlocked.Length; i++)
                {
                    ToReplace.SetValue(skinUnlocked[i], i);
                }
                skinUnlocked = ToReplace;
            }
            Debug.Log("Progress Overwritted Succesful");

            TryLoadFromCloud = false;
        }
    }

    void OnEnable()
    {
        RewardsInterface.GetComponent<DailyRewards>().onClaimPrize += OnClaimPrizeDailyRewards;
    }

    void OnDisable()
    {
        if(RewardsInterface != null)
        {
            RewardsInterface.GetComponent<DailyRewards>().onClaimPrize -= OnClaimPrizeDailyRewards;
        }
    }

    // this is your integration function. Can be on Start or simply a function to be called
    public void OnClaimPrizeDailyRewards(int day)
    {
        //This returns a Reward object
        //Reward myReward = DailyRewards.GetInstance().GetReward(day);
        Reward myReward = RewardsInterface.GetComponent<DailyRewards>().GetReward(day);

        // And you can access any property
        print(myReward.unit);   // This is your reward Unit name
        print(myReward.reward); // This is your reward count

        var rewardsCount = PlayerPrefs.GetInt("MY_REWARD_KEY", 0);
        rewardsCount += myReward.reward;
        //
        int _reward = DailyRewards.GetInstance().GetReward(day).reward;
        diamondCount += _reward;
        Save();
        //
        PlayerPrefs.SetInt("MY_REWARD_KEY", rewardsCount);
        PlayerPrefs.Save();
    }
}
