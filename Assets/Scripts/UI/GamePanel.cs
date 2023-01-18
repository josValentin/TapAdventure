using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    private Button btn_Pause;
    private Button btn_Play;
    private Button btn_Skill;
    private Image SkillColdown;
    private Text txt_time; 
    private float ShowTimeRest = 30;
    private float cooldown = 30f;
    private Text txt_Score;
    private Text txt_DiamondCount;
    private Image img_Guide;

    private const int SONIC_SKILL = 1;
    private bool DropSlowly = false;
    private int SKILLS_ELECTED = 0;

    private float AnimColorCooldown = 1;
    private float NegPosAnim = 0.005f;
    //private bool ButtonSkillEnabled = false;

    //private ManagerVars vars;
    //private AdManager Ad_Manager;


    private void Awake()
    {
        //Ad_Manager = GameObject.FindGameObjectWithTag("AdManager").GetComponent<AdManager>();
        EventCenter.AddListener(EventDefine.ShowGamePanel, Show);
        EventCenter.AddListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
        EventCenter.AddListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
        //vars = ManagerVars.GetManagerVars();
        Init();
    }
    private void Init()
    {
        btn_Pause = transform.Find("btn_Pause").GetComponent<Button>();
        btn_Pause.onClick.AddListener(OnPauseButtonClick);
        btn_Play = transform.Find("btn_Play").GetComponent<Button>();
        btn_Play.onClick.AddListener(OnPlayButtonClick);
        btn_Skill = transform.Find("btn_Skill").GetComponent<Button>();
        btn_Skill.onClick.AddListener(OnSkillButtonClick);
        SkillColdown = transform.Find("TimerSkill/Timer").GetComponent<Image>();
        txt_time = transform.Find("TimerSkill/txt_time").GetComponent<Text>();

        img_Guide = transform.Find("img_Guide").GetComponent<Image>();
        txt_Score = transform.Find("txt_Score").GetComponent<Text>();
        txt_DiamondCount = transform.Find("Diamond/txt_DiamondCount").GetComponent<Text>();
        btn_Play.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePanel, Show);
        EventCenter.RemoveListener<int>(EventDefine.UpdateScoreText, UpdateScoreText);
        EventCenter.RemoveListener<int>(EventDefine.UpdateDiamondText, UpdateDiamondText);
    }
    private void Show()
    {
        //print(ButtonSkillEnabled());
        print(PlayerController.Instance.GetPlayerSpriteRenderer().sprite);
        print(ManagerVars.GetManagerVars().characterSkinSpriteList[7]);

        gameObject.SetActive(true);
        //Ad_Manager.DisplayBannerAd();
        img_Guide.gameObject.SetActive(true);


        txt_time.text = "" + (int)ShowTimeRest;


    }

    private void Update()
    {
        if (ButtonSkillEnabled() && !btn_Skill.gameObject.activeSelf)
        {
            btn_Skill.gameObject.SetActive(true);
            print("ga");
        }

        if (SkillColdown.transform.parent.gameObject.activeSelf)
        {
            SkillColdown.fillAmount += 1.0f / cooldown * Time.deltaTime;
            ShowTimeRest -= Time.deltaTime;
            txt_time.text = "" + (int)ShowTimeRest;
            if(ShowTimeRest > 20)
            {
                //AnimColorCooldown -= 0.05f;

                if (AnimColorCooldown <= 0.35f)
                {
                    NegPosAnim = 0.005f;
                } else if (AnimColorCooldown >= 1f)
                {
                    NegPosAnim = -0.005f;
                }
                AnimColorCooldown += NegPosAnim;
                SkillColdown.transform.parent.GetComponent<Image>().color = new Color(AnimColorCooldown, AnimColorCooldown, AnimColorCooldown);
            }
            else
            {
                SkillColdown.transform.parent.GetComponent<Image>().color = Color.white;
            }

            if (SkillColdown.fillAmount >= 1)
            {
                //Time.timeScale = 1;
                //EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
                //gameObject.SetActive(false);
                //FrameTimingManager.CaptureFrameTimings();

                btn_Skill.gameObject.SetActive(true);
                //SkillColdown.gameObject.SetActive(false);
                SkillColdown.transform.parent.gameObject.SetActive(false);
                btn_Skill.image.color = Color.white;
                SkillColdown.fillAmount = 0;
                ShowTimeRest = 30;
            }
        }


    }
    /// <summary>
    /// 更新成绩显示
    /// </summary>
    /// <param name="score"></param>
    private void UpdateScoreText(int score)
    {
        txt_Score.text = score.ToString();
    }
    /// <summary>
    /// 更新钻石数量显示
    /// </summary>
    /// <param name="diamond"></param>
    private void UpdateDiamondText(int diamond)
    {
        txt_DiamondCount.text = diamond.ToString();
    }
    /// <summary>
    /// 暂停按钮点击
    /// </summary>
    private void OnPauseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        btn_Play.gameObject.SetActive(true);
        btn_Pause.gameObject.SetActive(false);
        //游戏暂停
        Time.timeScale = 0;
        GameManager.Instance.IsPause = true;
    }
    /// <summary>
    /// 开始按钮点击
    /// </summary>
    private void OnPlayButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClikAudio);
        btn_Play.gameObject.SetActive(false);
        btn_Pause.gameObject.SetActive(true);
        //继续游戏
        Time.timeScale = 1;
        GameManager.Instance.IsPause = false;
    }
    public bool getDropSlowly()
    {
        return DropSlowly;
    }

    private bool ButtonSkillEnabled()
    {
        if (PlayerController.Instance.WhichPlayer == 11)
        {
            btn_Skill.image.sprite = ManagerVars.GetManagerVars().SpritesButtonsSkills[0]; 
            SKILLS_ELECTED = SONIC_SKILL;
            return true;
         } else
        {
            return false;
        }      
    }

    private void OnSkillButtonClick()
    {
        switch (SKILLS_ELECTED)
        {
            case SONIC_SKILL:
                DropSlowly = true;
                StartCoroutine(StopSkill());
                break;

            
        }

        //btn_Skill.gameObject.SetActive(true);

        btn_Skill.image.color = new Color(0.35f, 0.35f, 0.35f);
        SkillColdown.transform.parent.gameObject.SetActive(true);
    }

    IEnumerator StopSkill()
    {
        yield return new WaitForSeconds(10f);
        DropSlowly = false;
        print("stop skill");
    }

      
    }
