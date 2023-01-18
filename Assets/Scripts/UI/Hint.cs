﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hint : MonoBehaviour
{
    public static Hint instance;
    private Image img_Bg;
    private Text txt_Hint;

    private void Awake()
    {
        instance = this;
        img_Bg = GetComponent<Image>();
        txt_Hint = GetComponentInChildren<Text>();
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        txt_Hint.color = new Color(txt_Hint.color.r, txt_Hint.color.g, txt_Hint.color.b, 0);

        EventCenter.AddListener<string>(EventDefine.Hint, Show);
    }

    private void Start()
    {
        //ClickAudio.Instance.setSelectAudio(1);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.Hint, Show);
    }
    private void Show(string text)
    {
        StopCoroutine("Dealy");
        transform.localPosition = new Vector3(0, -70, 0);
        transform.DOLocalMoveY(0, 0.3f).OnComplete(() =>
        {
            StartCoroutine("Dealy");
        });
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.4f), 0.1f);
        txt_Hint.DOColor(new Color(txt_Hint.color.r, txt_Hint.color.g, txt_Hint.color.b, 1), 0.1f);
    }
    private IEnumerator Dealy()
    {
        yield return new WaitForSeconds(1f);
        transform.DOLocalMoveY(70, 0.3f);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.1f);
        txt_Hint.DOColor(new Color(txt_Hint.color.r, txt_Hint.color.g, txt_Hint.color.b, 0), 0.1f);
    }

    public void ChangeLanguageHintPanel()
    {
        txt_Hint.text = GameManager.Instance.TextLenguaje[36];
    }
}
