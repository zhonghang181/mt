using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSMB : SceneLinkedSMB<Player>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.UpdateFace();
        m_MonoBehaviour.TryMoving();
    }
}
