using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public bool isGameEnded = false;
    public bool isFirstClick = true;
    public bool canClick = true;
    public static Action OnGameOver;
    public static Action OnWin;

    [Range(0, 100)] [SerializeField] int chanceToSpawnBomb = 10;
    [SerializeField] GameObject cellPrefab;
    [SerializeField] GridGenerator gridGenerator;

    private GameObject[,] grid;
    private int unopenedCellsCount = 0;

    private void Awake() 
    {
        grid = gridGenerator.GenerateGrid(cellPrefab, 10, 10);
        // isGameEnded = false;
        // isFirstClick = true;
        // canClick = true;
        // unopenedCellsCount = 0;
    }

    private void Start()
    {
        
    }

    private void OnEnable() 
    {
        Cell.OnGameOver += GameOver;
        Cell.OnOpenCells += DecreaseUnopenedCellsCount;
        Cell.onWin += CheckWinCondition;
    }

    public void OpenCellsRecurvely(Cell cell)
    {
        int iTemp = 0;
        int jTemp = 0;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (cell == grid[i, j].GetComponent<Cell>())
                {
                    iTemp = i;
                    jTemp = j;
                    break;
                }
            }
        }

        // Рекурсивное открытие клеток, цифра слева - сдвиг по x, справа - по y
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Huita(iTemp, jTemp, i, j);
            }
        }
    }

    public void GameOver()
    {
        canClick = false;
        OnGameOver();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j].GetComponent<Cell>().isBomb)
                {
                    grid[i, j].GetComponent<Cell>().SetBombSpriteActive();
                }
            }
        }
    }  

    public void GenerateBombsOnGrid(Cell whichStarted) // На какую клетку нажали впервые, чтобы бомба не сгенерировалась прямо на ней
    {
        isFirstClick = false;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j].GetComponent<Cell>() == whichStarted) continue;
                SetCellAsBomb(grid[i, j].GetComponent<Cell>());
            }
        }

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++) // Установка количества бомб кругом
            {
                grid[i, j].GetComponent<Cell>().countOfBombsAround 
                    = GetCountOfBombsAround(grid[i, j].GetComponent<Cell>());
            }
        }
    }

    public void SetUnopenedCellsCount()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (!grid[i, j].GetComponent<Cell>().isBomb)
                {
                    unopenedCellsCount++;
                }
            }
        }
    }

    private void CheckWinCondition()
    {
        if (unopenedCellsCount == 0)
        {
            OnWin();
        }
    }

    private void DecreaseUnopenedCellsCount()
    {
        unopenedCellsCount--;
    }

    private void SetCellAsBomb(Cell cell)
    {
        int rand = UnityEngine.Random.Range(0, 101);
        if (rand <= chanceToSpawnBomb)
        {
            cell.isBomb = true;
        }
    }

    private int GetCountOfBombsAround(Cell cell)
    {
        int iTemp = 0; 
        int jTemp = 0;
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (cell == grid[i, j].GetComponent<Cell>())
                {
                    iTemp = i;
                    jTemp = j;
                } 
            }
        }
        int bombsAroundCount = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i + iTemp >= 0 && i + iTemp < grid.GetLength(0) && j + jTemp >= 0 && j + jTemp < grid.GetLength(1))
                {
                    var temp = grid[i + iTemp, j + jTemp].GetComponent<Cell>();
                    if (temp.isBomb)
                    {
                        bombsAroundCount++;
                    }
                }
            }
        }
        return bombsAroundCount;
    }

    private void Huita(int i, int j, int deltai, int deltaj)
    {
        if (i + deltai >= 0 && i + deltai < grid.GetLength(0) &&
            j + deltaj >= 0 && j + deltaj < grid.GetLength(0))
        {
            Cell tempCell = grid[i + deltai, j + deltaj]?.GetComponent<Cell>();
            if (!tempCell.isChecked)
            {
                if (tempCell.countOfBombsAround == 0)
                {
                    tempCell.CheckCell();
                    OpenCellsRecurvely(tempCell);
                }
                if (tempCell.countOfBombsAround > 0)
                {
                    tempCell.CheckCell();
                }
            }
        }
    }
}
