using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
    // =========== Const Define ===========
    public enum ShowType
    {
        KeyYellow, KeyBlue, KeyRed,
    }
    // =========== Static Functions ===========
    private static Tips s_Instance;
    public static Tips Instance
    {
        get
        {
            if (s_Instance)
                return s_Instance;

            s_Instance = FindObjectOfType<Tips>();

            if (s_Instance)
                return s_Instance;

            Create();

            return s_Instance;
        }
    }
    private static void Create()
    {
        var prefab = Resources.Load<Tips>("Tips");
        s_Instance = Instantiate(prefab);
    }

    // =========== Properties ===========
    public Text tips;
    public GameObject content;
    float m_TimeAccumualte;

    // =========== MonoBehavior ===========
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        content.SetActive(false);
    }

    // =========== Private Functions ===========
    IEnumerator HideTips()
    {
        while(m_TimeAccumualte < 1)
        {
            m_TimeAccumualte += Time.deltaTime;
            yield return null;
        }
        content.SetActive(false);
    }

    // =========== Public Functions ===========
    public void ShowTips(ShowType showType)
    {
        string text = "";
        switch(showType)
        {
            case ShowType.KeyYellow:
                text = "获得 <color=#FECBAF>黄钥匙</color> x1";
                break;
            case ShowType.KeyBlue:
                text = "获得 <color=#CECCFD>蓝钥匙</color> x1";
                break;
            case ShowType.KeyRed:
                text = "获得 <color=#FD8B8E>红钥匙</color> x1";
                break;
        }

        tips.text = text;

        m_TimeAccumualte = 0f;
        if (!content.activeInHierarchy)
        {
            content.SetActive(true);
            StartCoroutine(HideTips());
        }
    }
}
