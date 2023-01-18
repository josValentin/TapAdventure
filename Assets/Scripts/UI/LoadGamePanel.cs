using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadGamePanel : MonoBehaviour
{
    private Image img_Bg;
    private GameObject dialog;

    private Button btn_Close;

    public ShopPanel _shopPanel;
    //public static LoadGamePanel instance;
    private void Awake()
    {
        //instance = this;
        EventCenter.AddListener(EventDefine.ShowLoadGamePanel, Show);
        img_Bg = transform.GetChild(0).GetComponent<Image>();
        dialog = transform.GetChild(1).gameObject;
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;
        btn_Close = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);

        gameObject.SetActive(false);

    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowLoadGamePanel, Show);
    }

    private void Show()
    {
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
            GameManager.Instance.RewardsInterface.Show();

        });
    }

    public void Loading()
    {
        GameObject btn_LoadGame = transform.GetChild(1).GetChild(2).transform.gameObject;
        GameObject ImageLoad = transform.GetChild(1).GetChild(3).transform.gameObject;

        btn_LoadGame.SetActive(false);
        ImageLoad.SetActive(true);
        StartCoroutine(Loaded());
    }

    IEnumerator Loaded()
    {
#if UNITY_ANDROID
        while (GameManager.Instance.GetComapreSaveCount() != SaveManager.instance.dataToCompare.GetCompareSaveCount())
        {
            yield return new WaitForSeconds(0.5f);
        }
#elif UNITY_IOS
        while (GameManager.Instance.GetComapreSaveCount() != iCloudSave.instance._dataToSet.GetCompareSaveCount())
        {
            yield return new WaitForSeconds(0.5f);
        }
#else
        yield return new WaitForSeconds(0.5f);
#endif
        //_shopPanel.ReplaceInitFromLoadData();
        MainPanel main_panel = GameObject.FindGameObjectWithTag("MainPanel").GetComponent<MainPanel>();
        main_panel.Sound();
        main_panel.Music();

        OnCloseButtonClick();
        ShopPanel.instance.Init();
        Debug.Log("Loaded from cloud succesfully");

    }


}
