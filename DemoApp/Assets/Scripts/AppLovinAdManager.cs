using System;
using UnityEngine;

public class AppLovinAdManager : MonoBehaviour
{
    public static AppLovinAdManager instance;
    [SerializeField] string MaxSdkKey = "M2_PBCB8OfZBytLAfr48zanu2sa1HrOvA5PbQa_59DrlcinKDqcscCRfV9fnrn6g3VvGypmBbErTlqkQEC8gqT";
    [SerializeField] string InterstitialAdUnitId = "33797672fd11aedc";
    [SerializeField] string RewardedAdUnitId = "cdc61084519c8f24";
    [SerializeField] string BannerAdUnitId = "efd97e8b9bd35066";
    private const string REMOVE_ADS = "RemoveAds";
    [Header("REMOVE ADS")] public bool removeAllAds;
    private Action successAction;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        if (removeAllAds)
            RemoveAds(removeAllAds);
        else if (!removeAllAds)
            RemoveAds(removeAllAds);

    }
    #region Remove Ads

    private bool CanShowAds()
    {
        if (!PlayerPrefs.HasKey(REMOVE_ADS))
            return true;
        else if (PlayerPrefs.GetInt(REMOVE_ADS) == 0)
            return true;

        return false;
    }

    public void RemoveAds(bool remove)
    {
        if (remove)
        {
            PlayerPrefs.SetInt(REMOVE_ADS, 1);
            HideBanner();
        }
        else
            PlayerPrefs.SetInt(REMOVE_ADS, 0);
    }

    #endregion
    void Start()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            InitializeInterstitialAds();
            InitializeRewardedAds();
            InitializeBannerAds();
        };
        MaxSdk.SetSdkKey(MaxSdkKey);
        MaxSdk.InitializeSdk();
    }
    #region Interstitial Ad Methods
    private void InitializeInterstitialAds()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;
        LoadInterstitial();
    }
    void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    }

    public void ShowInterstitial()
    {
        if (CanShowAds())
        {
            if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
            {
                MaxSdk.ShowInterstitial(InterstitialAdUnitId);
            }
            else
            {
                Debug.Log("Interstitial Ad not ready");
            }
        }
    }
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Interstitial loaded");
    }
    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        LoadInterstitial();
        Debug.Log("Interstitial failed to load with error code: " + errorInfo.Code);
    }
    private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Interstitial failed to display with error code: " + errorInfo.Code);
        LoadInterstitial();
    }
    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Interstitial dismissed");
        LoadInterstitial();
    }
    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Interstitial revenue paid");
    }
    #endregion
    #region Rewarded Ad Methods
    private void InitializeRewardedAds()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }
    public void ShowRewardedAd(Action onSuccess)
    {
        successAction = onSuccess;
        if (MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
        {
            MaxSdk.ShowRewardedAd(RewardedAdUnitId);
        }
        else
        {
            Debug.Log("Rewarded Ad not ready");
        }
    }
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad loaded");
    }
    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        LoadRewardedAd();
        Debug.Log("Rewarded ad failed to load with error code: " + errorInfo.Code);
    }
    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
        LoadRewardedAd();
    }
    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad displayed");
    }
    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad clicked");
    }
    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad dismissed");
        LoadRewardedAd();
    }
    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        RewardOfRewardedVideo();
        Debug.Log("Rewarded ad received reward");
    }
    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad revenue paid");
    }
    public void RewardOfRewardedVideo()
    {
        if (successAction != null)
        {
            successAction();
        }

        successAction = null;
    }
    #endregion
    #region Banner Ad Methods
    private void InitializeBannerAds()
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        Color transparent = new Color(0, 0, 0, 0);
        MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, transparent);
    }
    public void ShowBanner(MaxSdkBase.BannerPosition position)
    {
        if (CanShowAds())
        {

            MaxSdk.CreateBanner(BannerAdUnitId, position);
            MaxSdk.ShowBanner(BannerAdUnitId);
        }
    }
    public void HideBanner()
    {
        MaxSdk.HideBanner(BannerAdUnitId);
    }
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Banner ad loaded");
    }
    private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        Debug.Log("Banner ad failed to load with error code: " + errorInfo.Code);
    }
    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Banner ad clicked");
    }
    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Banner ad revenue paid");
    }
    #endregion
}