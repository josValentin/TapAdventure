using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;

using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GpgsScript : MonoBehaviour
{

    int points = 0;
    //public Text pointsText;
    [Header("Google Play Games (Android)")]
    public string leaderBoard;
    public string acheievementIDs;

    [Header("Game Center (IOS)")]
    public string leaderboardIOS;
    public string AchivementsIOS;

    void Start()
    {
        // Recommended for debugging
#if UNITY_ANDROID

        PlayGamesPlatform.DebugLogEnabled = true;

        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

#elif UNITY_IOS
        Debug.Log("Unity iPhone");

#else
    Debug.Log("Any other platform");

#endif

    }

    /// <summary>
    /// Make Login and manage the succes or failure
    /// </summary>
    public void LogIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login Sucess");
                GameManager.Instance.CheckForLoadingData();
                if(IAPManager.instance.IsInitialized() == true)
                {
                    IAPManager.instance.CheckRemoveAdsExternal();

                }
                else
                {
                    StartCoroutine(AppPurchasingIsInitialized());
                }
                LogInPanel.instance.OnClose();

            }
            else
            {

                StartCoroutine(TryLogInAgain());
            }
        });
    }

    IEnumerator TryLogInAgain()
    {
        yield return new WaitForSeconds(2f);

        if (Social.localUser.authenticated)
        {
            Debug.Log("Login Sucess");
            GameManager.Instance.CheckForLoadingData();
            if (IAPManager.instance.IsInitialized() == true)
            {
                IAPManager.instance.CheckRemoveAdsExternal();

            }
            else
            {
                StartCoroutine(AppPurchasingIsInitialized());
            }
            LogInPanel.instance.OnClose();

        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Login Sucess");
                    GameManager.Instance.CheckForLoadingData();
                    if (IAPManager.instance.IsInitialized() == true)
                    {
                        IAPManager.instance.CheckRemoveAdsExternal();

                    }
                    else
                    {
                        StartCoroutine(AppPurchasingIsInitialized());
                    }

                    LogInPanel.instance.OnClose();

                }
                else
                {
                    Debug.Log("Login failed");
                    //IAPManager.instance.CheckRemoveAdsExternal();
                    SaveManager.instance.CheckedForLoad = true;
                    LogInPanel.instance.OnClose();
                    if (IAPManager.instance.GetRemovingOfAdsIsChecked() == false)
                    {
                        IAPManager.instance.SetRemovingOfAdsIsChecked(true);

                    }

                }
            });
        }

    }

    IEnumerator AppPurchasingIsInitialized()
    {
        yield return new WaitForSeconds(2f);

        if (IAPManager.instance.IsInitialized() == true)
        {
            IAPManager.instance.CheckRemoveAdsExternal();

        }
        else
        {
            Debug.Log("App purchasing is not initialazing, continue with game");
            IAPManager.instance.SetRemovingOfAdsIsChecked(true);

        }
    }

    /// <summary>
    /// Shows Leaderboard
    /// </summary>
    public void OnShowLeaderBoard()
    {
        //Debug.Log("worksss");
        if (!Social.localUser.authenticated)
        {
            LogIn();
        }

#if UNITY_ANDROID


          PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoard);

#elif UNITY_IOS
        Debug.Log("Unity iPhone");
        Social.ShowLeaderboardUI();
#else
    Debug.Log("Any other platform");

#endif
        //Social.ShowLeaderboardUI (); // Show all leaderboard
        
    }

    /// <summary>
    /// Adds score to Leaderboard
    /// </summary>
    public void addScoreLeaderBoard(int score)
    {
#if UNITY_ANDROID

        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, leaderBoard, (bool success) =>
            {
                if (success)
                {
                    //points = 0;
                    //pointsText.text = "Points: " + points;
                    Debug.Log("UpdateScore Success");
                }
                else
                {
                    Debug.Log("Update Score Fail");
                }
            });
        }


#elif UNITY_IOS
        Debug.Log("Unity iPhone");

        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, leaderboardIOS, (bool success) =>
            {
                if (success)
                {
                    //points = 0;
                    //pointsText.text = "Points: " + points;
                    Debug.Log("UpdateScore Success");
                }
                else
                {
                    Debug.Log("Update Score Fail");
                }
            });
        }

#else
    Debug.Log("Any other platform");

#endif

    }

    /// <summary>
    /// Unlock Reward
    /// </summary>
    public void rewardAchiv()
    {
        Social.ReportProgress(acheievementIDs, 200.0f, (bool success) =>
        {
            // handle success or failure
        });
    }

    /// <summary>
    /// Adding points
    /// </summary>
    public void morePoints()
    {
        points = points + 100;
        //pointsText.text = "Points: " + points;
    }

    /// <summary>
    /// Log Out
    /// </summary>
    public void OnLogOut()
    {

#if UNITY_ANDROID

 ((PlayGamesPlatform)Social.Active).SignOut();

#elif UNITY_IOS
        Debug.Log("Unity iPhone");

#else
    Debug.Log("Any other platform");

#endif
       
    }


}
