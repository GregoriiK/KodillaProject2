using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    public string sceneBundle = "file:///Assets/StreamingAssets/scenes";
    public string spriteBundle = "2d";
    //public uint AbVersion;
    //public string AbVersionURL;
    public string BallSpriteName = "BallSprite";
    public string sceneName = "Level 1";
    private AssetBundle ab;
    private AssetBundle onlineAb;

    private IEnumerator Start()
    {
        if(!ab)
        {
            yield return StartCoroutine(LoadAssets(spriteBundle, result => ab = result));
        }
            ab.LoadAllAssets();
        if(!onlineAb)
        {
            yield return StartCoroutine(LoadAssetFromURL());
        }
        //yield return StartCoroutine(GetAbVersion());
    }
    private IEnumerator LoadAssets(string name, Action<AssetBundle> bundle)
    {
        AssetBundleCreateRequest abcr;
        string path = Path.Combine(Application.streamingAssetsPath, spriteBundle);
        abcr = AssetBundle.LoadFromFileAsync(path);
        yield return abcr;
        bundle.Invoke(abcr.assetBundle);
        Debug.LogFormat(abcr.assetBundle == null ? "Failed to load Asset Bundle : {0}" : "Asset Bundle {0} loaded", name);
    }
    private IEnumerator LoadAssetFromURL()
    {
        UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(sceneBundle);
        yield return uwr.SendWebRequest();
        if (uwr.result == UnityWebRequest.Result.ConnectionError ||
            uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            onlineAb = DownloadHandlerAssetBundle.GetContent(uwr);
            yield return onlineAb;
        }
        Debug.Log("Downloaded bytes: " + uwr.downloadedBytes);
        Debug.Log(onlineAb == null ? "Failed to download Asset Bundle" : "Asset Bundle Downloaded");
    }
    //private IEnumerator GetAbVersion()
    //{
    //    UnityWebRequest uwr = UnityWebRequest.Get(AbVersionURL);
    //    yield return uwr.SendWebRequest();
    //    if (uwr.result == UnityWebRequest.Result.ConnectionError ||
    //        uwr.result == UnityWebRequest.Result.ProtocolError)
    //    {
    //        Debug.Log(uwr.error);
    //    }
    //    Debug.Log(uwr.downloadHandler.text);
    //    AbVersion = uint.Parse(uwr.downloadHandler.text);
    //}
    public IEnumerator LoadSingleAsset(string assetName)
    {
        if (ab == null) Debug.Log("Asset Bundle not loaded");
        else
        {
            var loadAssetReq = ab.LoadAssetAsync(assetName);
            yield return loadAssetReq;

            var ballSprite = FindObjectOfType<BallComponent>().GetComponent<SpriteRenderer>();
            var texture = (Texture2D)loadAssetReq.asset;
            var newBallSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            ballSprite.sprite = newBallSprite;
        }
    }
    public IEnumerator LoadLevel(string sceneName)
    {
        if (sceneBundle == null)
        {
            Debug.Log("Scene bundle not loaded");
            yield break;
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        yield return null;

        if (spriteBundle == null)
        {
            Debug.Log("Sprite bundle not loaded");
            yield break;
        }

    }


    public Sprite GetSprite(string assetName)
    {
        return ab.LoadAsset<Sprite>(assetName);
    }
}