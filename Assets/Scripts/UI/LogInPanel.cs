using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInPanel : MonoBehaviour
{
    private Image img_Bg;
    private GameObject dialog;
    public static LogInPanel instance;
    private void Awake()
    {
        instance = this;
        EventCenter.AddListener(EventDefine.ShowLogIngPanel, Show);
        img_Bg = transform.GetChild(0).GetComponent<Image>();
        dialog = transform.GetChild(1).gameObject;
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;

        gameObject.SetActive(false);

    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowLogIngPanel, Show);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f), 0.3f);
        dialog.transform.DOScale(Vector3.one, 0.3f);

    }
    public void OnClose()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(Close());
        }
    }

    private IEnumerator Close()
    {
        while (!IAPManager.instance.GetRemovingOfAdsIsChecked())
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("The removing of ads still no checked");
        }


        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.3f);
        dialog.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        if (!GameManager.Instance.vars.RemoveAds)
        {
            //-----------------------------------------------------------------------------------Advertisment: BANNER_AD

            AdManager.instance.DisplayBannerAd();
            //UnityAdManager.showBanner();
            //-----------------------------------------------------------------------------------Advertisment: BANNER_AD
        }

        if (GameManager.Instance.SetLenguageFirstTime)
        {

            if (Social.localUser.authenticated)
            {
                if (SaveManager.instance.CheckedForLoad == true)
                {
                    SettingsPanel.instance.Show();

                }
                else
                {
                    EventCenter.Broadcast(EventDefine.ShowLoadGamePanel);
                }

            } else
            {
                SettingsPanel.instance.Show();

            }
        }
        else
        {
            if (Social.localUser.authenticated)
            {
                if(SaveManager.instance.CheckedForLoad == true)
                {
                    GameManager.Instance.RewardsInterface.Show();
                }
                else
                {
                    EventCenter.Broadcast(EventDefine.ShowLoadGamePanel);
                }
                
            }

        }

    }
}
