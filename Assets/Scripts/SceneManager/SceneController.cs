using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Static
    protected static SceneController s_Instance;
    public static SceneController Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<SceneController>();

            if (s_Instance != null)
                return s_Instance;

            Create();

            return s_Instance;
        }
    }

    public static void Create()
    {
        GameObject obj = new GameObject("SceneController");
        s_Instance = obj.AddComponent<SceneController>();
    }

    protected bool m_IsTransitioning;

    public static bool IsTransitioning
    {
        get { return Instance.m_IsTransitioning; }
    }

    public static void TransitionToScene(TransitionPoint transitionPoint)
    {
        Instance.StartCoroutine(Instance.Transition(transitionPoint.GetSceneName()));
    }

    public static void RestartLevel()
    {
        Instance.StartCoroutine(Instance.Transition(SceneManager.GetActiveScene().name));
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
    }

    public IEnumerator Transition(string newSceneName)
    {
        var options = SceneManager.LoadSceneAsync(newSceneName);
        options.allowSceneActivation = false;
        while (!options.isDone)
        {
            if (options.progress >= 0.9f)
            {
                options.allowSceneActivation = true;
            } 

            if (!m_IsTransitioning)
            {
                m_IsTransitioning = true;
                yield return Instance.StartCoroutine(SceneFader.FadeSceneIn(SceneFader.FadeType.Loading));
            } else
            {
                yield return null;
            }
        }

        yield return StartCoroutine(SceneFader.FadeSceneOut());
        m_IsTransitioning = false;
    }
    #endregion
}
