using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    private enum State
    {
        Alive,
        Dead
    }
    private State state;
    private Vector2Int gridPosition;
    private Direction gridMoveDirection;
    private float gridMoveTimer;
    public int speed;
    private float gridMoveTimerMax;
    public int snakeBodySize;
    private int width;
    private int height;

    [HideInInspector] public bool burnFood = false;
    [HideInInspector] public bool shield = false;

    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private PowerUpSpawner powerUpSpawner;

    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    private void Awake()
    {
        width = 20;
        height = 20;
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();
        state = State.Alive;
    }
    private void Update()
    {
        switch (state) 
        {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                break;
        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime*speed;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;
            SoundManager.PlaySound(SoundManager.Sound.SnakeMove);
            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);
            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Left:
                    gridMoveDirectionVector = new Vector2Int(-1, 0);
                    break;
                case Direction.Right:
                    gridMoveDirectionVector = new Vector2Int(1, 0);
                    break;
                case Direction.Up:
                    gridMoveDirectionVector = new Vector2Int(0, 1);
                    break;
                case Direction.Down:
                    gridMoveDirectionVector = new Vector2Int(0, -1);
                    break;
            }

            gridPosition += gridMoveDirectionVector;
            gridPosition = ValidateGridPosition(gridPosition);

            bool snakeAteFood = foodSpawner.SnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
                if (burnFood)
                {
                    RemoveSnakeBody();
                    snakeBodySize--;
                }
                else
                {
                    snakeBodySize++;
                    CreateSnakeBody();
                }
            }
            bool snakePowerUp = powerUpSpawner.SnakeEatPowerUp(gridPosition);

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();
            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    if (!shield)
                    {
                        state = State.Dead;
                        GameHandler.SnakeDied();
                        SoundManager.PlaySound(SoundManager.Sound.SnakeDie);
                    }
                }
            }
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);
        }
    }
    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0)
        {
            gridPosition.x = width;
        }
        if (gridPosition.x > width)
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
    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }
    private void RemoveSnakeBody()
    {
        Destroy(snakeBodyPartList[snakeBodyPartList.Count - 1].snakeBodyGameObject);
        snakeBodyPartList.RemoveAt(snakeBodyPartList.Count - 1);
    }
    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }
    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }
    }
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }
    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int> { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }
    private class SnakeBodyPart
    {
        private Transform transform;
        private SnakeMovePosition snakeMovePosition;
        public GameObject snakeBodyGameObject;
        public SnakeBodyPart(int bodyIndex)
        {
            snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }
        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);
            float angle = 0;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 0; break;
                        case Direction.Left:
                            angle = 0 + 45;
                            transform.position = new Vector3(transform.position.x + .2f, transform.position.y + .2f);
                            break;
                        case Direction.Right:
                            angle = 0 - 45;
                            transform.position = new Vector3(transform.position.x - .2f, transform.position.y + .2f);
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 90; break;
                        case Direction.Down:
                            angle = 45;
                            transform.position = new Vector3(transform.position.x + .2f, transform.position.y + .2f);
                            break;
                        case Direction.Up:
                            angle = -45;
                            transform.position = new Vector3(transform.position.x + .2f, transform.position.y - .2f);
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = -90; break;
                        case Direction.Down:
                            angle = -45;
                            transform.position = new Vector3(transform.position.x - .2f, transform.position.y + .2f);
                            break;
                        case Direction.Up:
                            angle = 45;
                            transform.position = new Vector3(transform.position.x - .2f, transform.position.y - .2f);
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 180; break;
                        case Direction.Left:
                            angle = 180 - 45;
                            transform.position = new Vector3(transform.position.x + .2f, transform.position.y - .2f);
                            break;
                        case Direction.Right:
                            angle = 180 + 45;
                            transform.position = new Vector3(transform.position.x - .2f, transform.position.y - .2f);
                            break;
                    }
                    break;
            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }

    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }
        public Direction GetDirection()
        {
            return direction;
        }
        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
        }
    }
    public int GetSnakeSize()
    {
        return snakeBodyPartList.Count;
    }
}

