using System;
using Unity.Services.LevelPlay;
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

        private string key;
        private string bannerKey;
        private string interstitialKey;
        private string rewardedKey;

        private LevelPlayBannerAd currentBanner;
        private LevelPlayInterstitialAd currentInterstitial;
        private LevelPlayRewardedAd currentRewarded;

        private bool hasAds = false;

        public void Initialize(Action<InitializationResponse> callback)
        {
            if (Application.isEditor)
            {
                callback?.Invoke(new InitializationResponse(true));

                return;
            }

#if UNITY_ANDROID
            key = androidKey;
            bannerKey = androidBannerKey;
            interstitialKey = androidInterstitialKey;
            rewardedKey = androidRewardedKey;

            Debug.Log("AdvertisementManager: Initialize with Android appKey " + key);
#else
            key = iosKey;
            bannerKey = iosBannerKey;
            interstitialKey = iosInterstitialKey;
            rewardedKey = iosRewardedKey;

            Debug.Log("AdvertisementManager: Initialize with iOS appKey " + key);
#endif

            com.unity3d.mediation.LevelPlayAdFormat[] legacyAdFormats = new[] { com.unity3d.mediation.LevelPlayAdFormat.BANNER, com.unity3d.mediation.LevelPlayAdFormat.INTERSTITIAL, com.unity3d.mediation.LevelPlayAdFormat.REWARDED };

            LevelPlay.OnInitSuccess += (levelPlayConfiguration) =>
            {
                Debug.Log("AdvertisementManager: OnInitSuccess");

                hasAds = true;

                callback?.Invoke(new InitializationResponse(true));
            };

            LevelPlay.OnInitFailed += (levelPlayInitError) =>
            {
                Debug.LogError("AdvertisementManager: OnInitFailed => " + levelPlayInitError.ErrorMessage);

                callback?.Invoke(new InitializationResponse(true));
            };

            LevelPlay.Init(key, SystemInfo.deviceUniqueIdentifier, legacyAdFormats);

            IronSource.Agent.shouldTrackNetworkState(true);
        }

        private void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        public void LoadBanner(string placement, com.unity3d.mediation.LevelPlayAdSize size, com.unity3d.mediation.LevelPlayBannerPosition position, Action<bool> callback)
        {
            if (!hasAds)
            {
                callback?.Invoke(true);

                return;
            }

            currentBanner?.HideAd();
            currentBanner?.DestroyAd();

            currentBanner = new LevelPlayBannerAd(bannerKey, size, position, placement, false, true);

            Setup();

            currentBanner.LoadAd();

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

            void Unsetup(bool success)
            {
                currentBanner.OnAdLoaded -= BannerOnAdLoaded;
                currentBanner.OnAdLoadFailed -= BannerOnAdLoadFailed;
                currentBanner.OnAdClicked -= BannerOnAdClicked;
                currentBanner.OnAdLeftApplication -= BannerOnAdLeftApplication;
                currentBanner.OnAdCollapsed -= BannerOnAdCollapsed;
                currentBanner.OnAdExpanded -= BannerOnAdExpanded;
                currentBanner.OnAdDisplayed -= BannerOnAdDisplayed;
                currentBanner.OnAdDisplayFailed -= BannerOnAdDisplayedFailed;

                callback?.Invoke(success);
            }

            void BannerOnAdLoaded(LevelPlayAdInfo info)
            {
                Unsetup(true);

                Debug.Log("AdvertisementManager: BannerOnAdLoaded");
            }

            void BannerOnAdLoadFailed(LevelPlayAdError error)
            {
                Unsetup(false);

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
                Debug.Log("AdvertisementManager: BannerOnAdDisplayed");
            }

            void BannerOnAdDisplayedFailed(LevelPlayAdDisplayInfoError error)
            {
                Debug.LogError("AdvertisementManager: BannerOnAdDisplayedFailed => " + error.LevelPlayError.ErrorMessage);
            }
        }

        public void ShowInterstitial(string placement, Action callback)
        {
            if (!hasAds)
            {
                callback?.Invoke();

                return;
            }

            currentInterstitial = new LevelPlayInterstitialAd(interstitialKey);
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
                currentInterstitial.OnAdInfoChanged += InterstitialOnAdInfoChanged;

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
            if (!hasAds)
            {
                callback?.Invoke(true);

                return;
            }

            currentRewarded = new LevelPlayRewardedAd(rewardedKey);
            currentRewarded.LoadAd();

            Setup();

            void Setup()
            {
                currentRewarded.OnAdClicked += OnAdClicked;
                currentRewarded.OnAdClosed += OnAdClosed;
                currentRewarded.OnAdDisplayed += OnAdDisplayed;
                currentRewarded.OnAdDisplayFailed += OnAdDisplayFailed;
                currentRewarded.OnAdInfoChanged += OnAdInfoChanged;
                currentRewarded.OnAdLoaded += OnAdLoaded;
                currentRewarded.OnAdLoadFailed += OnAdLoadFailed;
                currentRewarded.OnAdRewarded += OnAdRewarded;
            }

            void Unsetup(bool success)
            {
                currentRewarded.OnAdClicked -= OnAdClicked;
                currentRewarded.OnAdClosed -= OnAdClosed;
                currentRewarded.OnAdDisplayed -= OnAdDisplayed;
                currentRewarded.OnAdDisplayFailed -= OnAdDisplayFailed;
                currentRewarded.OnAdInfoChanged -= OnAdInfoChanged;
                currentRewarded.OnAdLoaded -= OnAdLoaded;
                currentRewarded.OnAdLoadFailed -= OnAdLoadFailed;
                currentRewarded.OnAdRewarded -= OnAdRewarded;

                callback?.Invoke(success);
            }

            void OnAdClicked(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdClicked");
            }

            void OnAdClosed(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdClosed");
            }

            void OnAdDisplayed(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdDisplayed");
            }

            void OnAdDisplayFailed(LevelPlayAdDisplayInfoError error)
            {
                Unsetup(false);

                Debug.Log("AdvertisementManager: RewardedOnAdDisplayFailed " + error.LevelPlayError.ErrorMessage);
            }

            void OnAdInfoChanged(LevelPlayAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdInfoChanged");
            }

            void OnAdLoaded(LevelPlayAdInfo info)
            {
                currentRewarded.ShowAd(placement);

                Debug.Log("AdvertisementManager: RewardedOnAdLoaded");
            }

            void OnAdLoadFailed(LevelPlayAdError error)
            {
                Unsetup(false);

                Debug.Log("AdvertisementManager: RewardedOnAdLoadFailed " + error.ErrorMessage);
            }

            void OnAdRewarded(LevelPlayAdInfo info, LevelPlayReward reward)
            {
                Unsetup(true);

                Debug.Log("AdvertisementManager: RewardedOnAdRewarded");
            }
        }
    }
}