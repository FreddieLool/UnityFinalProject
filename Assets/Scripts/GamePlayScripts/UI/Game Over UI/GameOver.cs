using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject UiAfterDelay;
    [SerializeField] GameObject GameOverImageGO;
    [SerializeField] GameObject FinalScoreTextGO;

    

    private float _done = 0.67f;
    private Image _uiImage;
    private TextMeshProUGUI _finalScoreText;
    private Color _c;

    private void Start()
    {
        Time.timeScale = 0;
        _uiImage = GameOverImageGO.GetComponent<Image>();
        _finalScoreText = FinalScoreTextGO.GetComponent<TextMeshProUGUI>();

        GameOverImageGO.SetActive(true);

        _c = _uiImage.color;
        _finalScoreText.text = $"Final Score : {(int)ScorePlayer.PLAYER_SCORE}";
        StartCoroutine(ScreenFade());
    }

    private IEnumerator ScreenFade()
    {
        for(float f = 0; f < _done; f+= 0.01f)
        {
            _uiImage.color = new Color(_c.r, _c.b, _c.g, f);
            yield return new WaitForSecondsRealtime(0.014f);
        }

        UiAfterDelay.SetActive(true);
    }



    public void ApplyRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ApplyMainMenu()
    {  
        SceneManager.LoadScene("Main Menu");
    }


    //FOR NOW!!
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }


}
