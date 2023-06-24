using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _options;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Options()
    {
        _options.SetActive(true);
    }
    public void Back()
    {
        _options.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }

}
