using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.GameScene);
    }
    public void QuitGame()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Application.Quit();
    }
    public void PlayButtonSound()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
    }
}
