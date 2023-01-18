using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

[Flags]
public enum PlayServiceError: byte
{
    None = 0,
    Timeout = 1,
    NotAuthenticated = 2,
    SaveGameNotEnabled = 4,
    CloudSaveNameNotSet = 8,
}

public class PlayServices : MonoBehaviour
{
    [Header("Save")]
    [SerializeField] private bool enableSaveGame;
    [SerializeField] private string cloudSaveName = "";
    [SerializeField] private DataSource dataSource;
    [SerializeField] private ConflictResolutionStrategy conflictStrategy;

    public static PlayServices instance;

#if UNITY_ANDROID
    private void Awake()
    {
        // Persist through scenes
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        PlayGamesClientConfiguration.Builder builder = new PlayGamesClientConfiguration.Builder();

        // Enables saving games progress
        if (enableSaveGame)
        {
            builder.EnableSavedGames();
        }

        PlayGamesPlatform.InitializeInstance(builder.Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCloudSave(Action<SavedGameRequestStatus, ISavedGameMetadata> callback, Action<PlayServiceError> errorCallBack = null)
    {
        PlayServiceError error = global::PlayServiceError.None;
        if (!Social.localUser.authenticated)
            error |= global::PlayServiceError.NotAuthenticated;
        if (PlayGamesClientConfiguration.DefaultConfiguration.EnableSavedGames)
            error |= global::PlayServiceError.SaveGameNotEnabled;
        if (string.IsNullOrWhiteSpace(cloudSaveName))
            error |= global::PlayServiceError.CloudSaveNameNotSet;
        if (error != global::PlayServiceError.None)
            errorCallBack?.Invoke(error);

        if (Social.localUser.authenticated)
        {

            var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.OpenWithAutomaticConflictResolution(cloudSaveName, dataSource, conflictStrategy, callback);
        }


    }
#elif UNITY_IOS
    private void Awake()
    {
        Debug.Log("no se puede inicializar Google play games en iOS");
    }
#endif
}
