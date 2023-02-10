using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAssetLoader : MonoBehaviour
{
    public string SpriteName;
    SpriteRenderer m_spriteRenderer;

    private void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [ContextMenu("Load Sprite")]
    void LoadSprite()
    {
        string spriteName = SpriteName.ToLower();
        Sprite m_sprite = AssetBundleManager.Instance.GetSprite(spriteName);
        m_spriteRenderer.sprite = m_sprite;
    }
}
