using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePanel : MonoBehaviour
{
    private Image img_Bg;
    private GameObject dialog;


    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowUpdatePanel, Show);
        img_Bg = transform.GetChild(0).GetComponent<Image>();
        dialog = transform.GetChild(1).gameObject;
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;


        gameObject.SetActive(false);

    }
    void Start()
    {

    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowUpdatePanel, Show);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f), 0.3f);
        dialog.transform.DOScale(Vector3.one, 0.3f);

    }
}
