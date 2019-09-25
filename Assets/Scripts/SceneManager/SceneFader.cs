using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public enum FadeType
    {
        Loading, GameOver,
    }

    #region Static-Instance
    protected static SceneFader s_Instance;
    public static SceneFader Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<SceneFader>();

            if (s_Instance != null)
                return s_Instance;

            Create();

            return s_Instance;
        }
    }

    public static void Create()
    {
        SceneFader prefab = Resources.Load<SceneFader>("SceneFader");
        s_Instance = Instantiate(prefab);
    }
    #endregion

    // =========== Properties ===========
    public CanvasGroup loadingCanvasGroup;
    public CanvasGroup gameOverCanvasGroup;
    public float fadeDuration = 1f;
    protected bool m_IsFading;

    #region Static-Api
    public static bool IsFading
    {
        get
        {
            return Instance.m_IsFading;
        }
    }

    public static IEnumerator FadeSceneIn(FadeType fadeType)
    {
        Debug.Log("FadeSceneIn Start");
        CanvasGroup canvasGroup;
        switch(fadeType)
        {
            case FadeType.GameOver:
                canvasGroup = Instance.gameOverCanvasGroup;
                break;
            default:
                canvasGroup = Instance.loadingCanvasGroup;
                break;
        }

        canvasGroup.gameObject.SetActive(true);
        yield return Instance.StartCoroutine(Instance.Fade(1f, canvasGroup));
        Debug.Log("FadeSceneIn End");
    }

    public static IEnumerator FadeSceneOut()
    {
        Debug.Log("FadeSceneOut Start");
        CanvasGroup canvasGroup;
        canvasGroup = Instance.loadingCanvasGroup.alpha > 0.1f ? Instance.loadingCanvasGroup : null;
        canvasGroup = Instance.gameOverCanvasGroup.alpha > 0.1f ? Instance.gameOverCanvasGroup : canvasGroup;

        yield return Instance.StartCoroutine(Instance.Fade(0f, canvasGroup));
        canvasGroup.gameObject.SetActive(false);
        Debug.Log("FadeSceneOut End");
    }
    #endregion

    #region Instance
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        loadingCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.alpha = 0f;
    }

    protected IEnumerator Fade(float dstAlpha, CanvasGroup canvasGroup)
    {
        Debug.Log("Fade Start");
        m_IsFading = true;
        canvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(canvasGroup.alpha - dstAlpha) / fadeDuration;
        while(!Mathf.Approximately(canvasGroup.alpha, dstAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, dstAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Fade End");
        canvasGroup.alpha = dstAlpha;
        m_IsFading = false;
        canvasGroup.blocksRaycasts = false;
    }
    #endregion
}
