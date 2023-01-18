using System.Collections;
using System.Collections.Generic;
using System;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

public class CloudSaveTest : MonoBehaviour
{
    public static CloudSaveTest instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SaveManager.instance.OnSave += AfterSave;
        SaveManager.instance.OnLoad += AfterLoad;
    }

    public void AfterSave(SavedGameRequestStatus status)
    {
        switch (status)
        {
            case SavedGameRequestStatus.Success:
                SaveManager.instance.State.SaveCount++;
                SaveManager.instance.State.LastSaveTime = DateTime.Now;
                break;
            default:
                Debug.Log(status.ToString());
                break;
        }
    }

    public void AfterLoad(SavedGameRequestStatus status)
    {
        try
        {
            switch (status)
            {
                case SavedGameRequestStatus.Success:
                    Debug.Log("Loaded save!");
                    break;
                default:
                    Debug.Log(status.ToString());
                    break;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    public void SaveInCloud()
    {
#if UNITY_ANDROID
        SaveManager.instance.SavetoCloud();
#elif UNITY_IOS
        iCloudSave.instance.iCloudSaveValue();
#endif
        Debug.Log("Saving to Cloud...");

    }

    public void CheckLoadFromTheCloud()
    {
        //SaveManager.instance.LoadFromCloud();

#if UNITY_ANDROID
        SaveManager.instance.LoadFromCloud();
#elif UNITY_IOS
        iCloudSave.instance.iCloudLoadValue();
#endif
    }

    public void LoadFromCloud()
    {
        //SaveManager.instance.SetLoadedGameData();

#if UNITY_ANDROID
        SaveManager.instance.SetLoadedGameData();
#elif UNITY_IOS
        iCloudSave.instance.SetLoadedGameDataiCloud();
#endif
    }
}
