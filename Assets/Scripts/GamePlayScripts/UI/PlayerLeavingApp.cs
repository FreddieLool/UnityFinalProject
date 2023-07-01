using UnityEngine;

public class PlayerLeavingApp : MonoBehaviour
{
    bool isPaused = false;

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            GameOver.PauseGame();
        }
        else
        {
            GameOver.ResumeGame();
        }

        //isPaused = !hasFocus;
    }


    //void OnApplicationPause(bool pauseStatus)
    //{
    //    GameOver.PauseGame();
    //    isPaused = pauseStatus;
    //}
}
