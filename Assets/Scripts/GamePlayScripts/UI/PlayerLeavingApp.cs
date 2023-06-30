using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeavingApp : MonoBehaviour
{
    bool isPaused = false;

    void OnApplicationFocus(bool hasFocus)
    {
        GameOver.ResumeGame();
        isPaused = !hasFocus;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        GameOver.PauseGame();
        isPaused = pauseStatus;
    }
}
