using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlaySinglePlayerGame()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.SingleGameScene);
    }
    public void PlayCOopPlayerGame()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
        Loader.Load(Loader.Scene.COopGameScene);
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
