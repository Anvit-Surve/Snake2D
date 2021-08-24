using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverWindow : MonoBehaviour
{
    private static GameOverWindow instance;
    public Button buttonRetry;
    private void Awake()
    {
        instance =  this;
        buttonRetry.onClick.AddListener(RetryGame);
        Hide();
    }
    private void RetryGame()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.SingleGameScene);
    }
    private void Show(bool isNewHighScore)
    {
        gameObject.SetActive(true);
        transform.Find("newhighscoretext").gameObject.SetActive(isNewHighScore);
        transform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = HighScore.GetScore().ToString();
        transform.Find("highscoreText").GetComponent<TextMeshProUGUI>().text = "HIGHSCORE " + HighScore.GetHighscore();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    public static void ShowStatic(bool isNewHighScore)
    {
        instance.Show(isNewHighScore);
    }
}
