using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

#if EASY_MOBILE
using EasyMobile;
#endif

namespace CurvyPath
{
    public class UIManager : MonoBehaviour
    {
        [Header("Object References")]
        public GameObject mainCanvas;
        public GameObject characterSelectionUI;
        public GameObject header;
        public GameObject title;
        public Text score;
        public Text bestScore;
        public Text coinText;
        public GameObject newBestScore;
        public GameObject playBtn;
        public GameObject restartBtn;
        public GameObject menuButtons;
        public GameObject dailyRewardBtn;
        public Text dailyRewardBtnText;
        public GameObject AdsButton;
        public GameObject rewardUI;
        public GameObject settingsUI;
        public GameObject soundOnBtn;
        public GameObject soundOffBtn;
        public GameObject musicOnBtn;
        public GameObject musicOffBtn;
        public GameObject homeButton;
        public GameObject backgroundSelection;
        public GameObject SettingscloseButton;
        public GameObject PauseButton;
        public GameObject pauseMenu;

        [Header("Premium Features Buttons")]
        //public GameObject watchRewardedAdBtn;
        public GameObject leaderboardBtn;
        public GameObject achievementBtn;
        public GameObject shareBtn;
       // public GameObject iapPurchaseBtn;
        public GameObject removeAdsBtn;
        public GameObject restorePurchaseBtn;

        [Header("In-App Purchase Store")]
        public GameObject storeUI;

        [Header("Sharing-Specific")]
        public GameObject shareUI;
        public ShareUIController shareUIController;

        public GameObject[] CollectItems;

        Animator scoreAnimator;
        Animator dailyRewardAnimator;
        //bool isWatchAdsForCoinBtnActive;

        void OnEnable()
        {
            GameManager.GameStateChanged += GameManager_GameStateChanged;
            ScoreManager.ScoreUpdated += OnScoreUpdated;
        }

        void OnDisable()
        {
            GameManager.GameStateChanged -= GameManager_GameStateChanged;
            ScoreManager.ScoreUpdated -= OnScoreUpdated;
        }

        // Use this for initialization
        void Start()
        {
          
            scoreAnimator = score.GetComponent<Animator>();
            dailyRewardAnimator = dailyRewardBtn.GetComponent<Animator>();

            Reset();
            ShowStartUI();
        }

        // Update is called once per frame
        void Update()
        {
            score.text = ScoreManager.Instance.Score.ToString();
            bestScore.text = ScoreManager.Instance.HighScore.ToString();
            coinText.text = CoinManager.Instance.Coins.ToString();

            if (!DailyRewardController.Instance.disable && dailyRewardBtn.gameObject.activeInHierarchy)
            {
                if (DailyRewardController.Instance.CanRewardNow())
                {
                    dailyRewardBtnText.text = "GRAB YOUR REWARD!";
                   // dailyRewardAnimator.SetTrigger("activate");
                }
                else
                {
                    TimeSpan timeToReward = DailyRewardController.Instance.TimeUntilReward;
                    dailyRewardBtnText.text = string.Format("REWARD IN {0:00}:{1:00}:{2:00}", timeToReward.Hours, timeToReward.Minutes, timeToReward.Seconds);
                   // dailyRewardAnimator.SetTrigger("deactivate");
                }
            }

            if (settingsUI.activeSelf)
            {
                UpdateSoundButtons();
                UpdateMusicButtons();
            }
        }

        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if(newState==GameState.Playing)
            {
                newState = GameState.Playing;
                ShowGameUI();
            }
            /*
            if (newState == GameState.Playing)
            {
                newState = GameState.Playing;
                ShowGameUI();
            }
            */
            else if (newState == GameState.PreGameOver)
            {
                // Before game over, i.e. game potentially will be recovered
            }
            else if (newState == GameState.GameOver)
            {
                newState = GameState.GameOver;
                Invoke("ShowGameOverUI", 0.1f);
            }
        }

        void OnScoreUpdated(int newScore)
        {
            scoreAnimator.Play("NewScore");
        }

        void Reset()
        {
            mainCanvas.SetActive(true);
            characterSelectionUI.SetActive(false);
            header.SetActive(false);
            title.SetActive(false);
            score.gameObject.SetActive(false);
            newBestScore.SetActive(false);
            playBtn.SetActive(false);
            menuButtons.SetActive(false);
            dailyRewardBtn.SetActive(true);
            AdsButton.SetActive(true);
            //watchRewardedAdBtn.SetActive(false);

            // Enable or disable premium stuff
            bool enablePremium = IsPremiumFeaturesEnabled();
            leaderboardBtn.SetActive(enablePremium);
            shareBtn.SetActive(enablePremium);
            //iapPurchaseBtn.SetActive(enablePremium);
           // removeAdsBtn.SetActive(enablePremium);
           // restorePurchaseBtn.SetActive(enablePremium);

            // Hidden by default
            storeUI.SetActive(false);
            settingsUI.SetActive(false);
            shareUI.SetActive(false);

            // These premium feature buttons are hidden by default
            // and shown when certain criteria are met (e.g. rewarded ad is loaded)
           // watchRewardedAdBtn.gameObject.SetActive(true);
        }

