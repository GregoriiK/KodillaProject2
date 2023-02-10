using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    public string AssetBundleName;
    private AssetBundle ab;

    private void Start()
    {
        StartCoroutine(LoadAssets());
    }

    private IEnumerator LoadAssets()
    {
        AssetBundleCreateRequest abcr;
        string path = Path.Combine(Application.streamingAssetsPath, AssetBundleName);

        abcr = AssetBundle.LoadFromFileAsync(path);

        yield return abcr;

        ab = abcr.assetBundle;
        Debug.Log(ab == null ? "Failed to load Asset Bundle" : "Asset Bundle loaded");
    }

    public Sprite GetSprite(string assetName)
    {
        return ab.LoadAsset<Sprite>(assetName);
    }
}
