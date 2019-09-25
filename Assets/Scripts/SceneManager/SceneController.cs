using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
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

    public IEnumerator Transition(string newSceneName)
    {
        // TODO SaveData
        m_IsTransitioning = true;
        yield return StartCoroutine(SceneFader.FadeSceneIn(SceneFader.FadeType.Loading));
        Debug.Log("After FadeSceneIn");

        yield return SceneManager.LoadSceneAsync(newSceneName);
        Debug.Log("After LoadSceneAsync");

        yield return StartCoroutine(SceneFader.FadeSceneOut());
        Debug.Log("After FadeSceneOut");
        m_IsTransitioning = false;
    }
}
