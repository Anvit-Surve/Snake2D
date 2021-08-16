using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public enum Food
    {
        food,
        burnFood,
    }
    public Food foodType;
    public Food GetFood = Food.burnFood;
    private Vector2Int foodPosition;
    [SerializeField]
    private Snake snake;
    private int width, height;
    private void Awake()
    {
        width = 20;
        height = 20;
        foodType = Food.food;
    }
    private void Start()
    {
        StartCoroutine(FoodTimer());
    }
    public void SpawnFood()
    {
        do
        {
            foodPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        }
        while (snake.GetFullSnakeGridPositionList().IndexOf(foodPosition)!= -1);

        if(snake.GetSnakeSize() > 1)
        {
            int choice = Random.Range(0, 5);
            switch (choice)
            {
                default:
                    GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
                    foodType = Food.food;
                    break;
                case 0:
                    GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
                    foodType = Food.food;
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodBurnSprite;
                    foodType = Food.burnFood;
                    break;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
            foodType = Food.food;
        }
        transform.position = new Vector3(foodPosition.x, foodPosition.y);
    }
    public bool SnakeEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodPosition)
        {
            switch (foodType)
            {
                case Food.food:
                    snake.burnFood = false;
                    HighScore.AddScore();
                    break;
                case Food.burnFood:
                    HighScore.SubtractScore();
                    snake.burnFood = true;
                    break;
                default:
                    HighScore.AddScore();
                    snake.burnFood = false;
                    break;
            }
            SpawnFood();
            return true;
        }
        else return false;
    }
    public IEnumerator FoodTimer()
    {
        while (true)
        {
            SpawnFood();
            yield return new WaitForSeconds(5f);
        }
    }
}
