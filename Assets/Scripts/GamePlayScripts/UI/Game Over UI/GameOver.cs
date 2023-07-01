using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject UiAfterDelay;
    [SerializeField] GameObject GameOverImageGO;
    [SerializeField] GameObject FinalScoreTextGO;
    [SerializeField] GameObject ReviveButtonGO;

    public int AmountsRevived = 0;
    private float _done = 0.67f;
    private Image _uiImage;
    private TextMeshProUGUI _finalScoreText;
    private Color _c;

    public static bool IsGamePaused = false;

    public static GameObject MapProcGenGO; 
    


    public void ApplyGameOver()
    {
        PauseGame();

        _uiImage = GameOverImageGO.GetComponent<Image>();
        _finalScoreText = FinalScoreTextGO.GetComponent<TextMeshProUGUI>();

        GameOverImageGO.SetActive(true);

        _c = _uiImage.color;
        _finalScoreText.text = $"Final Score : {(int)ScorePlayer.PLAYER_SCORE}";
        StartCoroutine(ScreenFade());
    }

    private IEnumerator ScreenFade()
    {
        for (float f = 0; f < _done; f += 0.01f)
        {
            _uiImage.color = new Color(_c.r, _c.b, _c.g, f);
            yield return new WaitForSecondsRealtime(0.014f);
        }

        UiAfterDelay.SetActive(true);
        ReviveButtonGO.SetActive(false);
    }



    public void ApplyRestart()
    {
        AmountsRevived++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ApplyMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }


    //FOR NOW!!
    public static void PauseGame()
    {
        MapProcGenGO.SetActive(false);
        IsGamePaused = true;
    }

    public static void ResumeGame()
    {
        MapProcGenGO.SetActive(true);
        IsGamePaused = false;
    }


}
