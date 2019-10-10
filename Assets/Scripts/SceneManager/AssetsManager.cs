using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsManager : MonoBehaviour
{
    protected static AssetsManager s_Instance;
    public static AssetsManager Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<AssetsManager>();

            if (s_Instance != null)
                return s_Instance;

            Create();

            return s_Instance;
        }
    }

    public static void Create()
    {
        GameObject obj = new GameObject("AssetsManager");
        s_Instance = obj.AddComponent<AssetsManager>();
    }

    // =========== Properties ===========
    Dictionary<string, Sprite> m_DicSprites = new Dictionary<string, Sprite>();

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // =========== Public Funcstions ===========
    public Sprite GetSprite(string nameInSheet, string relativePath = "")
    {
        Sprite target = null;
        if (!m_DicSprites.ContainsKey(nameInSheet) && relativePath.Length > 0)
        {
            LoadSpriteSheet(relativePath);
        }

        if (m_DicSprites.ContainsKey(nameInSheet))
        {
            target = m_DicSprites[nameInSheet];
        }

        return target;
    }

    // =========== Private Functions ===========
    void LoadSpriteSheet(string sheetName)
    {
        Sprite[] spArr = Resources.LoadAll<Sprite>(sheetName);
        foreach (var sprite in spArr)
        {
            if (!m_DicSprites.ContainsValue(sprite))
            {
                m_DicSprites.Add(sprite.name, sprite);
            }
        }
    }
}
