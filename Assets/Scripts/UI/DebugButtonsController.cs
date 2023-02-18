using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugButtonsController : MonoBehaviour
{
    public Button LoadBallSprite;
    public Button LoadNextLevel;
    void Start()
    {
        LoadBallSprite.onClick.AddListener(delegate {
            StartCoroutine(AssetBundleManager.Instance.LoadSingleAsset(AssetBundleManager.Instance.BallSpriteName)); });
        LoadNextLevel.onClick.AddListener(delegate { 
            StartCoroutine(AssetBundleManager.Instance.LoadLevel(AssetBundleManager.Instance.sceneName)); }) ;
    }

}
