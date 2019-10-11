using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
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
    public void ShowTips(ItemType showType)
    {
        string text = "";
        switch(showType)
        {
            case ItemType.KeyYellow:
                text = "获得 <color=#FECBAF>黄钥匙</color> x1";
                break;
            case ItemType.KeyBlue:
                text = "获得 <color=#CECCFD>蓝钥匙</color> x1";
                break;
            case ItemType.KeyRed:
                text = "获得 <color=#FD8B8E>红钥匙</color> x1";
                break;
            case ItemType.GemRed:
                text = "获得 <color=#FD8B8E>红宝石</color> x1";
                break;
            case ItemType.GemBlue:
                text = "获得 <color=#FD8B8E>蓝宝石</color> x1";
                break;
            case ItemType.MedicineHp1:
                text = "获得 <color=#FD8B8E>小瓶药剂</color> x1";
                break;
            case ItemType.MedicineHp2:
                text = "获得 <color=#FD8B8E>大瓶药剂</color> x1";
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
