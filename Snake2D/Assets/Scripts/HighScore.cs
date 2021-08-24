using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HighScore
{
    public static event EventHandler OnHighscoreChanged;
    private static int score;
    public static bool ScoreBoost = false;
    public static void InitialiseStatic()
    {
        OnHighscoreChanged = null;
        score = 0;
    }
    public static int GetScore()
    {
        return score;
    }
    public static void AddScore()
    {
        if (ScoreBoost)
        {
            score += 200;
        }
        else { score += 100; }
    }
    public static void SubtractScore()
    {
        score -= 100;
    }
    public static int GetHighscore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }
    public static bool TrySetNewHighscore()
    {
        return TrySetNewHighscore(score);
    }
    public static bool TrySetNewHighscore(int score)
    {
        int highscore = GetHighscore();
        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            if (OnHighscoreChanged != null) OnHighscoreChanged(null, EventArgs.Empty);
            return true;
        }
        else
        {
            return false;
        }
    }
}
