using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject options;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Options()
    {
        options.SetActive(true);
    }
    public void Back()
    {
        options.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }

}
