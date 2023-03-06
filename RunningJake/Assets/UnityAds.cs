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
        public Button _showRewardedAdButton;
        public GameObject BackGroundSelectionScreen;

        


        private void Start()
        {
            StartCoroutine(BannerAdsWhenReady());
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
           

        }

        public void ShowRewardedAd()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
               Advertisement.Show("Rewarded_Android");
               
            }
            else
            {
                Advertisement.Show("Rewarded_iOS");
            }
            CoinManager.Instance.AddCoins(50);
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
            throw new System.NotImplementedException();
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
        }
    }
}
