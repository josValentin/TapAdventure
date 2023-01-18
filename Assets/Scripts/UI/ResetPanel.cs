using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResetPanel : MonoBehaviour
{
    public static ResetPanel instance;
    private Button btn_Yes;
    private Button btn_No;
    private Image img_Bg;
    private Image timer;
    private Text txt_time;
    private float ShowTimeRest = 10;
    private GameObject dialog;
    private Text diamondToRest;
    private TimeBody PlayerResetTime;
    private GameObject CostUI;
    private float cooldown = 10f;
    private float dt = 0;

    public Text txt_continue;

    private void Awake()
    {
        instance = this;
        EventCenter.AddListener(EventDefine.ShowResetPanel, Show);
        img_Bg = transform.Find("bg").GetComponent<Image>();
        timer = transform.Find("Dialog/Time").GetComponent<Image>();
        btn_Yes = transform.Find("Dialog/btn_Yes").GetComponent<Button>();
        btn_Yes.onClick.AddListener(OnYesButtonClick);
        btn_No = transform.Find("Dialog/btn_No").GetComponent<Button>();
        btn_No.onClick.AddListener(OnNoButtonClick);

        diamondToRest = transform.Find("Dialog/Cost").GetComponent<Text>();
        txt_time = transform.Find("Dialog/Time/txt_time").GetComponent<Text>();

        dialog = transform.Find("Dialog").gameObject;
        CostUI = transform.Find("Dialog/Cost").transform.gameObject;

        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowResetPanel, Show);
    }
    private void Show()
    {
        dt = Time.deltaTime;
        GameManager.Instance.changeLenguaje();
        if(PlayerController.Instance.WhichPlayer == 10)
        {
            CostUI.SetActive(false);
        }
        else
        {
            CostUI.SetActive(true);
        }
        gameObject.SetActive(true);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f), 0.3f);
        dialog.transform.DOScale(Vector3.one, 0.3f);
        PlayerResetTime = GameObject.FindGameObjectWithTag("Player").GetComponent<TimeBody>();

        diamondToRest.text = "- " + (int)(GameManager.Instance.GetGameDiamond() * 0.2f);
        txt_time.text = "" + (int)ShowTimeRest;
        StartCoroutine(StopTime());
    }

    private void Update()
    {
        //cooldown -= Time.deltaTime;
        timer.fillAmount -= 1.0f / cooldown * dt;
        ShowTimeRest -= dt;
        txt_time.text = "" + (int)ShowTimeRest;
        if(timer.fillAmount <= 0)
        {
            Time.timeScale = 1;
            EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
            gameObject.SetActive(false);
            //FrameTimingManager.CaptureFrameTimings();
            
            
        }


    }

    IEnumerator StopTime()
    {
        yield return new WaitForSeconds(0.3f);
        //调用结束面板
        //EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
        EventCenter.Broadcast(EventDefine.ShowResetPanel);
        Time.timeScale = 0;
    }
    /// <summary>
    /// 是按钮点击
    /// </summary>
    private void OnYesButtonClick()
    {
        Time.timeScale = 1;
        EventCenter.Broadcast(EventDefine.PlayClikAudio);


        //GameManager.Instance.ResetData();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.Fall = false;
        PlayerResetTime.StartRewind();
        if(PlayerController.Instance.WhichPlayer == 7)
        {
            GameManager.Instance.restDiamond((int)(GameManager.Instance.GetGameDiamond() * 0.2f));

        }

        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.3f);
        dialog.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        //Play again the music
        GameManager.Instance.MenuMusic.Play();
    }
    /// <summary>
    /// 否按钮点击
    /// </summary>
    private void OnNoButtonClick()
    {
        Time.timeScale = 1;
        EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.3f);
        dialog.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void ChangeLanguageResetPanel()
    {
        txt_continue.text = GameManager.Instance.TextLenguaje[35];
    }
}
