using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopPanel : MonoBehaviour
{
    public static ShopPanel instance;
    private ManagerVars vars;
    private Transform parent;
    private Text txt_Name;
    private Text txt_Description;
    private Text txt_Diamond;
    private Button btn_Back;
    private Button btn_Select;
    private Button btn_Buy;
    private Button btn_Info;
    private int selectIndex;

    public bool initialized = false;
    //public static ShopPanel instance;
    //private bool showInfo = false;

    private void Awake()
    {
        instance = this;
        EventCenter.AddListener(EventDefine.ShowShopPanel, Show);
        parent = transform.Find("ScroolRect/Parent");
        txt_Name = transform.Find("txt_Name").GetComponent<Text>();
        txt_Description = transform.Find("description").GetComponent<Text>();

        txt_Diamond = transform.Find("Diamond/Text").GetComponent<Text>();
        btn_Back = transform.Find("btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);
        btn_Select = transform.Find("btn_Select").GetComponent<Button>();
        btn_Select.onClick.AddListener(OnSelectButtonClick);
        btn_Buy = transform.Find("btn_Buy").GetComponent<Button>();
        btn_Buy.onClick.AddListener(OnBuyButtonClick);
        btn_Info = transform.Find("btn_Info").GetComponent<Button>();
        btn_Info.onClick.AddListener(OnInfoButtonClick);


        vars = ManagerVars.GetManagerVars();
    }
    private void Start()
    {
        //Init();
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowShopPanel, Show);
    }
    private void Show()
    {
        if (!initialized)
        {
            Init();
        }
        gameObject.SetActive(true);
        //showInfo = false;
        txt_Description.enabled = false;
    }
    /// <summary>
    /// 返回按钮点击
    /// </summary>
    private void OnBackButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 购买按钮点击
    /// </summary>
    private void OnBuyButtonClick()
    {



        int price = int.Parse(btn_Buy.GetComponentInChildren<Text>().text);
        if (price > GameManager.Instance.GetAllDiamond())
        {
            ClickAudio.Instance.setSelectAudio(1);
            EventCenter.Broadcast(EventDefine.Hint, "钻石不足");
            Debug.Log("钻石不足，不能购买");
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            return;
        }
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        GameManager.Instance.UpdateAllDiamond(-price);
        GameManager.Instance.SetSkinUnloacked(selectIndex);
        parent.GetChild(selectIndex).GetChild(0).GetComponent<Image>().color = Color.white;
    }
    /// <summary>
    /// 选择按钮点击
    /// </summary>
    private void OnSelectButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        EventCenter.Broadcast(EventDefine.ChangeSkin, selectIndex);
        GameManager.Instance.SetSelectedSkin(selectIndex);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }

    public int getSelectIndex()
    {
        return selectIndex;
    }

    private void OnInfoButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);

        /*if (!showInfo)
        {
            showInfo = true;
        }
        else
        {
            showInfo = false;
        }*/
        if(!txt_Description.enabled)
        {
            txt_Description.enabled = true;
        }
        else
        {
            txt_Description.enabled = false;

        }

        //EventCenter.Broadcast(EventDefine.ShowInfoPanel);
    }
    private GameObject go;
    public void Init()
    {
        initialized = true;
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.skinSpriteList.Count + 2) * 160, 302);
        for (int i = 0; i < vars.skinSpriteList.Count; i++)
        {
             go = Instantiate(vars.skinChooseItemPre, parent);
            //GameObject go = Instantiate(vars.skinChooseItemPre, parent);
            //未解锁
            if (GameManager.Instance.GetSkinUnlocked(i) == false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else//解锁了
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            go.transform.localPosition = new Vector3((i + 1) * 160, 0, 0);
        }
        //打开页面直接定位到选中的皮肤
        parent.transform.localPosition =
            new Vector3(GameManager.Instance.GetCurrentSelectedSkin() * -160, 0);

        
    }

    public void ReplaceInitFromLoadData()
    {
        for (int i = 0; i < vars.skinSpriteList.Count; i++)
        {

            if (GameManager.Instance.GetSkinUnlocked(i) == false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else//解锁了
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];


        }
    }
    private void Update()
    {
        selectIndex = (int)Mathf.Round(parent.transform.localPosition.x / -160.0f);
        if (Input.GetMouseButtonUp(0))
        {
            parent.transform.DOLocalMoveX(selectIndex * -160, 0.2f);
            //txt_Description.enabled = false;
            //parent.transform.localPosition = new Vector3(currentIndex * -160, 0);
        }
        SetItemSize(selectIndex);
        RefreshUI(selectIndex);
    }
    private void SetItemSize(int selectIndex)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (selectIndex == i)
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);

            }
            else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
                //txt_Description.enabled = false;

            }
        }
    }

    private string CanSurvive = "Can survive";
    private string DoubleCoins = "Double coins";
    //private string CanSurvive = "Can survive";
    private string TwoLife = "2 Life";
    private string SlowDownTimeTenSec = "Slow down time \n 10 seconds";
    private string DoubleScore = "Double score";

    public void ChangeLanguageShopPanel()
    {
        CanSurvive = GameManager.Instance.TextLenguaje[0];
        DoubleCoins = GameManager.Instance.TextLenguaje[1];
        TwoLife = GameManager.Instance.TextLenguaje[2];
        SlowDownTimeTenSec = GameManager.Instance.TextLenguaje[3];
        DoubleScore = GameManager.Instance.TextLenguaje[4];


    }

    private void RefreshUI(int selectIndex)
    {
        txt_Name.text = vars.skinNameList[selectIndex];
        txt_Diamond.text = GameManager.Instance.GetAllDiamond().ToString();


            if (selectIndex == 7)
            {

            if (!btn_Info.gameObject.activeSelf)
            {
                btn_Info.gameObject.SetActive(true);
            }
            txt_Description.text = CanSurvive;
            }
            else if (selectIndex == 8)
            {
            if (!btn_Info.gameObject.activeSelf)
            {
                btn_Info.gameObject.SetActive(true);
            }

            txt_Description.text = DoubleCoins;
            }
            else if (selectIndex == 9)
            {
            if (!btn_Info.gameObject.activeSelf)
            {
                btn_Info.gameObject.SetActive(true);
            }

            txt_Description.text = DoubleCoins;
            }
            else if (selectIndex == 10)
            {
            if (!btn_Info.gameObject.activeSelf)
            {
                btn_Info.gameObject.SetActive(true);
            }

            txt_Description.text = TwoLife;
            }

            else if (selectIndex == 11)
            {
            if (!btn_Info.gameObject.activeSelf)
            {
                btn_Info.gameObject.SetActive(true);
            }
            txt_Description.text = SlowDownTimeTenSec;
            }
            else if (selectIndex == 12)
            {
            if (!btn_Info.gameObject.activeSelf)
            {
                btn_Info.gameObject.SetActive(true);
            }
            txt_Description.text = DoubleScore;
            }
            else
            {
            if (btn_Info.gameObject.activeSelf)
            {
                btn_Info.gameObject.SetActive(false);
            }
            txt_Description.text = "";
            }

        //未解锁
        if (GameManager.Instance.GetSkinUnlocked(selectIndex) == false)
        {
            btn_Select.gameObject.SetActive(false);
            btn_Buy.gameObject.SetActive(true);
            btn_Buy.GetComponentInChildren<Text>().text = vars.skinPrice[selectIndex].ToString();
        }
        else
        {
            btn_Select.gameObject.SetActive(true);
            btn_Buy.gameObject.SetActive(false);
        }
    }
}