        public void StartGame()
        {
            GameManager.Instance.StartGame();
        }

        public void EndGame()
        {
            GameManager.Instance.GameOver();
        }

        public void RestartGame()
        {
            GameManager.Instance.RestartGame(0.2f);
        }

        public void ShowStartUI()
        {
            settingsUI.SetActive(false);

            header.SetActive(true);
            title.SetActive(true);
            playBtn.SetActive(true);
            //watchRewardedAdBtn.SetActive(true);
            homeButton.SetActive(false);
            restartBtn.SetActive(false);
            menuButtons.SetActive(true);
            shareBtn.SetActive(false);
            PauseButton.SetActive(false);
            // If first launch: show "WatchForCoins" and "DailyReward" buttons if the conditions are met
            if (GameManager.GameCount == 0)
            {
                ShowDailyRewardBtn();
            }
        }

        public void ShowGameUI()
        {
            header.SetActive(true);
            title.SetActive(false);
            score.gameObject.SetActive(true);
            playBtn.SetActive(false);
            menuButtons.SetActive(false);
            dailyRewardBtn.SetActive(false);
            AdsButton.SetActive(false);
           // watchRewardedAdBtn.SetActive(false);
            PauseButton.SetActive(true);
            for (int i=0;i<=CollectItems.Length;i++)
            {
                if(PlayerPrefs.GetInt("ChooseItems")==1)
                {
                    CollectItems[0].SetActive(true);
                }
                if(PlayerPrefs.GetInt("ChooseItems")==2)
                {
                    CollectItems[1].SetActive(true);
                }
                if(PlayerPrefs.GetInt("ChooseItems")==3)
                {
                    CollectItems[2].SetActive(true);
                }
                if (PlayerPrefs.GetInt("ChooseItems") == 4)
                {
                    CollectItems[3].SetActive(true);
                }
                if (PlayerPrefs.GetInt("ChooseItems") == 5)
                {
                    CollectItems[4].SetActive(true);
                }
            }
            
        }

        public void ShowGameOverUI()
        {
            header.SetActive(true);
            title.SetActive(false);
            score.gameObject.SetActive(true);
            newBestScore.SetActive(ScoreManager.Instance.HasNewHighScore);

            playBtn.SetActive(false);
           // watchRewardedAdBtn.SetActive(false);
           // watchRewardedAdBtn.SetActive(false);
            restartBtn.SetActive(true);
            dailyRewardBtn.SetActive(false);
            homeButton.SetActive(true);
            menuButtons.SetActive(false);
            settingsUI.SetActive(false);
            for (int i = 0; i <= CollectItems.Length; i++)
            {
                if (PlayerPrefs.GetInt("ChooseItems") == 1)
                {
                    CollectItems[0].SetActive(false);
                }
                if (PlayerPrefs.GetInt("ChooseItems") == 2)
                {
                    CollectItems[1].SetActive(false);
                }
                if (PlayerPrefs.GetInt("ChooseItems") == 3)
                {
                    CollectItems[2].SetActive(false);
                }
                if (PlayerPrefs.GetInt("ChooseItems") == 4)
                {
                    CollectItems[3].SetActive(false);
                }
                if (PlayerPrefs.GetInt("ChooseItems") == 5)
                {
                    CollectItems[4].SetActive(false);
                }
            }

            // Show 'daily reward' button
            //showDailyRewardBtn();

          
            if (IsPremiumFeaturesEnabled())
            {
                ShowShareUI();
               
            }
        }


