using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    private Image img_Bg;
    private GameObject dialog;
    private TextMeshProUGUI txt_Info;

    private Button btn_Close;
    // Start is called before the first frame update
    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowInfoPanel, Show);
        //img_Bg = transform.Find("bg").GetComponent<Image>();
        img_Bg = transform.GetChild(0).GetComponent<Image>();
        //dialog = transform.Find("Dialog").gameObject;
        dialog = transform.GetChild(1).gameObject;
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;
        //btn_Close = transform.Find("Dialog/btn_Close").GetComponent<Button>();
        btn_Close = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);
        txt_Info = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);

    }
    void Start()
    {

    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowInfoPanel, Show);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f), 0.3f);
        dialog.transform.DOScale(Vector3.one, 0.3f);
        int SelectedIndex = gameObject.GetComponentInParent<ShopPanel>().getSelectIndex();
        if(SelectedIndex == 7)
        {
            txt_Info.text = "Zombie has a second opportunity coming back to the last point in the game. \n \n Cost: 20% of the gems collected";

        }
        else if(SelectedIndex == 8)
        {
            txt_Info.text = "St. Patrick get the double value for each gem.";
        }
        else if (SelectedIndex == 9)
        {
            txt_Info.text = "Sonic is able to make the blocks fall slowly for 10 seconds. \n \n Cooldown: 30 seconds";

        }
        else if (SelectedIndex == 10)
        {
            txt_Info.text = "The Flash get double score climbing the blocks.";
        }
        else
        {
            txt_Info.text = "";

        }
        print(SelectedIndex);

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
}
