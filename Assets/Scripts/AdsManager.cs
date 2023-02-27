using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsManager : Singleton<AdsManager>
{
    private const string ANDROID_AD_ID = "5176995";
    private bool testMode = true;
    private string bannerID = "banner";

    public Button noAdsButton;

    void Start()
    {
        noAdsButton.onClick.AddListener(delegate { DisableAllAds(); });
        if (PurchaseManager.Instance.AreAdsDisabled())
        {
            noAdsButton.gameObject.SetActive(false);
            Advertisement.Banner.Hide();
        }
        else
        {
            Advertisement.Initialize(ANDROID_AD_ID, testMode);
            StartCoroutine(ShowBannerWhenInitialized());
        }
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

    public void DisableAllAds()
    {
        Debug.Log("Disabling ads");
        noAdsButton.gameObject.SetActive(false);
        StopAllCoroutines();
        Advertisement.Banner.Hide();
    }

}
