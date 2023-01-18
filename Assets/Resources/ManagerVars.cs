 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName ="CreatManagerVarsContainer")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManagerVars()
    {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }
    public List<Sprite> bgThemeSpriteList = new List<Sprite>();
    public List<Sprite> platformThemeSpriteList = new List<Sprite>();
    public List<Sprite> skinSpriteList = new List<Sprite>();
    public List<Sprite> characterSkinSpriteList = new List<Sprite>();
    public GameObject characterPre;
    public GameObject skinChooseItemPre;
    public List<string> skinNameList = new List<string>();
    /// <summary>
    /// 皮肤价格
    /// </summary>
    public List<int> skinPrice = new List<int>();
    public GameObject normalPlatformPre;
    public List<GameObject> commonPlatformGroup = new List<GameObject>();
    public List<GameObject> grassPlatformGroup = new List<GameObject>();
    public List<GameObject> winterPlatformGroup = new List<GameObject>();
    public GameObject spikePlatformLeft;
    public GameObject spikePlatformRight;
    public GameObject deathEffect;
    public GameObject diamondPre;
    public float nextXPos = 0.554f, nextYPos = 0.645f;

    public List<Sprite> DiamondsSpritesList = new List<Sprite>();

    public AudioClip jumpClip, hitClip, errorClip,diamondClip, diamondClip2, diamondClip3 ,buttonClip, rewardClip;
    public List<AudioClip> FallsDeath = new List<AudioClip>();
    public AudioClip SpongeBob2, SpongeBob3;

    public List<AudioClip> Music = new List<AudioClip>();

    public List<Sprite> SpritesButtonsSkills = new List<Sprite>();
    public Sprite musicOn, musicOff, menuMusicOff, menuMusicOn;

    public List<Sprite> LanguageFlagsSPrites = new List<Sprite>();

    [HideInInspector]
    public bool OnStart = true;
    //[HideInInspector]
    //public bool AdsEnabled = true;
    [HideInInspector]
    public bool RemoveAds = false;
}
