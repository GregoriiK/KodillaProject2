using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : Singleton<AdsManager>
{
    private const string ANDROID_AD_ID = "5176995";
    private bool testMode = true;
    private string bannerID = "banner";
    void Start()
    {
        Advertisement.Initialize(ANDROID_AD_ID, testMode);
        StartCoroutine(ShowBannerWhenInitialized());
    }

    private IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Show(bannerID);
    }
}
