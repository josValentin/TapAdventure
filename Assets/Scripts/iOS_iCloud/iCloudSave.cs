using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class iCloudSave : MonoBehaviour
{
    //private const string ICLOUD_KEY = "myGameData";
    private const string ICLOUD_KEY_IsMusicOn = "isMusicOn";
    private const string ICLOUD_KEY_SoundValue = "soundValue";
    private const string ICLOUD_KEY_IsMusicMenuGameOn = "isMusicMenuGameOn";
    private const string ICLOUD_KEY_MusicValue = "musicValue";
    private const string ICLOUD_KEY_BestScoreFirst = "bestScoreFirst";
    private const string ICLOUD_KEY_BestScoreSceond = "bestScoreSecond";
    private const string ICLOUD_KEY_BestScoreThird = "bestScoreThird";
    private const string ICLOUD_KEY_SelectedSkin = "selectedSkin";


    private const string ICLOUD_KEY_DiamondCount = "diamondCount";
    private const string ICLOUD_KEY_CompareSaveCount = "compareSaveCount";
    private const string ICLOUD_KEY_LanguageIndex = "languageIndex";


    public static iCloudSave instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()  
    {

    }
#if UNITY_IOS
    void ShowBasicAlert() 
    {
        iOSPlugin.ShowAlert("Basic Alert", "Hello this is a basic alert !");
    }

    void ShowAlertConfirmation()
    {
        iOSPlugin.ShowAlertConfirmation("Basic Alert Confirmation", "Hello this is a basic confirmation !", "CallBack");
    }

    void RotateUpAlertConfirmation()
    {
        iOSPlugin.ShowAlertConfirmation("Rotating Up", "Should I Rotate Up?", "RotateUpCallBack");
    }

    void RotateDownAlertConfirmation()
    {
        iOSPlugin.ShowAlertConfirmation("Rotating Down", "Should I Rotate Down?", "RotateDownCallBack");
    }

    void ShareMessage()
    {
        iOSPlugin.ShareMessage("Sharing a message!", "https://www.youtube.com/c/dilmervalecillos");
    }

    void BatteryStatus()
    {
        var batteryStatus = iOSPlugin.GetBatteryStatus();
        iOSPlugin.ShowAlert("Battery Status", batteryStatus.ToString());
    }

    void BatteryLevel()
    {
        string batteryLevel = iOSPlugin.GetBatteryLevel();
        iOSPlugin.ShowAlert("Battery Level", batteryLevel);
    }

    [HideInInspector]
    public GameData _dataToSet;
    //
    private bool _isMusicOn;
    private float _soundValue;
    private bool _isMusicMenuGameOn;
    private float _MusicValue;

    private int[] _bestScoreArr = new int[3];

    private int _selectedSkin;

    private bool[] _skinUnlocked = new bool[0];

    private int _diamondCount;

    private float _compareSaveCount = 0.000f;

    private int _languageIndex;
    //
    public void iCloudLoadValue()
    {
        //string savedValue = iOSPlugin.iCloudGetStringValue(ICLOUD_KEY);
        //iOSPlugin.ShowAlert("iCloud Value", string.IsNullOrEmpty(savedValue) ? "Nothing Saved Yet..." : savedValue);
        //
        //
        //GameData saveGameData = iOSPlugin.iCloudGetGameDataValue(ICLOUD_KEY);
        //
        /*bool _isMusicOn = iOSPlugin.iCloudGetBoolValue(ICLOUD_KEY_IsMusicOn);
        float _soundValue = iOSPlugin.iCloudGetFloatValue(ICLOUD_KEY_SoundValue);
        bool _isMusicMenuGameOn = iOSPlugin.iCloudGetBoolValue(ICLOUD_KEY_IsMusicMenuGameOn);
        float _MusicValue = iOSPlugin.iCloudGetFloatValue(ICLOUD_KEY_MusicValue);

        int[] _bestScoreArr = new int[3];

        for (int i = 0; i < GameManager.Instance.GetScoreArr().Length; i++)
        {
            string ICLOUD_KEY_BestScore_I = "BestScore_" + i;
            int _BestScoreArr_I = iOSPlugin.iCloudGetIntValue(ICLOUD_KEY_BestScore_I);
            _bestScoreArr = _bestScoreArr.Concat(new int[] { _BestScoreArr_I }).ToArray();
        }

        int _selectedSkin = iOSPlugin.iCloudGetIntValue(ICLOUD_KEY_SelectedSkin);

        bool[] _skinUnlocked = new bool[0];
        for (int i = 0; i < GameManager.Instance.vars.skinSpriteList.Count; i++)
        {
            string ICLOUD_KEY_SkinUnlocked_I = "SkinUnlocked_" + i;
            bool _SkinUnlocked_I = iOSPlugin.iCloudGetBoolValue(ICLOUD_KEY_SkinUnlocked_I);
            _skinUnlocked = _skinUnlocked.Concat(new bool[] { _SkinUnlocked_I }).ToArray();
        }

        int _diamondCount = iOSPlugin.iCloudGetIntValue(ICLOUD_KEY_DiamondCount);
        float _compareSaveCount = iOSPlugin.iCloudGetFloatValue(ICLOUD_KEY_CompareSaveCount);
        int _languageIndex = iOSPlugin.iCloudGetIntValue(ICLOUD_KEY_LanguageIndex);

        GameData TryLoadedGameData = new GameData();

        TryLoadedGameData.SetIsMusicOn(_isMusicOn);
        TryLoadedGameData.SetSoundValue(_soundValue);
        TryLoadedGameData.SetIsMusicMenuGameOn(_isMusicMenuGameOn);
        TryLoadedGameData.SetMusicValue(_MusicValue);
        TryLoadedGameData.SetBestScoreArr(SortScoreArr(_bestScoreArr));
        TryLoadedGameData.SetSelectSkin(_selectedSkin);
        TryLoadedGameData.SetSkinUnlocked(_skinUnlocked);
        TryLoadedGameData.SetDiamondCount(_diamondCount);
        TryLoadedGameData.SetCompareSaveCount(_compareSaveCount);
        TryLoadedGameData.SetLanguageIndex(_languageIndex);*/
        //
        //
         _isMusicOn = iOSPlugin.iCloudGetBoolValue(ICLOUD_KEY_IsMusicOn);
         _soundValue = iOSPlugin.iCloudGetFloatValue(ICLOUD_KEY_SoundValue);
         _isMusicMenuGameOn = iOSPlugin.iCloudGetBoolValue(ICLOUD_KEY_IsMusicMenuGameOn);
         _MusicValue = iOSPlugin.iCloudGetFloatValue(ICLOUD_KEY_MusicValue);

         

        for (int i = 0; i < GameManager.Instance.GetScoreArr().Length; i++)
        {
            string ICLOUD_KEY_BestScore_I = "BestScore_" + i;
            int _BestScoreArr_I = iOSPlugin.iCloudGetIntValue(ICLOUD_KEY_BestScore_I);
            _bestScoreArr = _bestScoreArr.Concat(new int[] { _BestScoreArr_I }).ToArray();
        }

         _selectedSkin = iOSPlugin.iCloudGetIntValue(ICLOUD_KEY_SelectedSkin);


        for (int i = 0; i < GameManager.Instance.vars.skinSpriteList.Count; i++)
        {
            string ICLOUD_KEY_SkinUnlocked_I = "SkinUnlocked_" + i;
            bool _SkinUnlocked_I = iOSPlugin.iCloudGetBoolValue(ICLOUD_KEY_SkinUnlocked_I);
            _skinUnlocked = _skinUnlocked.Concat(new bool[] { _SkinUnlocked_I }).ToArray();
        }

         _diamondCount = iOSPlugin.iCloudGetIntValue(ICLOUD_KEY_DiamondCount);
         _compareSaveCount = iOSPlugin.iCloudGetFloatValue(ICLOUD_KEY_CompareSaveCount);
         _languageIndex = iOSPlugin.iCloudGetIntValue(ICLOUD_KEY_LanguageIndex);


        //iOSPlugin.ShowAlert("iCloud Value", saveGameData == null ? "Nothing Saved Yet..." : "Game Data Loaded succesfully");
        /*if (TryLoadedGameData.GetCompareSaveCount() != 0.000f && !float.IsNaN(TryLoadedGameData.GetCompareSaveCount()) && GameManager.Instance.GetComapreSaveCount() < TryLoadedGameData.GetCompareSaveCount())
        {
            _dataToSet = TryLoadedGameData;
            LogInPanel.instance.OnClose();
        }
        else
        {
            Debug.Log("The user is up to date with it's progress in IOS");
            SaveManager.instance.CheckedForLoad = true;
            LogInPanel.instance.OnClose();
            //ShopPanel.instance.Init();
        }*/

        if (_compareSaveCount != 0.000f && !float.IsNaN(_compareSaveCount) && GameManager.Instance.GetComapreSaveCount() < _compareSaveCount)
        {
            //_dataToSet = TryLoadedGameData;
            LogInPanel.instance.OnClose();
        }
        else
        {
            Debug.Log("The user is up to date with it's progress in IOS");
            SaveManager.instance.CheckedForLoad = true;
            LogInPanel.instance.OnClose();
            //ShopPanel.instance.Init();
        }

    }

    public int[] SortScoreArr(int[] ScoreArr)
    {
        List<int> list = ScoreArr.ToList();
        //从大到小排序list
        list.Sort((x, y) => (-x.CompareTo(y)));
        ScoreArr = list.ToArray();

        return ScoreArr;
    }

    public void SetLoadedGameDataiCloud()
    {
        GameManager.Instance.GetGameData().SetIsMusicOn(_isMusicOn);
        GameManager.Instance.GetGameData().SetSoundValue(_soundValue);
        GameManager.Instance.GetGameData().SetIsMusicMenuGameOn(_isMusicMenuGameOn);
        GameManager.Instance.GetGameData().SetMusicValue(_MusicValue);
        GameManager.Instance.GetGameData().SetBestScoreArr(SortScoreArr(_bestScoreArr));
        GameManager.Instance.GetGameData().SetSelectSkin(_selectedSkin);
        GameManager.Instance.GetGameData().SetSkinUnlocked(_skinUnlocked);
        GameManager.Instance.GetGameData().SetDiamondCount(_diamondCount);
        GameManager.Instance.GetGameData().SetCompareSaveCount(_compareSaveCount);
        GameManager.Instance.GetGameData().SetLanguageIndex(_languageIndex);

        //GameManager.Instance.SetGameData(_dataToSet);
        GameManager.Instance.TryLoadFromCloud = true;
        Debug.Log("Loading...");
    }

    public void iCloudSaveValue()
    {
        /*string valueToSave = System.Guid.NewGuid().ToString();
        bool success = iOSPlugin.iCloudSaveStringValue(ICLOUD_KEY, valueToSave);
        
        if(success)
        {
            iOSPlugin.ShowAlert("iCloud Value Saved Success", valueToSave);
        }
        else 
        {
            iOSPlugin.ShowAlert("iCloud Value Saved failed", valueToSave);
        }*/

        //GameData valueToSave = GameManager.Instance.GetGameData();
        //bool success = iOSPlugin.iCloudSaveGameDataValue(ICLOUD_KEY, valueToSave);

        bool SuccessMusicOn = iOSPlugin.iCloudSaveBoolValue(ICLOUD_KEY_IsMusicOn, GameManager.Instance.GetIsMusicOn());

        bool SuccessSoundValue = iOSPlugin.iCloudSaveFloatValue(ICLOUD_KEY_SoundValue, GameManager.Instance.GetSoundValue());

        bool SuccessMusicMenuGameOn = iOSPlugin.iCloudSaveBoolValue(ICLOUD_KEY_IsMusicMenuGameOn, GameManager.Instance.GetIsMainGameMusicOn());

        bool SuccessMusicValue = iOSPlugin.iCloudSaveFloatValue(ICLOUD_KEY_MusicValue, GameManager.Instance.GetMusicValue());

        for (int i = 0; i < GameManager.Instance.GetScoreArr().Length; i++)
        {
            string ICLOUD_KEY_BestScore_I = "BestScore_" + i;
            //int _BestScoreArr_I = iOSPlugin.iCloudGetIntValue(ICLOUD_KEY_BestScore_I);

            bool SuccessBestScore_I = iOSPlugin.iCloudSaveIntValue(ICLOUD_KEY_BestScore_I, GameManager.Instance.GetScoreArr()[i]); 
            //_bestScoreArr = _bestScoreArr.Concat(new int[] { _BestScoreArr_I }).ToArray();
        }

        bool SuccessSelectedSkin = iOSPlugin.iCloudSaveIntValue(ICLOUD_KEY_SelectedSkin, GameManager.Instance.GetCurrentSelectedSkin());

        for (int i = 0; i < GameManager.Instance.vars.skinSpriteList.Count; i++)
        {
            string ICLOUD_KEY_SkinUnlocked_I = "SkinUnlocked_" + i;
            //bool _SkinUnlocked_I = iOSPlugin.iCloudGetBoolValue(ICLOUD_KEY_SkinUnlocked_I);
            bool SuccessSkinUnlocked_I = iOSPlugin.iCloudSaveBoolValue(ICLOUD_KEY_SkinUnlocked_I, GameManager.Instance.GetSkinUnlocked(i));
            //_skinUnlocked = _skinUnlocked.Concat(new bool[] { _SkinUnlocked_I }).ToArray();
        }
        bool SuccessDiamonCount = iOSPlugin.iCloudSaveIntValue(ICLOUD_KEY_DiamondCount, GameManager.Instance.GetAllDiamond());
        bool SuccessCompareSaveCount = iOSPlugin.iCloudSaveFloatValue(ICLOUD_KEY_CompareSaveCount, GameManager.Instance.GetComapreSaveCount());
        bool SuccessLanguageIndex = iOSPlugin.iCloudSaveIntValue(ICLOUD_KEY_LanguageIndex, GameManager.Instance.getLanguageIndex());


        if (SuccessCompareSaveCount)
        {
            //iOSPlugin.ShowAlert("iCloud Value Saved Success", "Game Data saved in cloud succesully");
        }
        else
        {
            //iOSPlugin.ShowAlert("iCloud Value Saved failed", "Game Data was not saved in cloud");
            Debug.Log("There was an error, can't save to cloud");
        }
    }
#endif
}