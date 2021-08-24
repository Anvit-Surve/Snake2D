using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreWindow : MonoBehaviour
{
    private static ScoreWindow instance;
    private TextMeshProUGUI scoreText;
    private void Awake()
    {
        instance = this;
        scoreText = transform.Find("scoreText").GetComponent<TextMeshProUGUI>();
        HighScore.OnHighscoreChanged += Score_OnHighscoreChanged;
        UpdateHighscore();
    }

    private void Score_OnHighscoreChanged(object sender, System.EventArgs e)
    {
        UpdateHighscore();
    }
    private void UpdateHighscore()
    {
        int highscore = HighScore.GetHighscore();
        transform.Find("highScoreText").GetComponent<TextMeshProUGUI>().text = "HIGHSCORE\n" + highscore.ToString();
    }
    private void Update()
    {
        scoreText.text = HighScore.GetScore().ToString();
    }
    public static void HideStatic()
    {
        instance.gameObject.SetActive(false);
    }
}
