using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public enum PowerUp
    {
        Sheild,
        ScoreBoost,
        SpeedBoost,
    }
    public PowerUp powerUpType;
    private Vector2Int powerUpPosition;
    [SerializeField]
    private Snake snake;
    private int width, height;
    private void Awake()
    {
        width = 20;
        height = 20;
        powerUpType = PowerUp.Sheild;
    }
    private void Start()
    {
        StartCoroutine(PowerUpTimer());
    }
    public void SpawnPowerUp()
    {
        do
        {
            powerUpPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        }
        while (snake.GetFullSnakeGridPositionList().IndexOf(powerUpPosition) != -1);
        if (snake.GetSnakeSize() > 3)
        {
            int choice = Random.Range(0, 10);
            switch (choice)
            {
                case 0:
                    GetComponent<SpriteRenderer>().sprite = GameAssets.i.Shield;
                    powerUpType = PowerUp.Sheild;
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().sprite = GameAssets.i.SpeedBoost;
                    powerUpType = PowerUp.SpeedBoost;
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().sprite = GameAssets.i.ScoreBoost;
                    powerUpType = PowerUp.ScoreBoost;
                    break;
            }
            transform.position = new Vector3(powerUpPosition.x, powerUpPosition.y);
        }
    }
    public bool SnakeEatPowerUp(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == powerUpPosition)
        {
            switch (powerUpType)
            {
                case PowerUp.Sheild:
                    snake.shield = true;
                    StartCoroutine(Shield());
                    break;
                case PowerUp.ScoreBoost:
                    HighScore.ScoreBoost = true;
                    StartCoroutine(ScoreBoost());
                    break;
                case PowerUp.SpeedBoost:
                    snake.speed = snake.speed * 2;
                    StartCoroutine(SpeedBoost());
                    break;
                default:
                    break;
            }
            SpawnPowerUp();
            return true;
        }
        else return false;
    }
    public IEnumerator PowerUpTimer()
    {
        while (true)
        {
            SpawnPowerUp();
            yield return new WaitForSeconds(Random.Range(10,15));
        }
    }
    public IEnumerator SpeedBoost()
    {
        yield return new WaitForSeconds(3);
        snake.speed = snake.speed/2;
        yield break;
    }
    public IEnumerator ScoreBoost()
    {
        yield return new WaitForSeconds(3);
        HighScore.ScoreBoost = false;
        yield break;
    }
    public IEnumerator Shield()
    {
        yield return new WaitForSeconds(3);
        snake.shield = false;
        yield break;
    }
}
