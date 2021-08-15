using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    private Vector2Int foodGridPosition;
    private Vector2Int foodburnGridPosition;
    private GameObject foodGameObject;
    private GameObject foodburnGameObject;
    private Snake snake;
    private int width;
    private int height;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
    public void Setup(Snake snake)
    {
        this.snake = snake;
        SpawnFood();
        SpawnBurnFood();
    }
    private void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);
        
        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }
    private void SpawnBurnFood()
    {
        do
        {
            foodburnGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        foodburnGameObject = new GameObject("BurnFood", typeof(SpriteRenderer));
        foodburnGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodBurnSprite;
        foodburnGameObject.transform.position = new Vector3(foodburnGridPosition.x, foodburnGridPosition.y);
    }
    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            SpawnFood();
            HighScore.AddScore();
            return true;
        }
        else { return false; }
    }
    public bool TrySnakeEatBurnFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodburnGridPosition)
        {
            Object.Destroy(foodburnGameObject);
            SpawnBurnFood();
            HighScore.SubtractScore();
            return true;
        }
        else { return false; }
    }
    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if(gridPosition.x < 0)
        {
            gridPosition.x = width;
        }
        if(gridPosition.x > width)
        {
            gridPosition.x = 0;
        }
        if (gridPosition.y < 0)
        {
            gridPosition.y = height;
        }
        if (gridPosition.y > height)
        {
            gridPosition.y = 0;
        }
        return gridPosition;
    }
}
