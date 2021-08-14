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
        instance = this;
        buttonRetry.onClick.AddListener(RetryGame);
        Hide();
    }
    private void RetryGame()
    {
        Loader.Load(Loader.Scene.GameScene);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    public static void ShowStatic()
    {
        instance.Show();
    }
}
