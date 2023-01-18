using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Michsky.UI.ModernUIPack;
using GooglePlayGames.BasicApi.Multiplayer;
using TMPro;

public class SettingsPanel : MonoBehaviour
{
    public static SettingsPanel instance;

    public Button btn_Sound;
    public Button btn_Music;
    private ManagerVars vars;
    private Button btn_Close;
    private GameObject go_ScoreList;

    public Slider SoundBar;
    public Slider MusicBar;

    public TextMeshProUGUI txt_settings;
    public TextMeshProUGUI txt_languaje;

    public Button btn_RestorePurchases;
    public GameObject GlobalSettings;

    private void Awake()
    {
        instance = this;
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.ShowSettingsPanel, Show);
        //btn_Sound = transform.Find("Bg/btn_Sound").GetComponent<Button>();
        btn_Sound.onClick.AddListener(OnSoundButtonClick);
        //btn_Music = transform.Find("Bg/btn_Music").GetComponent<Button>();
        btn_Music.onClick.AddListener(OnMenuGameMusicButtonClick);

        SoundBar.onValueChanged.AddListener(delegate { OnSoundValueBarChanged(); });
        
        MusicBar.onValueChanged.AddListener(delegate { OnMusicValueBarChanged(); });

#if UNITY_ANDROID
        btn_RestorePurchases.gameObject.SetActive(false);
        //GlobalSettings.transform.position = new Vector3(0, -36f, 0);
        GlobalSettings.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -42f);
#elif UNITY_IOS
                btn_RestorePurchases.gameObject.SetActive(true);
                GlobalSettings.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0f);

#else
        
#endif
        btn_RestorePurchases.onClick.AddListener(OnRestoreButtonClick);

        btn_Close = transform.Find("btn_Close").GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);

        btn_Close.GetComponent<Image>().color = new Color(btn_Close.GetComponent<Image>().
    color.r, btn_Close.GetComponent<Image>().color.g, btn_Close.GetComponent<Image>().color.b, 0);

        go_ScoreList = transform.Find("Bg").gameObject;

        go_ScoreList.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);

    }
    // Start is called before the first frame update
    void Start()
    {

        SoundBar.value = GameManager.Instance.GetSoundValue() *100;
        MusicBar.value = GameManager.Instance.GetMusicValue() * 100;

        Sound();
        Music();
    }

    public void changeLanguajeSettingsPanel()
    {
        txt_settings.text = GameManager.Instance.TextLenguaje[18];
        txt_languaje.text = GameManager.Instance.TextLenguaje[19];

    }

    private void OnSoundButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);

        GameManager.Instance.SetIsMusicOn(!GameManager.Instance.GetIsMusicOn());

        Sound();
    }

    private void OnMenuGameMusicButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        GameManager.Instance.SetIsGameMainMusicOn(!GameManager.Instance.GetIsMainGameMusicOn());
        Music();
    }
    private void Sound()
    {
        if (GameManager.Instance.GetIsMusicOn())
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOn;
        }
        else
        {
            btn_Sound.transform.GetChild(0).GetComponent<Image>().sprite = vars.musicOff;
        }
        EventCenter.Broadcast(EventDefine.IsMusicOn, GameManager.Instance.GetIsMusicOn());

    }

    private void Music()
    {
        if (GameManager.Instance.GetIsMainGameMusicOn())
        {
            btn_Music.transform.GetChild(0).GetComponent<Image>().sprite = vars.menuMusicOn;
        }
        else
        {
            btn_Music.transform.GetChild(0).GetComponent<Image>().sprite = vars.menuMusicOff;
        }
        EventCenter.Broadcast(EventDefine.IsMainGameMusicOn, GameManager.Instance.GetIsMainGameMusicOn());

    }

    private void OnSoundValueBarChanged()
    {
        GameManager.Instance.SetSoundValue(SoundBar.value / 100f);

        EventCenter.Broadcast(EventDefine.UpdateSliderBarSound, GameManager.Instance.GetSoundValue());

    }

    private void OnMusicValueBarChanged()
    {
        GameManager.Instance.SetMusicValue(MusicBar.value / 100f);

        EventCenter.Broadcast(EventDefine.UpdateSliderBarMusic, GameManager.Instance.GetMusicValue());

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        gameObject.SetActive(true);
        btn_Close.GetComponent<Image>().DOColor(new Color(btn_Close.GetComponent<Image>().
            color.r, btn_Close.GetComponent<Image>().color.g,
            btn_Close.GetComponent<Image>().color.b, 0.3f), 0.3f);
        go_ScoreList.transform.DOScale(Vector3.one, 0.3f);


    }

    private void OnCloseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        btn_Close.GetComponent<Image>().DOColor(new Color(btn_Close.GetComponent<Image>().
            color.r, btn_Close.GetComponent<Image>().color.g,
            btn_Close.GetComponent<Image>().color.b, 0), 0.3f);
        go_ScoreList.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });

        if (GameManager.Instance.SetLenguageFirstTime)
        {
            GameManager.Instance.RewardsInterface.Show();
            GameManager.Instance.SetLenguageFirstTime = false;
        }
        //GameManager.Instance.MaxGameMusic = GameManager.Instance.GameMusic.volume;
        //GameManager.Instance.MaxMenuMusic = GameManager.Instance.GameMusic.volume;

    }

    private void OnRestoreButtonClick()
    {
        IAPManager.instance.RestorePurchases();
        Debug.Log("Trying to restore the puschases");
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowSettingsPanel, Show);
    }
}
