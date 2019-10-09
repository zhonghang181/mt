using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSMB : SceneLinkedSMB<Player>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.TryMoving();
    }
}
