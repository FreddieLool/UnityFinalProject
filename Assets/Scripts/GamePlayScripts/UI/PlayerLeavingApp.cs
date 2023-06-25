using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeavingApp : MonoBehaviour
{
    bool isPaused = false;
    GameOver gameOver;

    void OnApplicationFocus(bool hasFocus)
    {
        gameOver.ResumeGame();
        isPaused = !hasFocus;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        gameOver.PauseGame();
        isPaused = pauseStatus;
    }
}
