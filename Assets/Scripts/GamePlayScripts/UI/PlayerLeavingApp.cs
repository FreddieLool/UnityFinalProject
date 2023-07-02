using UnityEngine;

public class PlayerLeavingApp : MonoBehaviour
{

    [SerializeField] GameObject LevelUpPanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject PausePanel;

    void OnApplicationFocus(bool hasFocus)
    {
        if (!LevelUpPanel.activeSelf && !GameOverPanel.activeSelf && !PausePanel.activeSelf)
        {
            if (!hasFocus)
            {
                GameOver.PauseGame();
            }
            else
            {
                GameOver.ResumeGame();
            }
        }
    }
}
