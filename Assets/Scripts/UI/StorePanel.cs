using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StorePanel : MonoBehaviour
{
    private Image img_Bg;
    private GameObject dialog;
    public GameObject VerifyRemove;
    public GameObject RemoveAds;
    public static StorePanel instance;

    private Button btn_Close;
    private Button btn_Ad;

    public TextMeshProUGUI Gems100;
    public TextMeshProUGUI Gems500;
    public TextMeshProUGUI Gems1000;
    public TextMeshProUGUI Gems2000;

    //public TextMeshProUGUI RemoveAdsPrice;
    public TextMeshProUGUI Gems100Price;
    public TextMeshProUGUI Gems500Price;
    public TextMeshProUGUI Gems1000Price;
    public TextMeshProUGUI Gems2000Price;


    private void Awake()
    {
        instance = this;
        EventCenter.AddListener(EventDefine.ShowStorePanel, Show);
        img_Bg = transform.GetChild(0).GetComponent<Image>();
        dialog = transform.GetChild(1).gameObject;
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;
        btn_Close = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);

        btn_Ad = transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>();
        btn_Ad.onClick.AddListener(OnAdButtonClick);
        gameObject.SetActive(false);
    }
    void Start()
    {
        
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowStorePanel, Show);
    }
    public void DisableBuyRemoveAdsButton()
    {
        if (GameManager.Instance.vars.RemoveAds)
        {
            RemoveAds.SetActive(false);
            VerifyRemove.SetActive(true);
        }
        else
        {
            RemoveAds.SetActive(true);
            VerifyRemove.SetActive(false);
        }
    }
    private void Show()
    {

        DisableBuyRemoveAdsButton();
        //IAPManager.instance.CheckRemoveAdsExternal();
        gameObject.SetActive(true);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f), 0.3f);
        dialog.transform.DOScale(Vector3.one, 0.3f);
        
    }
    private void OnCloseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.3f);
        dialog.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnAdButtonClick()
    {
        Debug.Log("Watching rewarded Ad");
        AdManager.instance.SetClaimRewarded_JustWatch(1);
        AdManager.instance.DisplayVideoAd(NoReady);
    }

    private void NoReady()
    {
        Debug.Log("The ad is not Ready, it won't work");
    }

    public void ChangeLanguageStorePanel()
    {
        btn_Close.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.TextLenguaje[6];
        btn_Ad.transform.GetChild(0).GetComponent<Text>().text = GameManager.Instance.TextLenguaje[7];
        RemoveAds.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.TextLenguaje[8];
        VerifyRemove.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.TextLenguaje[9];

        Gems100.text = GameManager.Instance.TextLenguaje[10];
        Gems500.text = GameManager.Instance.TextLenguaje[11];
        Gems1000.text = GameManager.Instance.TextLenguaje[12];
        Gems2000.text = GameManager.Instance.TextLenguaje[13];

        //RemoveAdsPrice.text = "";

        RemoveAds.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameManager.Instance.TextLenguaje[14];
        Gems100Price.text = GameManager.Instance.TextLenguaje[15];
        Gems500Price.text = GameManager.Instance.TextLenguaje[16];
        Gems1000Price.text = GameManager.Instance.TextLenguaje[17];
        Gems2000Price.text = GameManager.Instance.TextLenguaje[14];


    }
}
