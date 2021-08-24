using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAssets : MonoBehaviour
{
    public static GameAssets i;
    public Sprite snakeHeadSprite;
    public Sprite snakeBodySprite;
    public Sprite foodSprite;
    public Sprite foodBurnSprite;
    public Sprite Shield;
    public Sprite SpeedBoost;
    public Sprite ScoreBoost;
    public SoundAudioClip[] soundAudioClip;

    private void Awake()
    {
        i = this;
    }
    [Serializable]
    public class SoundAudioClip {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
