using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private Snake snake;
    private LevelGrid levelGrid;
    void Start()
    {
        levelGrid = new LevelGrid(50, 50);
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }

  
}
