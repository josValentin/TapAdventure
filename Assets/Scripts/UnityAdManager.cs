using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdManager : MonoBehaviour, IUnityAdsListener
{
#if UNITY_ANDROID
    private static readonly string storeID = "3741569";
#elif UNITY_IOS
    private static readonly string storeID = "3741568";
#else
    private static readonly string storeID = "none";
#endif

    private static readonly string videoID = "video";
    private static readonly string rewardedID = "rewardedVideo";

    
    private static readonly string bannerID = "banner";

    private Action adSuccess;
    private Action adSkipped;
    private Action adFailed;


#if UNITY_EDITOR
    private static bool testMode = true;
#else
    private static bool testMode = false;
#endif

    public static UnityAdManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Advertisement.AddListener(this);
            Advertisement.Initialize(storeID, testMode);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static void ShowStandarAd()
    {
        if (Advertisement.IsReady(videoID))
        {
            Advertisement.Show(videoID);
        }
    }

    public static void showBanner()
    {
        instance.StartCoroutine(ShowBannerWhenReady());
    }

    public static void hideBanner()
    {
        
        Advertisement.Banner.Hide();
    }

    public static void ShowRewardedAd(Action success, Action skipped, Action failed)
    {
        /*if (Advertisement.IsReady(rewardedID))
        {
            Advertisement.Show(rewardedID);
        }*/
        Advertisement.Show(rewardedID);

        instance.adSuccess = success;
        instance.adSkipped = skipped;
        instance.adFailed = failed;

    }

    private static IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady())
        {
            yield return new WaitForSeconds(0.5f);
        }
        //yield return new WaitForSeconds(0.5f);
        //yield return new WaitForSeconds(0.5f);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(bannerID);

    }


    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(placementId == rewardedID)
        {
            switch (showResult)
            {
                case ShowResult.Finished:
                    adSuccess();
                    break;
                case ShowResult.Skipped:
                    adSkipped();
                    break;
                case ShowResult.Failed:
                    adFailed();
                    break;
            }
        }
    }




    public void OnUnityAdsDidError(string message)
    {
    } 
    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
