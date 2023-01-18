using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class HelpPanel : MonoBehaviour
{
    //public static HelpPanel instance;
    private Image img_Bg;
    private GameObject dialog;

    [SerializeField]
    private Button btn_Close;
    [SerializeField]
    private Text txt_close;


    public TextMeshProUGUI GemsInformation;
    public Text Stairs;
    public TextMeshProUGUI HowToPlay;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Left;
    public TextMeshProUGUI Right;


    private void Awake()
    {
        //instance = this;
        EventCenter.AddListener(EventDefine.ShowHelpPanel, Show);
        img_Bg = transform.GetChild(0).GetComponent<Image>();
        dialog = transform.GetChild(1).gameObject;
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;
        //btn_Close = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);

        gameObject.SetActive(false);

    }
    void Start()
    {
        
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowHelpPanel, Show);
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
        });
    }

    public void ChangeLanguageHelpPanel()
    {
        txt_close.text = GameManager.Instance.TextLenguaje[6];
        GemsInformation.text = GameManager.Instance.TextLenguaje[20];
        Stairs.text = GameManager.Instance.TextLenguaje[21] + " = 0+ \n" + GameManager.Instance.TextLenguaje[21]+ " = 150+ \n" + GameManager.Instance.TextLenguaje[21] + " = 500+" ;
        HowToPlay.text = GameManager.Instance.TextLenguaje[22];
        Description.text = GameManager.Instance.TextLenguaje[23];
        Left.text = GameManager.Instance.TextLenguaje[24];
        Right.text = GameManager.Instance.TextLenguaje[25];


    }

}
