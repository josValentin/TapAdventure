using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLogo : MonoBehaviour
{
    // Start is called before the first frame update
    private bool OnStart = true;
    private ManagerVars vars;
    private AdManager Ad_Manager;
    //private GameObject MainPanel;

    public AudioSource MenuMusic;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        Ad_Manager = GameObject.FindGameObjectWithTag("AdManager").GetComponent<AdManager>();

        //MainPanel = GameObject.FindGameObjectWithTag("MainPanel");

    }


    void Start()
    {
        //MainPanel.SetActive(false);
        if (OnStart != vars.OnStart)
        {
            GameObject Parent = GameObject.FindGameObjectWithTag("LogoStart");
            Destroy(Parent);
        }

        MenuMusic.volume = 0;
        MenuMusic.Stop();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyLogo()
    {
        
        if (OnStart == vars.OnStart)
        {
            /*if (!GameManager.Instance.RemoveAds)
            {
                //-----------------------------------------------------------------------------------Advertisment: BANNER_AD
                Ad_Manager.DisplayBannerAd();
                //UnityAdManager.showBanner();
                //-----------------------------------------------------------------------------------Advertisment: BANNER_AD
            }*/
            
            //
            GameObject Parent = GameObject.FindGameObjectWithTag("LogoStart");
            Destroy(Parent);
            //OnStart = false;
            EventCenter.Broadcast(EventDefine.ShowLogIngPanel);
            GameManager.Instance.GooglePlayServices.LogIn();
            //Change the language
            //GameManager.Instance.changeLenguaje();

            vars.OnStart = false;

        }

    }

    private void OnDestroy()
    {
        //MainPanel.SetActive(true);
        MenuMusic.Play();
        MenuMusic.DOFade(GameManager.Instance.GetMusicValue(), 2.3f);
        //MainPanel.GetComponent<MainPanel>().MenuMusic.DOFade(1, 2f);
        // GameObject.FindGameObjectWithTag("MainPanel").GetComponent<MainPanel>().;
    }


}
