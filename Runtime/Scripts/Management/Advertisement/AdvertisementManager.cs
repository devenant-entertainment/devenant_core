using System;
using UnityEngine;

namespace Devenant
{
    [RequireComponent(typeof(InitializableObject))]
    public class AdvertisementManager : Singleton<AdvertisementManager>, IInitializable
    {
        [SerializeField] private string androidKey;
        [SerializeField] private string iosKey;

        public void Initialize(Action<InitializationResponse> callback)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            string appKey = androidKey;
#elif UNITY_IPHONE && !UNITY_EDITOR
            string appKey = iosKey;
#else
            string appKey = "unexpected_platform";

            callback?.Invoke(new InitializationResponse(true));
#endif
            IronSourceEvents.onSdkInitializationCompletedEvent += () =>
            {
                callback?.Invoke(new InitializationResponse(true));
            };

            IronSource.Agent.validateIntegration();

            IronSource.Agent.init(appKey);
        }

        private void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        public void ShowBanner(string placement, IronSourceBannerSize size, IronSourceBannerPosition position)
        {
            Setup();

            IronSource.Agent.loadBanner(size, position, placement);

            void Setup()
            {
                IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
                IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
                IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
                IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
                IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
            }

            void Unsetup()
            {
                IronSourceBannerEvents.onAdLoadedEvent -= BannerOnAdLoadedEvent;
                IronSourceBannerEvents.onAdLoadFailedEvent -= BannerOnAdLoadFailedEvent;
                IronSourceBannerEvents.onAdClickedEvent -= BannerOnAdClickedEvent;
                IronSourceBannerEvents.onAdLeftApplicationEvent -= BannerOnAdLeftApplicationEvent;
                IronSourceBannerEvents.onAdScreenPresentedEvent -= BannerOnAdScreenPresentedEvent;
            }

            void BannerOnAdLoadedEvent(IronSourceAdInfo info)
            {
                IronSource.Agent.displayBanner();

                Unsetup();

                Debug.Log("AdvertisementManager: BannerOnAdLoadedEvent");
            }

            void BannerOnAdLoadFailedEvent(IronSourceError error)
            {
                Unsetup();

                ShowBanner(placement, size, position);

                Debug.Log("AdvertisementManager: BannerOnAdLoadFailedEvent");
            }

            void BannerOnAdClickedEvent(IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: BannerOnAdClickedEvent");
            }

            void BannerOnAdLeftApplicationEvent(IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: BannerOnAdLeftApplicationEvent");
            }

            void BannerOnAdScreenPresentedEvent(IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: BannerOnAdScreenPresentedEvent");
            }
        }

        public void HideBanner()
        {
            IronSource.Agent.destroyBanner();
        }

        public void ShowInterstitial(string placement, Action callback)
        {
            Setup();

            IronSource.Agent.loadInterstitial();

            void Setup()
            {
                IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
                IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
                IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
                IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
                IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
                IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
                IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
            }

            void Unsetup()
            {
                IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialOnAdReadyEvent;
                IronSourceInterstitialEvents.onAdLoadFailedEvent -= InterstitialOnAdLoadFailed;
                IronSourceInterstitialEvents.onAdOpenedEvent -= InterstitialOnAdOpenedEvent;
                IronSourceInterstitialEvents.onAdClickedEvent -= InterstitialOnAdClickedEvent;
                IronSourceInterstitialEvents.onAdShowSucceededEvent -= InterstitialOnAdShowSucceededEvent;
                IronSourceInterstitialEvents.onAdShowFailedEvent -= InterstitialOnAdShowFailedEvent;
                IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialOnAdClosedEvent;

                callback?.Invoke();
            }

            void InterstitialOnAdReadyEvent(IronSourceAdInfo info)
            {
                IronSource.Agent.showInterstitial(placement);

                Debug.Log("AdvertisementManager: InterstitialOnAdReadyEvent");
            }

            void InterstitialOnAdLoadFailed(IronSourceError error)
            {
                Unsetup();

                Debug.Log("AdvertisementManager: InterstitialOnAdLoadFailed");
            }

            void InterstitialOnAdOpenedEvent(IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: InterstitialOnAdOpenedEvent");
            }

            void InterstitialOnAdClickedEvent(IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: InterstitialOnAdClickedEvent");
            }

            void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: InterstitialOnAdShowSucceededEvent");
            }

            void InterstitialOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo info)
            {
                Unsetup();

                Debug.Log("AdvertisementManager: InterstitialOnAdShowFailedEvent");
            }

            void InterstitialOnAdClosedEvent(IronSourceAdInfo info)
            {
                Unsetup();

                Debug.Log("AdvertisementManager: InterstitialOnAdClosedEvent");
            }
        }

        public void ShowRewarded(string placement, Action<bool> callback)
        {
            Setup();

            IronSource.Agent.loadRewardedVideo();

            void Setup()
            {
                IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
                IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
                IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
                IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
                IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
                IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
                IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
            }

            void Unsetup(bool success)
            {
                IronSourceRewardedVideoEvents.onAdOpenedEvent -= RewardedVideoOnAdOpenedEvent;
                IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoOnAdClosedEvent;
                IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
                IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
                IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
                IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
                IronSourceRewardedVideoEvents.onAdClickedEvent -= RewardedVideoOnAdClickedEvent;
                IronSourceRewardedVideoEvents.onAdLoadFailedEvent -= RewardedOnAdLoadFailedEvent;
                IronSourceRewardedVideoEvents.onAdReadyEvent -= RewardedOnAdReadyEvent;

                callback?.Invoke(success);
            }

            void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedVideoOnAdOpenedEvent");
            }

            void RewardedVideoOnAdClosedEvent(IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedVideoOnAdClosedEvent");
            }

            void RewardedVideoOnAdAvailable(IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedVideoOnAdAvailable");
            }

            void RewardedVideoOnAdUnavailable()
            {
                Debug.Log("AdvertisementManager: RewardedVideoOnAdUnavailable");
            }

            void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedVideoOnAdShowFailedEvent");

                Unsetup(false);
            }

            void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedVideoOnAdRewardedEvent");

                Unsetup(true);
            }

            void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedVideoOnAdClickedEvent");
            }

            void RewardedOnAdLoadFailedEvent(IronSourceError error)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdLoadFailedEvent");

                Unsetup(false);
            }

            void RewardedOnAdReadyEvent(IronSourceAdInfo info)
            {
                Debug.Log("AdvertisementManager: RewardedOnAdReadyEvent");
            }
        }
    }
}
