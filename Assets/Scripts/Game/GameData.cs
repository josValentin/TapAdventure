using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    /// <summary>
    /// 是否再来一次游戏
    /// </summary>
    public static bool IsAgainGame = false;

    private bool isFirstGame;
    //private bool adsEnabled;

    //Efecto de juego
    private bool isMusicOn;
    private float SoundValue;

    //Musica de juego
    private bool isMusicMenuGameOn;
    private float MusicValue;
 

    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;
    private float compareSaveCount;

    private int LanguageIndex;
    public void SetIsFirstGame(bool isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }
    /*public void SetAdsEnabled(bool adsEnabled)
    {
        this.adsEnabled = adsEnabled;
    }*/
    public void SetIsMusicOn(bool isMusicOn)
    {
        this.isMusicOn = isMusicOn;
    }
    public void SetIsMusicMenuGameOn(bool isMusicMenuGameOn)
    {
        this.isMusicMenuGameOn = isMusicMenuGameOn;
    }

    public void SetMusicValue(float MusicValue)
    {
        this.MusicValue = MusicValue;
    }

    public void SetSoundValue(float SoundValue)
    {
        this.SoundValue = SoundValue;
    }
    public void SetBestScoreArr(int[] bestScoreArr)
    {
        this.bestScoreArr = bestScoreArr;
    }
    public void SetSelectSkin(int selectSkin)
    {
        this.selectSkin = selectSkin;
    }
    public void SetSkinUnlocked(bool[] skinUnlocked)
    {
        this.skinUnlocked = skinUnlocked;
    }
    public void SetDiamondCount(int diamondCount)
    {
        this.diamondCount = diamondCount;
    }

    public void SetLanguageIndex(int languageIndex)
    {
        this.LanguageIndex = languageIndex;
    }

    public void SetCompareSaveCount(float compareSaveCount)
    {
        this.compareSaveCount = compareSaveCount;
    }
    public bool GetIsFirstGame()
    {
        return isFirstGame;
    }
    /*public bool GetAdsEnabled()
    {
        return adsEnabled;
    }*/
    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }
    public bool GetIsMusicGameOn()
    {
        return isMusicMenuGameOn;
    }

    public float GetMusicValue()
    {
        return MusicValue;
    }

    public float GetSoundValue()
    {
        return SoundValue;
    }
    public int[] GetBestScoreArr()
    {
        return bestScoreArr;
    }
    public int GetSelectSkin()
    {
        return selectSkin;
    }
    public bool[] GetSkinUnlocked()
    {
        return skinUnlocked;
    }
    public int GetDiamondCount()
    {
        return diamondCount;
    }

    public float GetCompareSaveCount()
    {
        return compareSaveCount;
    }

    public int GetLanguageIndex()
    {
        return LanguageIndex;
    }
}
