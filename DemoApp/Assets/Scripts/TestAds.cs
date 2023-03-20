using UnityEngine;
using UnityEngine.UI;

public class TestAds : MonoBehaviour
{
    public Text rewardText;
    void Start()
    {
        AppLovinAdManager.instance.ShowBanner(MaxSdkBase.BannerPosition.TopCenter);
    }
    public void Inter()
    {
        AppLovinAdManager.instance.ShowInterstitial();
    }
    public void Rewarded()
    {
        AppLovinAdManager.instance.ShowRewardedAd(Reward);
    }
    private void Reward()
    {
        rewardText.text = "Reward Given by AppLovin Rewarded Successful";
        Invoke(nameof(ResetText), 2f);
    }
    void ResetText()
    {
        rewardText.text = "";
    }
    public void Debugger()
    {
        MaxSdk.ShowMediationDebugger();
    }
}
