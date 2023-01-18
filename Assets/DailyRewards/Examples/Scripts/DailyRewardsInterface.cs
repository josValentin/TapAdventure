/***************************************************************************\
Project:      Daily Rewards
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace NiobiumStudios
{
    /**
     * The UI Logic Representation of the Daily Rewards
     **/
    public class DailyRewardsInterface : MonoBehaviour
    {
        public Canvas canvas;
        public GameObject dailyRewardPrefab;        // Prefab containing each daily reward

        [Header("Panel Debug")]
		public bool isDebug;
        public GameObject panelDebug;
		public Button buttonAdvanceDay;
		public Button buttonAdvanceHour;
		public Button buttonReset;
		public Button buttonReloadScene;

        [Header("Panel Reward Message")]
        public GameObject panelReward;              // Rewards panel
        public Text textReward;                     // Reward Text to show an explanatory message to the player
        public Button buttonCloseReward;            // The Button to close the Rewards Panel
        public Image imageReward;                   // The image of the reward

        [Header("Panel Reward")]
        public Button buttonClaim;                  // Claim Button
        public Button buttonClose;                  // Close Button
        public Button buttonCloseWindow;            // Close Button on the upper right corner
        public Text textTimeDue;                    // Text showing how long until the next claim
        public GridLayoutGroup dailyRewardsGroup;   // The Grid that contains the rewards
        public ScrollRect scrollRect;               // The Scroll Rect

        private bool readyToClaim;                  // Update flag
        private List<DailyRewardUI> dailyRewardsUI = new List<DailyRewardUI>();

		private DailyRewards dailyRewards;          // DailyReward Instance      

        [HideInInspector]
        public bool WillAppear = false;

        //Language
        public Text DailyRewardText;
        private string ComebackForNextReward = "Come back in {0} for your next reward";
        private string _Day = "Day ";
        public Text RewardText;
        private string youGot = "You got ";
        private string YouCanClaimReward = "You can claim your reward!";



        public Image img_Bg;
        public GameObject dialog;

        void Awake()
        {
            canvas.gameObject.SetActive(false);
			dailyRewards = GetComponent<DailyRewards>();

            img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
            dialog.transform.localScale = Vector3.zero;

        }

        public void ChangeLanguageDailyRewardsInterface()
        {

                DailyRewardText.text = GameManager.Instance.TextLenguaje[28];
                ComebackForNextReward = GameManager.Instance.TextLenguaje[29];

                _Day = GameManager.Instance.TextLenguaje[30];
                /*for (int i = 0; i < dailyRewardsUI.Count; i++)
                {

                    dailyRewardsUI[i].DayString = _Day;
                    dailyRewardsUI[i].textDay.text = string.Format(dailyRewardsUI[i].DayString + "{0}", dailyRewardsUI[i].day.ToString());
                }*/


                buttonClose.transform.GetChild(2).GetComponent<Text>().text = GameManager.Instance.TextLenguaje[6];
                RewardText.text = GameManager.Instance.TextLenguaje[31];
                youGot = GameManager.Instance.TextLenguaje[32];
                YouCanClaimReward = GameManager.Instance.TextLenguaje[33];
                buttonClaim.transform.GetChild(2).GetComponent<Text>().text = GameManager.Instance.TextLenguaje[34];
                buttonCloseReward.transform.GetChild(2).GetComponent<Text>().text = GameManager.Instance.TextLenguaje[6];
            

        }

        public void Show()
        {
            if (WillAppear)
            {

                var showWhenNotAvailable = dailyRewards.keepOpen;
                var isRewardAvailable = dailyRewards.availableReward > 0;

                UpdateUI();
                canvas.gameObject.SetActive(showWhenNotAvailable || (!showWhenNotAvailable && isRewardAvailable));

                SnapToReward();
                CheckTimeDifference();

                StartCoroutine(TickTime());

                //canvas.gameObject.SetActive(true);
                img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f), 0.3f);
                dialog.transform.DOScale(Vector3.one, 0.3f);
            }
            else
            {
                Debug.Log("The user is not reconized, Rewards panel disabled");
            }

        }
        void Start()
        {
            InitializeDailyRewardsUI();

            if (panelDebug)
                panelDebug.SetActive(isDebug);

            buttonClose.gameObject.SetActive(false);

            buttonClaim.onClick.AddListener(() =>
            {
                //Select Type Of Rewarded Video
                if (!GameManager.Instance.vars.RemoveAds)
                {
                    AdManager.instance.SetClaimRewarded_JustWatch(2);
                    AdManager.instance.DisplayVideoAd(VideoAdIsNotReady);

                    //TO DELETE
                    //VideoAdIsNotReady();
                }
                else
                {
                    VideoAdIsNotReady();

                }

                //
                //dailyRewards.ClaimPrize();
                //readyToClaim = false;
                //UpdateUI();
            });

            buttonCloseReward.onClick.AddListener(() =>
            {
				var keepOpen = dailyRewards.keepOpen;
                panelReward.SetActive(false);
                canvas.gameObject.SetActive(keepOpen);
            });

            buttonClose.onClick.AddListener(() =>
            {
                //canvas.gameObject.SetActive(false);
                OnCloseButtonClick();
            });

            buttonCloseWindow.onClick.AddListener(() =>
            {
                //canvas.gameObject.SetActive(false);
                OnCloseButtonClick();

            });

            // Simulates the next Day
            if (buttonAdvanceDay)
				buttonAdvanceDay.onClick.AddListener(() =>
				{
                    dailyRewards.debugTime = dailyRewards.debugTime.Add(new TimeSpan(1, 0, 0, 0));
                    UpdateUI();
				});

			// Simulates the next hour
			if(buttonAdvanceHour)
				buttonAdvanceHour.onClick.AddListener(() =>
              	{
                      dailyRewards.debugTime = dailyRewards.debugTime.Add(new TimeSpan(1, 0, 0));
                      UpdateUI();
				});

			if(buttonReset)
				// Resets Daily Rewards from Player Preferences
				buttonReset.onClick.AddListener(() =>
				{
					dailyRewards.Reset();
                    dailyRewards.debugTime = new TimeSpan();
                    dailyRewards.lastRewardTime = System.DateTime.MinValue;
					readyToClaim = false;
				});

			// Reloads the same scene
			if(buttonReloadScene)
				buttonReloadScene.onClick.AddListener(() =>
				{
					Application.LoadLevel (Application.loadedLevel);
				});


            

			UpdateUI();
        }

        public void CreateTimer()
        {
            dailyRewards.Reset();
            dailyRewards.debugTime = new TimeSpan();
            dailyRewards.lastRewardTime = System.DateTime.MinValue;
            readyToClaim = false;
        }

        private void OnCloseButtonClick()
        {
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
            img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.3f);
            dialog.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                canvas.gameObject.SetActive(false);
            });
        }
        public void VideoAdIsNotReady()
        {
            dailyRewards.ClaimPrize();
            readyToClaim = false;
            UpdateUI();
        }

        void OnEnable()
        {
            dailyRewards.onClaimPrize += OnClaimPrize;
            dailyRewards.onInitialize += OnInitialize;
        }

        void OnDisable()
        {
            if (dailyRewards != null)
            {
                dailyRewards.onClaimPrize -= OnClaimPrize;
                dailyRewards.onInitialize -= OnInitialize;
            }
        }

        // Initializes the UI List based on the rewards size
        private void InitializeDailyRewardsUI()
        {
            for (int i = 0; i < dailyRewards.rewards.Count; i++)
            {
                int day = i + 1;
                var reward = dailyRewards.GetReward(day);

                GameObject dailyRewardGo = GameObject.Instantiate(dailyRewardPrefab) as GameObject;

                DailyRewardUI dailyRewardUI = dailyRewardGo.GetComponent<DailyRewardUI>();
                //dailyRewardUI.DayString = _Day;
                dailyRewardUI.transform.SetParent(dailyRewardsGroup.transform);
                dailyRewardGo.transform.localScale = Vector2.one;

                dailyRewardUI.DayString = _Day;
                dailyRewardUI.day = day;
                dailyRewardUI.reward = reward;
                dailyRewardUI.Initialize();

                dailyRewardsUI.Add(dailyRewardUI);
            }
        }

        public void UpdateUI()
        {
            dailyRewards.CheckRewards();

            bool isRewardAvailableNow = false;

            var lastReward = dailyRewards.lastReward;
            var availableReward = dailyRewards.availableReward;

            foreach (var dailyRewardUI in dailyRewardsUI)
            {
                var day = dailyRewardUI.day;

                if (day == availableReward)
                {
                    dailyRewardUI.state = DailyRewardUI.DailyRewardState.UNCLAIMED_AVAILABLE;

                    isRewardAvailableNow = true;
                }
                else if (day <= lastReward)
                {
                    dailyRewardUI.state = DailyRewardUI.DailyRewardState.CLAIMED;
                }
                else
                {
                    dailyRewardUI.state = DailyRewardUI.DailyRewardState.UNCLAIMED_UNAVAILABLE;
                }

                dailyRewardUI.Refresh();
            }

            buttonClaim.gameObject.SetActive(isRewardAvailableNow);
            buttonClose.gameObject.SetActive(!isRewardAvailableNow);
            if (isRewardAvailableNow)
            {
                SnapToReward();
                textTimeDue.text = YouCanClaimReward;
            }
            readyToClaim = isRewardAvailableNow;
        }

        // Snap to the next reward
        public void SnapToReward()
        {
            Canvas.ForceUpdateCanvases();

            var lastRewardIdx = dailyRewards.lastReward;

            // Scrolls to the last reward element
            if (dailyRewardsUI.Count - 1 < lastRewardIdx)
                lastRewardIdx++;

			if(lastRewardIdx > dailyRewardsUI.Count - 1)
				lastRewardIdx = dailyRewardsUI.Count - 1;

            var target = dailyRewardsUI[lastRewardIdx].GetComponent<RectTransform>();

            var content = scrollRect.content;

            //content.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(content.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            float normalizePosition = (float)target.GetSiblingIndex() / (float)content.transform.childCount;
            scrollRect.verticalNormalizedPosition = normalizePosition;
        }

        private void CheckTimeDifference ()
        {
            if (!readyToClaim)
            {
                TimeSpan difference = dailyRewards.GetTimeDifference();

                // If the counter below 0 it means there is a new reward to claim
                if (difference.TotalSeconds <= 0)
                {
                    readyToClaim = true;
                    UpdateUI();
                    SnapToReward();
                    return;
                }

                string formattedTs = dailyRewards.GetFormattedTime(difference);

                textTimeDue.text = string.Format(ComebackForNextReward, formattedTs);
            }
        }

        // Delegate
        private void OnClaimPrize(int day)
        {
            panelReward.SetActive(true);

            var reward = dailyRewards.GetReward(day);
            var unit = reward.unit;
            var rewardQt = reward.reward;
            imageReward.sprite = reward.sprite;
            if (rewardQt > 0)
            {
                textReward.text = string.Format(youGot + "{0} {1}!", reward.reward, unit);
            }
            else
            {
                textReward.text = string.Format(youGot + "{0}!", unit);
            }

            //Sound
            ClickAudio.Instance.setSelectAudio(2);
            EventCenter.Broadcast(EventDefine.PlayClikAudio);
        }
        private void OnInitialize(bool error, string errorMessage)
        {
            if (!error)
            {

                WillAppear = true;
                /*var showWhenNotAvailable = dailyRewards.keepOpen;
                var isRewardAvailable = dailyRewards.availableReward > 0;

                UpdateUI();
                canvas.gameObject.SetActive(showWhenNotAvailable || (!showWhenNotAvailable && isRewardAvailable));

                SnapToReward();
                CheckTimeDifference();

				StartCoroutine(TickTime());*/
            }
        }

		private IEnumerator TickTime() {
			for(;;){
				dailyRewards.TickTime();
				// Updates the time due
				CheckTimeDifference();
				yield return null;
			}
		}


    }
}