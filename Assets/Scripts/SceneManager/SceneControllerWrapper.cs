using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerWrapper : MonoBehaviour
{
    public void TransitionToScene(TransitionPoint transitionPoint)
    {
        SceneController.TransitionToScene(transitionPoint);
    }
}
