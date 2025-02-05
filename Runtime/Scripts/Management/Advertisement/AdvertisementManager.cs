using com.unity3d.mediation;
using System;
using UnityEngine;

namespace Devenant
{
    [RequireComponent(typeof(InitializableObject))]
    public class AdvertisementManager : Singleton<AdvertisementManager>, IInitializable
    {
        [Header("Application")]
        [SerializeField] private string androidKey;
        [SerializeField] private string iosKey;

        [Header("Banner")]
        [SerializeField] private string androidBannerKey;
        [SerializeField] private string iosBannerKey;

        [Header("Interstitial")]
        [SerializeField] private string androidInterstitialKey;
        [SerializeField] private string iosInterstitialKey;

        [Header("Rewarded")]
        [SerializeField] private string androidRewardedKey;
        [SerializeField] private string iosRewardedKey;
        
        private LevelPlayBannerAd currentBanner;
        private LevelPlayInterstitialAd currentInterstitial;
        private LevelPlayRewardedAd currentRewarded;

        public void Initialize(Action<InitializationResponse> callback)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            string appKey = androidKey;

            Debug.Log("AdvertisementManager: Initialize with Android appKey " + appKey);
#elif UNITY_IPHONE && !UNITY_EDITOR
            string appKey = iosKey;

            Debug.Log("AdvertisementManager: Initialize with iOS appKey " + appKey);
#else
            string appKey = "unexpected_platform";

            Debug.Log("AdvertisementManager: Initialize with null appKey " + appKey);

            callback?.Invoke(new InitializationResponse(true));
#endif

            LevelPlayAdFormat[] legacyAdFormats = new[] { LevelPlayAdFormat.BANNER, LevelPlayAdFormat.INTERSTITIAL, LevelPlayAdFormat.REWARDED };

            LevelPlay.OnInitSuccess += (levelPlayConfiguration) =>
            {
                Debug.Log("AdvertisementManager: OnInitSuccess");

                callback?.Invoke(new InitializationResponse(true));
            };

            LevelPlay.OnInitFailed += (levelPlayInitError) =>
            {
                Debug.LogError("AdvertisementManager: OnInitFailed => " + levelPlayInitError.ErrorMessage);

                callback?.Invoke(new InitializationResponse(false));
            };

            LevelPlay.Init(appKey, SystemInfo.deviceUniqueIdentifier, legacyAdFormats);
        }

