using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

namespace CurvyPath
{


    public class UnityAds : MonoBehaviour
    {
        string myplacementIdAndroid = "Rewarded_Android";
        string myplacementIdIos = "Rewarded_iOS";
        public Button _showRewardedAdButton;
        public GameObject BackGroundSelectionScreen;
        public bool isAdReady;
        public bool isAdReadyIos;
        public GameObject AdsnotReadyPanel;
        


        private void Start()
        {
            StartCoroutine(BannerAdsWhenReady());
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
           

        }

        public void ShowRewardedAd()
        {
            if(isAdReady)
            {
                Advertisement.Show(myplacementIdAndroid);
            }
            if(isAdReadyIos)
            {
                Advertisement.Show(myplacementIdIos);
            }
            else
            {
                StartCoroutine(notReadyPanel());
            }
        }
        IEnumerator notReadyPanel()
        {
            AdsnotReadyPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            AdsnotReadyPanel.SetActive(false);
        }

        IEnumerator BannerAdsWhenReady()
        {
            yield return new WaitForSeconds(0.5f);
            OnBannerLoaded();

        }
        public void LoadBannerAds()
        {
            BannerLoadOptions bannerLoadOptions = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };
            Advertisement.Banner.Load("Banner_Android", bannerLoadOptions);
        }

        public void OnBannerLoaded()
        {
            Debug.Log("Banner Loaded");
            Advertisement.Banner.Show("Banner_Android");
        }

        public void OnBannerError(string message)
        {
            Debug.Log("Banner Error:{message}");
        }


        public void ShowInterstialAd()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Advertisement.Show("Interstitial_Android");
            }
            else
            {
                Advertisement.Show("Interstitial_iOS");
            }
        }



        private void Update()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Advertisement.Initialize("5000103"); 
            }
            else
            {
                Advertisement.Initialize("5000102");
           
                
            }
        }

        public void OnUnityAdsReady(string placementId)
        {
           if(Application.platform==RuntimePlatform.Android)
            {
                if(placementId==myplacementIdAndroid)
                {
                    isAdReady = true;

                }    
            }
           else
            {
                if(placementId==myplacementIdIos)
                {
                    isAdReadyIos = true;
                }
            }
        }

        public void OnUnityAdsDidError(string message)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if(showResult==ShowResult.Finished)
            {
                if(placementId==myplacementIdAndroid && showResult==ShowResult.Finished)
                {
                    CoinManager.Instance.AddCoins(100);
                }
            }
            else if(showResult==ShowResult.Skipped)
            {
                Debug.Log("Ad Skipped");
            }
            else if(showResult==ShowResult.Failed)
            {
                Debug.Log("Failed");
            }

        }
    }
}
