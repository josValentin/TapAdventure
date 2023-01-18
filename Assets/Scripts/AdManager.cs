using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
public class AdManager : MonoBehaviour
{

    //private BannerView bannerAD;
    private RewardBasedVideoAd videoAD;
    private BannerView bannerView;

    private InterstitialAd intersitialAd;

    public GameObject ButtonAd;

    private int ClaimReward_JustWatch = 0;

    public static AdManager instance;

    
    private void Awake()
    {
        instance = this;
        videoAD = RewardBasedVideoAd.Instance;

/*#if UNITY_ANDROID
        string APP_ID = "ca-app-pub-4710545068159097~5559130709";
#elif UNITY_IPHONE
 string APP_ID = "ca-app-pub-4710545068159097~8890396275";
#else
        string APP_ID = "unexpected_platform";
#endif
*/
        MobileAds.Initialize(initStatus => { });
        //MobileAds.Initialize(APP_ID);
        
        RequestVideoAd();
        RequestBannerAd();
        RequestIntersitialAd();
    }
    // Start is called before the first frame update
    void Start()
    {
       

    }

    public void RequestBannerAd()
    {
#if UNITY_ANDROID
        string BannerId = "ca-app-pub-4710545068159097/1499169020";
        //string BannerId = "ca-app-pub-3940256099942544/6300978111"; //For test ad
#elif UNITY_IPHONE
        string BannerId = "ca-app-pub-4710545068159097/2936577035";
             //string BannerId = "ca-app-pub-3940256099942544/2934735716" For test ad;
#else
        string BannerId = "unexpected_platform";
#endif

        bannerView = new BannerView(BannerId, AdSize.Banner, AdPosition.Bottom);
    }

    public void DisplayBannerAd()
    {
        //AdRequest request = new AdRequest.Builder().Build();
        AdRequest request = new AdRequest.Builder().Build();

        this.bannerView.LoadAd(request);

    }




    public void RequestIntersitialAd()
    {
#if UNITY_ANDROID
        string IntersitialId = "ca-app-pub-4710545068159097/1991700022";
        //string IntersitialId = "ca-app-pub-3940256099942544/1033173712"; //for Test ad
#elif UNITY_IPHONE
        string IntersitialId = "ca-app-pub-4710545068159097/7997332021";
             //string IntersitialId = "ca-app-pub-3940256099942544/4411468910" // for Test ad;
#else
        string IntersitialId = "none";
        string BannerId = "unexpected_platform";
#endif

        intersitialAd = new InterstitialAd(IntersitialId);
        AdRequest request = new AdRequest.Builder().Build();

        this.intersitialAd.LoadAd(request);
    }

    public void DisplayIntersitialAd()
    {
        //AdRequest request = new AdRequest.Builder().Build();

        if (intersitialAd.IsLoaded())
        {
            intersitialAd.Show();

        }
    }

    public void DestroyBannert ()
    {
        this.bannerView.Destroy();
        print("Banner destroyed");
        //RequestBannerAd();
    }

    public void DestroyIntersitial()
    {
        this.intersitialAd.Destroy();
        print("Intersitial destroyed");
        RequestIntersitialAd();
    }

    public void RequestVideoAd()
    {
#if UNITY_ANDROID
            //string adUnitId = "ca-app-pub-3940256099942544/5224354917"; //ForTestAd
        string adUnitId = "ca-app-pub-4710545068159097/9153286003";
#elif UNITY_IPHONE
         string adUnitId = "ca-app-pub-4710545068159097/3446497904";
              //string adUnitId = "ca-app-pub-4710545068159097/3446497904" // For Test Ad;
#else
        string adUnitId = "unexpected_platform";
#endif

        videoAD = RewardBasedVideoAd.Instance;

        //AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        AdRequest adRequest = new AdRequest.Builder().Build();


        videoAD.LoadAd(adRequest, adUnitId);
    }

    public void DisplayVideoAd(Action ifNoLoaded)
    {
        //--------------------------------------------------------Advertisement REWARDED_VIDEO_AD

        //UnityAdManager.ShowRewardedAd(AddCoins, AddCoins, failToLoad);

        //
        //ifNoLoaded();
        if (videoAD.IsLoaded())
        {


            videoAD.Show();
            RequestVideoAd();

        }
        else
        {
            ifNoLoaded();
        }
        //--------------------------------------------------------Advertisement REWARDED_VIDEO_AD
    }

    //This is only for unityAds
    void AddCoins()
    {
        GameManager.Instance.adddiamondForAd();
        ButtonAd.SetActive(false);
    }

    void failToLoad()
    {
        Debug.Log("Can't load rewarded video");
        UnityAdManager.ShowRewardedAd(AddCoins, AddCoins, failToLoad);
    }
    //This is only for unityAds

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdLoaded event received");
        //DisplayVideoAd();
        
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
        //+ args.Message);
        RequestVideoAd();
        //RequestBannerAd();
        //RequestIntersitialAd();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }
    public void SetClaimRewarded_JustWatch(int TypeOfRewardInVideoAd)
    {
        ClaimReward_JustWatch = TypeOfRewardInVideoAd;
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        if(ClaimReward_JustWatch == 1)
        {
            GameManager.Instance.adddiamondForAd();
            Debug.Log("Ad has been seen: Got Gems From Ad");
        }
        else if(ClaimReward_JustWatch == 2)
        {
            GameManager.Instance.RewardsInterface.VideoAdIsNotReady();
            Debug.Log("Ad has been seen: Got the daily reward");

        }

        //ButtonAd.SetActive(false);

        //RequestVideoAd();

        ClaimReward_JustWatch = 0;
        //RequestBannerAd();
        //RequestIntersitialAd();

        //AddCoins();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    void HandleBannerADEvents(bool subscribe)
    {
        if (subscribe)
        {
            // Called when an ad request has successfully loaded.
            videoAD.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            videoAD.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            videoAD.OnAdOpening += HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            videoAD.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            videoAD.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        }
        else
        {
            // Called when an ad request has successfully loaded.
            videoAD.OnAdLoaded -= HandleOnAdLoaded;
            // Called when an ad request failed to load.
            videoAD.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            videoAD.OnAdOpening -= HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            videoAD.OnAdClosed -= HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            videoAD.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
        }
    }
 
    void OnEnable()
    {
        HandleBannerADEvents(true);
    }

     void OnDisable()
    {
        HandleBannerADEvents(false);

    }


}