        private void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        public void ShowBanner(string placement, LevelPlayAdSize size, LevelPlayBannerPosition position)
        {
#if UNITY_EDITOR
            return;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            string key = androidBannerKey;
#elif UNITY_IPHONE && !UNITY_EDITOR
            string key = iosBannerKey;
#else
            string key = string.Empty;
#endif

            currentBanner = new LevelPlayBannerAd(key, size, position, placement, true, true);
            currentBanner.LoadAd();

            Setup();

            void Setup()
            {
                currentBanner.OnAdLoaded += BannerOnAdLoaded;
                currentBanner.OnAdLoadFailed += BannerOnAdLoadFailed;
                currentBanner.OnAdClicked += BannerOnAdClicked;
                currentBanner.OnAdLeftApplication += BannerOnAdLeftApplication;
                currentBanner.OnAdCollapsed += BannerOnAdCollapsed;
                currentBanner.OnAdExpanded += BannerOnAdExpanded;
                currentBanner.OnAdDisplayed += BannerOnAdDisplayed;
                currentBanner.OnAdDisplayFailed += BannerOnAdDisplayedFailed;
            }

            void Unsetup()
            {
                currentBanner.OnAdLoaded -= BannerOnAdLoaded;
                currentBanner.OnAdLoadFailed -= BannerOnAdLoadFailed;
                currentBanner.OnAdClicked -= BannerOnAdClicked;
                currentBanner.OnAdLeftApplication -= BannerOnAdLeftApplication;
                currentBanner.OnAdCollapsed -= BannerOnAdCollapsed;
                currentBanner.OnAdExpanded -= BannerOnAdExpanded;
                currentBanner.OnAdDisplayed -= BannerOnAdDisplayed;
                currentBanner.OnAdDisplayFailed -= BannerOnAdDisplayedFailed;
            }

            void BannerOnAdLoaded(LevelPlayAdInfo info)
            {
                currentBanner.ShowAd();

                Unsetup();

                Debug.Log("AdvertisementManager: BannerOnAdLoaded");
            }

            void BannerOnAdLoadFailed(LevelPlayAdError error)
            {
                Unsetup();

                ShowBanner(placement, size, position);

                Debug.LogError("AdvertisementManager: BannerOnAdLoadFailed => " + error.ErrorMessage);
            }

            void BannerOnAdClicked(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: BannerOnAdClicked");
            }

            void BannerOnAdLeftApplication(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: BannerOnAdLeftApplication");
            }

            void BannerOnAdCollapsed(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: BannerOnAdCollapsed");
            }

            void BannerOnAdExpanded(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: BannerOnAdExpanded");
            }

            void BannerOnAdDisplayed(LevelPlayAdInfo info)
            {
                Unsetup();

                Debug.Log("AdvertisementManager: BannerOnAdDisplayed");
            }

            void BannerOnAdDisplayedFailed(LevelPlayAdDisplayInfoError error)
            {
                Unsetup();

                Debug.LogError("AdvertisementManager: BannerOnAdDisplayedFailed => " + error.LevelPlayError.ErrorMessage);
            }
        }

        public void HideBanner()
        {
            currentBanner?.HideAd();
        }

        public void ShowInterstitial(string placement, Action callback)
        {
#if UNITY_EDITOR
            callback?.Invoke();

            return;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            string key = androidInterstitialKey;
#elif UNITY_IPHONE && !UNITY_EDITOR
            string key = iosInterstitialKey;
#else
            string key = string.Empty;
#endif
            
            currentInterstitial = new LevelPlayInterstitialAd(key);
            currentInterstitial.LoadAd();

            Setup();

            void Setup()
            {
                currentInterstitial.OnAdLoaded += InterstitialOnAdLoaded;
                currentInterstitial.OnAdLoadFailed += InterstitialOnAdLoadFailed;
                currentInterstitial.OnAdClicked += InterstitialOnAdClicked;
                currentInterstitial.OnAdDisplayed += InterstitialOnAdDisplayed;
                currentInterstitial.OnAdDisplayFailed += InterstitialOnAdDisplayFailed;
                currentInterstitial.OnAdClosed += InterstitialOnAdClosed;
                currentInterstitial.OnAdInfoChanged += InterstitialOnAdInfoChanged;
            }

            void Unsetup()
            {
                currentInterstitial.OnAdLoaded -= InterstitialOnAdLoaded;
                currentInterstitial.OnAdLoadFailed -= InterstitialOnAdLoadFailed;
                currentInterstitial.OnAdClicked -= InterstitialOnAdClicked;
                currentInterstitial.OnAdDisplayed -= InterstitialOnAdDisplayed;
                currentInterstitial.OnAdDisplayFailed -= InterstitialOnAdDisplayFailed;
                currentInterstitial.OnAdClosed -= InterstitialOnAdClosed;
                currentInterstitial.OnAdInfoChanged -= InterstitialOnAdInfoChanged;

                callback?.Invoke();
            }

            void InterstitialOnAdLoaded(LevelPlayAdInfo info)
            {
                currentInterstitial.ShowAd(placement);

                Debug.Log("AdvertisementManager: InterstitialOnAdLoaded");
            }

            void InterstitialOnAdLoadFailed(LevelPlayAdError error)
            {
                Unsetup();

                Debug.LogError("AdvertisementManager: InterstitialOnAdLoadFailed => " + error.ErrorMessage);
            }

            void InterstitialOnAdClicked(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: InterstitialOnAdClicked");
            }

            void InterstitialOnAdDisplayed(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: InterstitialOnAdDisplayed");
            }

            void InterstitialOnAdDisplayFailed(LevelPlayAdDisplayInfoError error)
            {
                Unsetup();

                Debug.LogError("AdvertisementManager: InterstitialOnAdDisplayFailed => " + error.LevelPlayError.ErrorMessage);
            }

            void InterstitialOnAdClosed(LevelPlayAdInfo info)
            {
                Unsetup();

                Debug.Log("AdvertisementManager: InterstitialOnAdClosed");
            }

            void InterstitialOnAdInfoChanged(LevelPlayAdInfo info)
            {
                Unsetup();

                Debug.Log("AdvertisementManager: InterstitialOnAdInfoChanged");
            }
        }

