using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using NiobiumStudios;

[Flags]
public enum SaveMethod : byte
{
    GoogleCloud,
    LocalFile,
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [Header("Config")]
    [SerializeField] private SaveMethod saveMethod;
    [SerializeField] private SaveMethod loadMethod;

    [Header("Local")]
    [SerializeField] private string savefileName = "data.ss";

    [Header("Logic")]
    private SaveState state;
    private BinaryFormatter formatter;
    public Action<SavedGameRequestStatus> OnSave;
    public Action<SavedGameRequestStatus> OnLoad;

    private byte[] LoadedGameData;

    public bool CheckedForLoad = false;

    private void Awake()
    {
        instance = this;
        CheckedForLoad = false;

    }
    // Start is called before the first frame update
    void Start()
    {
        formatter = new BinaryFormatter();
        state = new SaveState();

    }
    public SaveState State { get => state; set => state = value;}

    private byte[] SerializeState()
    {
        using(MemoryStream ms = new MemoryStream())
        {
            //formatter.Serialize(ms, state);
            formatter.Serialize(ms, GameManager.Instance.GetGameData());
            return ms.GetBuffer();
        }
    }

    private GameData DeserializeState(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        {
            return (GameData)formatter.Deserialize(ms);
        }
    }

    // Google Cloud
    public void SavetoCloud(Action<PlayServiceError> errorCallBack = null)
    {
#if UNITY_ANDROID
        PlayServices.instance.OpenCloudSave(OnSaveResponse, errorCallBack);
#elif UNITY_IOS
        Debug.Log("No se puede guardar el progreso en la nube de google play en iOS");
#endif
    }
    private void OnSaveResponse(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            byte[] data = SerializeState();
            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().WithUpdatedDescription("Last save : " + DateTime.Now.ToString()).Build();



#if UNITY_ANDROID
                        var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.CommitUpdate(meta, update, data, SaveCallBack);
#elif UNITY_IOS
            Debug.Log("No se puede guardar el progreso en la nube de google play en iOS");
#endif
        }
        else
        {
            OnSave?.Invoke(status);
        }

    }
    private void SaveCallBack(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        OnSave?.Invoke(status);
    }

    public void LoadFromCloud(Action<PlayServiceError> errorCallBack = null)
    {
        Debug.Log("Checking to load from the cloud...");

#if UNITY_ANDROID
       PlayServices.instance.OpenCloudSave(OnLoadResponse, errorCallBack);
#elif UNITY_IOS
        Debug.Log("No se puede guardar el progreso en la nube de google play en iOS");
        //CheckedForLoad = true; 

        //LogInPanel.instance.OnClose();
        //ShopPanel.instance.Init();
#endif

    }

    private void OnLoadResponse(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if(status == SavedGameRequestStatus.Success)
        {


#if UNITY_ANDROID
                   var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.ReadBinaryData(meta, LoadCallBack);
#elif UNITY_IOS
            Debug.Log("No se puede guardar el progreso en la nube de google play en iOS");
                            //CheckedForLoad = true;

#endif
        }
        else
        {
            OnLoad?.Invoke(status);
            CheckedForLoad = true;

            LogInPanel.instance.OnClose();

        }
    }

    private void LoadCallBack(SavedGameRequestStatus status, byte[] data)
    {
        if(data != null)
        {
            bool dataNoExist = data.Length == 0;
            Debug.Log( "DataNoExist : " + dataNoExist);

            if (!dataNoExist && GameManager.Instance.GetComapreSaveCount() < DeserializeState(data).GetCompareSaveCount())
            {
                LoadedGameData = data;
                LogInPanel.instance.OnClose();
            }
            else 
            {
                Debug.Log("The user is up to date with it's progress");
                CheckedForLoad = true;
                LogInPanel.instance.OnClose();
            }


        }

        OnLoad?.Invoke(status);
    }

    public GameData dataToCompare;
    public void SetLoadedGameData()
    {
        dataToCompare = DeserializeState(LoadedGameData);
        GameManager.Instance.SetGameData(dataToCompare);        
        GameManager.Instance.TryLoadFromCloud = true;
        Debug.Log("Loading...");
    }


}
