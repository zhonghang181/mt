using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TransitionPoint : MonoBehaviour
{
    public enum SceneType
    {
        Level1,
        Level2,
        Level3,
    }
    protected string[] m_SceneArr = { "Level1", "Level2", "Level3" };


    // =========== Properties ===========
    public GameObject transitionGameObject;
    public SceneType newScene = SceneType.Level1;
    public BirthPointType birthPoint = BirthPointType.Upstairs;

    public string GetSceneName()
    {
        return m_SceneArr[(int)newScene];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == transitionGameObject)
        {
            if (SceneFader.IsFading || SceneController.IsTransitioning)
                return;

            TransitionInternal();
        }   
    }

    protected void TransitionInternal()
    {
        SceneController.TransitionToScene(this);
    }
}