        public void pauseGame()
        {
            Time.timeScale=0;
            pauseMenu.SetActive(true);
        }
        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
           
        }


        void ShowDailyRewardBtn()
        {
            // Not showing the daily reward button if the feature is disabled
            if (!DailyRewardController.Instance.disable)
            {
                dailyRewardBtn.SetActive(true);
            }
        }

        public void ShowSettingsUI()
        {
            settingsUI.SetActive(true);
        }

        public void HideSettingsUI()
        {
            settingsUI.SetActive(false);
        }

        public void ShowStoreUI()
        {
            storeUI.SetActive(true);
        }

        public void HideStoreUI()
        {
            storeUI.SetActive(false);
        }

        public void GoToHomeFn(int index)
        {
            SceneManager.LoadScene(index);
            SoundManager.Instance.StopMusic();
            Time.timeScale = 1;
        }
        public void ShowCharacterSelectionScene()
        {
            mainCanvas.SetActive(false);
            characterSelectionUI.SetActive(true);
        }

        public void CloseCharacterSelectionScene()
        {
            mainCanvas.SetActive(true);
            characterSelectionUI.SetActive(false);

        }
        public void CloseBackgroundSelectionScene()
        {
            backgroundSelection.SetActive(false);
        }
        void OnCompleteRewardedAdToEarnCoins()
        {

        }

        public void GrabDailyReward()
        {
            if (DailyRewardController.Instance.CanRewardNow())
            {
                int reward = DailyRewardController.Instance.GetRandomReward();

                //Round the numbers and make multiples of 5 only
                int roundedReward = (reward / 5) * 5;

                // Show the reward UI
                ShowRewardUI(roundedReward);

                // Update next time for the reward
                DailyRewardController.Instance.ResetNextRewardTime();
            }
        }

        public void ShowRewardUI(int reward)
        {
            rewardUI.SetActive(true);
            rewardUI.GetComponent<RewardUIController>().Reward(reward);
        }

        public void HideRewardUI()
        {
            rewardUI.GetComponent<RewardUIController>().Close();
        }

        public void ShowLeaderboardUI()
        {
            #if EASY_MOBILE
        if (GameServices.IsInitialized())
        {
            GameServices.ShowLeaderboardUI();
        }
        else
        {
#if UNITY_IOS
            NativeUI.Alert("Service Unavailable", "The user is not logged in to Game Center.");
#elif UNITY_ANDROID
            GameServices.Init();
            #endif
        }
            #endif
        }

        public void ShowAchievementsUI()
        {
            #if EASY_MOBILE
        if (GameServices.IsInitialized())
        {
            GameServices.ShowAchievementsUI();
        }
        else
        {
#if UNITY_IOS
            NativeUI.Alert("Service Unavailable", "The user is not logged in to Game Center.");
#elif UNITY_ANDROID
            GameServices.Init();
            #endif
        }
            #endif
        }

        public void PurchaseRemoveAds()
        {
            #if EASY_MOBILE
        InAppPurchaser.Instance.Purchase(InAppPurchaser.Instance.removeAds);
            #endif
        }

        public void RestorePurchase()
        {
            #if EASY_MOBILE
        InAppPurchaser.Instance.RestorePurchase();
            #endif
        }

        public void ShowShareUI()
        {
            if (!ScreenshotSharer.Instance.disableSharing)
            {
                Texture2D texture = ScreenshotSharer.Instance.CapturedScreenshot;
                shareUIController.ImgTex = texture;

#if EASY_MOBILE
            AnimatedClip clip = ScreenshotSharer.Instance.RecordedClip;
            shareUIController.AnimClip = clip;
#endif

                shareUI.SetActive(true);
            }
        }

        public void HideShareUI()
        {
            shareUI.SetActive(false);
        }

        public void ToggleSound()
        {
            SoundManager.Instance.ToggleSound();
        }

        public void ToggleMusic()
        {
            SoundManager.Instance.ToggleMusic();
        }
        public void instagramLink()
        {
            Application.OpenURL("https://www.instagram.com/digimantralabs/");
        }
        public void RateApp()
        {
            Application.OpenURL("https://play.google.com/store/apps/developer?id=DigiMantra+Labs");
        }

        public void CloseApplication()
        {
            Application.Quit();
        }

        public void CloseSettingsMenu()
        {
            settingsUI.SetActive(false);
        }

        public void openMoreGames()
        {
            Application.OpenURL("https://play.google.com/store/apps/developer?id=DigiMantra+Labs");
        }

        public void ButtonClickSound()
        {
            Utilities.ButtonClickSound();
        }

        void UpdateSoundButtons()
        {
            if (SoundManager.Instance.IsSoundOff())
            {
                soundOnBtn.gameObject.SetActive(false);
                soundOffBtn.gameObject.SetActive(true);
            }
            else
            {
                soundOnBtn.gameObject.SetActive(true);
                soundOffBtn.gameObject.SetActive(false);
            }
        }

        void UpdateMusicButtons()
        {
            if (SoundManager.Instance.IsMusicOff())
            {
                musicOffBtn.gameObject.SetActive(true);
                musicOnBtn.gameObject.SetActive(false);
            }
            else
            {
                musicOffBtn.gameObject.SetActive(false);
                musicOnBtn.gameObject.SetActive(true);
            }
        }

        bool IsPremiumFeaturesEnabled()
        {
            return PremiumFeaturesManager.Instance != null && PremiumFeaturesManager.Instance.enablePremiumFeatures;
        }
    }
}