        public void ShowRewarded(string placement, Action<bool> callback)
        {
#if UNITY_EDITOR
            callback?.Invoke(true); 
            
            return;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            string key = androidRewardedKey;
#elif UNITY_IPHONE && !UNITY_EDITOR
            string key = iosRewardedKey;
#else
            string key = string.Empty;
#endif
            currentRewarded = new LevelPlayRewardedAd(key);

            Setup();

            currentRewarded.LoadAd();

            void Setup()
            {
                currentRewarded.OnAdClicked += RewardedOnAdClicked;
                currentRewarded.OnAdClosed += RewardedOnAdClosed;
                currentRewarded.OnAdDisplayed += RewardedOnAdDisplayed;
                currentRewarded.OnAdDisplayFailed += RewardedOnAdDisplayFailed;
                currentRewarded.OnAdInfoChanged += RewardedOnAdInfoChanged;
                currentRewarded.OnAdLoaded += RewardedOnAdLoaded;
                currentRewarded.OnAdLoadFailed += RewardedOnAdLoadFailed;
                currentRewarded.OnAdRewarded += RewardedOnAdRewarded;
            }

            void Unsetup(bool success)
            {
                currentRewarded.OnAdClicked -= RewardedOnAdClicked;
                currentRewarded.OnAdClosed -= RewardedOnAdClosed;
                currentRewarded.OnAdDisplayed -= RewardedOnAdDisplayed;
                currentRewarded.OnAdDisplayFailed -= RewardedOnAdDisplayFailed;
                currentRewarded.OnAdInfoChanged -= RewardedOnAdInfoChanged;
                currentRewarded.OnAdLoaded -= RewardedOnAdLoaded;
                currentRewarded.OnAdLoadFailed -= RewardedOnAdLoadFailed;
                currentRewarded.OnAdRewarded -= RewardedOnAdRewarded;

                callback?.Invoke(success);
            }

            void RewardedOnAdClicked(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdClicked");
            }

            void RewardedOnAdClosed(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdClosed");
            }

            void RewardedOnAdDisplayed(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdDisplayed");
            }

            void RewardedOnAdDisplayFailed(LevelPlayAdDisplayInfoError error)
            {
                Debug.LogError("AdvertisementManager: RewardedOnAdDisplayFailed => " + error.LevelPlayError.ErrorMessage);

                Unsetup(false);
            }

            void RewardedOnAdInfoChanged(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdInfoChanged");
            }

            void RewardedOnAdLoaded(LevelPlayAdInfo info)
            {
                currentRewarded.ShowAd(placement);

                Debug.Log("AdvertisementManager: RewardedOnAdLoaded");
            }

            void RewardedOnAdLoadFailed(LevelPlayAdError error)
            {
                Debug.LogError("AdvertisementManager: RewardedOnAdLoadFailed => " + error.ErrorMessage);

                Unsetup(false);
            }

            void RewardedOnAdRewarded(LevelPlayAdInfo info, LevelPlayReward reward)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdRewarded");

                Unsetup(true);
            }
        }
    }
}
