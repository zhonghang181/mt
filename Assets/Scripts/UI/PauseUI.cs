using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public void RestartLevel()
    {
        Close();
        SceneController.RestartLevel();
    }

    public void Close()
    {
        Player.Instance.Resume();
    }
}
