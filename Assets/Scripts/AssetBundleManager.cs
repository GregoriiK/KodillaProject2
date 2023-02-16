using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    public string AssetBundleURL;
    public string AssetBundleName;
    public uint AbVersion;
    public string AbVersionURL;
    public string BallSpriteName;
    public string[] scenePaths;
    private AssetBundle ab;
    private AssetBundle onlineAb;

    //private IEnumerator Start()
    //{
    //    //yield return StartCoroutine(GetAbVersion());
    //}

    private IEnumerator LoadAssets(string name, Action<AssetBundle> bundle)
    {
        AssetBundleCreateRequest abcr;
        string path = Path.Combine(Application.streamingAssetsPath, AssetBundleName);

        abcr = AssetBundle.LoadFromFileAsync(path);

        yield return abcr;

        bundle.Invoke(abcr.assetBundle);
        Debug.LogFormat(abcr.assetBundle == null ? "Failed to load Asset Bundle : {0}" : "Asset Bundle {0} loaded", name);
    }

    private IEnumerator LoadAssetFromURL()
    {
        UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(AssetBundleURL, AbVersion, 0);
        yield return uwr.SendWebRequest();

        if(uwr.result == UnityWebRequest.Result.ConnectionError || 
            uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            onlineAb = DownloadHandlerAssetBundle.GetContent(uwr);
            yield return onlineAb;
            scenePaths = onlineAb.GetAllScenePaths();
        }
        Debug.Log("Downloaded bytes: " + uwr.downloadedBytes);
        Debug.Log(onlineAb == null ? "Failed to download Asset Bundle" : "Asset Bundle Downloaded");
    }

    private IEnumerator GetAbVersion()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(AbVersionURL);
        yield return uwr.SendWebRequest();
        if (uwr.result == UnityWebRequest.Result.ConnectionError ||
            uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(uwr.error);
        }
        Debug.Log(uwr.downloadHandler.text);
        AbVersion = uint.Parse(uwr.downloadHandler.text);
    }

    public IEnumerator LoadSingleAsset(string assetName)
    {
        yield return StartCoroutine(LoadAssets(AssetBundleName, result => ab = result));
        var loadAssetReq = ab.LoadAssetAsync(assetName);
        yield return loadAssetReq;
        var ballSprite = FindObjectOfType<BallComponent>().GetComponent<SpriteRenderer>();
        var texture = (Texture2D)loadAssetReq.asset;
        Debug.Log(texture);
        var newBallSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        ballSprite.sprite = newBallSprite;
    }

    public IEnumerator LoadLevel()
    {
        yield return StartCoroutine(LoadAssetFromURL());
        yield return StartCoroutine(LoadAssets(AssetBundleName, result => ab = result));
        ab.LoadAllAssets();
        int i = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(i);
        Debug.Log(scenePaths.Length);
        if(i == scenePaths.Length-1)
        {
            i = 0;
        }

        SceneManager.LoadScene(scenePaths[i]);
        i++;
    }


    public Sprite GetSprite(string assetName)
    {
        return ab.LoadAsset<Sprite>(assetName);
    }
}
