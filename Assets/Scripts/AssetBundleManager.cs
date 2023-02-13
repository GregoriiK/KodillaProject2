using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    public string AssetBundleURL;
    public string AssetBundleName;
    public uint AbVersion;
    public string AbVersionURL;
    public string BallSpriteName;
    private AssetBundle ab;

    private IEnumerator Start()
    {
        //yield return StartCoroutine(GetAbVersion());
        yield return StartCoroutine(LoadAssets(AssetBundleName, result => ab = result));
        //yield return StartCoroutine(LoadSingleAsset(BallSpriteName));
        //yield return StartCoroutine(LoadAssetFromURL());
    }

    private IEnumerator LoadAssets(string name, Action<AssetBundle> bundle)
    {
        AssetBundleCreateRequest abcr;
        string path = Path.Combine(Application.streamingAssetsPath, AssetBundleName);

        abcr = AssetBundle.LoadFromFileAsync(path);

        yield return abcr;

        bundle.Invoke(abcr.assetBundle);
        Debug.LogFormat(abcr.assetBundle == null ? "Failed to load Asset Bundle : {0}" : "Asset Bundle {0} loaded", name);

        var loadedBundle = abcr.assetBundle;
        var loadAssetReq = loadedBundle.LoadAssetAsync<GameObject>(BallSpriteName);
        yield return loadAssetReq;

        Debug.Log(loadAssetReq.asset);
    }

    private IEnumerator LoadAssetFromURL()
    {
        UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(AssetBundleURL, AbVersion, 0);
        yield return uwr.SendWebRequest();
        //if(uwr.isNetworkError || uwr.isHttpError)
        //the one above was tagged as deprecated. Changed as suggested is below.
        if(uwr.result == UnityWebRequest.Result.ConnectionError || 
            uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            ab = DownloadHandlerAssetBundle.GetContent(uwr);
        }
        Debug.Log("Downloaded bytes: " + uwr.downloadedBytes);
        Debug.Log(ab == null ? "Failed to download Asset Bundle" : "Asset Bundle Downloaded");
    }

    private IEnumerator GetAbVersion()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(AbVersionURL);
        //uwr.SetRequestHeader("Content-Type", "application/json");
        //uwr.SetRequestHeader("User-Agent", "DefaultBrowser");
        yield return uwr.SendWebRequest();
        if (uwr.result == UnityWebRequest.Result.ConnectionError ||
            uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(uwr.error);
        }
        Debug.Log(uwr.downloadHandler.text);
        AbVersion = uint.Parse(uwr.downloadHandler.text);
    }


    public Sprite GetSprite(string assetName)
    {
        return ab.LoadAsset<Sprite>(assetName);
    }
}
