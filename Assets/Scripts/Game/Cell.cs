using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cell : MonoBehaviour
{
    public bool isBomb = false;
    public bool isChecked = false;
    public int countOfBombsAround = 0;
    public static Action OnGameOver;
    public static Action onWin;
    public static Action OnOpenCells;

    [SerializeField] GameObject bombSprite;
    [SerializeField] GameObject unclickedObject;
    [SerializeField] GameObject objectAfterClicking;
    [SerializeField] GameObject[] BombCountSprites;
    [SerializeField] GameObject flag = null;

    private GameManager gameManager;

    private void Start() 
    {
        gameManager = FindObjectOfType<GameManager>();    
    }

    // private bool OpenCell()
    // {
    //     RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    //     if (hit.collider != null)
    //     {
    //         Debug.Log("Hit");
    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             if (gameManager.isFirstClick)
    //             {
    //                 gameManager.GenerateBombsOnGrid(this);
    //                 gameManager.SetUnopenedCellsCount();
    //             }
    //             if (!gameManager.canClick && isChecked || gameManager.isGameEnded)
    //             {   
    //                 CheckCell();
    //             }
    //         }
    //         return true;
    //     }
    //     return false;
    // }

    // private bool SetFlag()
    // {
    //     RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
    //     if (hit.collider != null)
    //     {
            
    //         if (Input.GetMouseButtonUp(1))
    //         {
    //             if (!gameManager.canClick && isChecked || gameManager.isGameEnded)
    //             {
                    
    //             }   
    //         }
    //         return true;
    //     }
    //     return false;
    // }

    private void OnMouseUp() 
    {
        if (gameManager.isFirstClick)
        {
            gameManager.GenerateBombsOnGrid(this);
            gameManager.SetUnopenedCellsCount();
        }
        if (!gameManager.canClick) return;
        if (isChecked || gameManager.isGameEnded) return;
        CheckCell();
    }

    private void SetCountSprite() // Включает спрайт с количеством бомб кругом
    {
        if (countOfBombsAround != 0)
        {
            for (int i = 0; i < BombCountSprites.Length; i++)
            {
                if (i == countOfBombsAround - 1)
                {
                    BombCountSprites[i].SetActive(true);
                }
            }
        } 
    }  

    public void CheckCell()
    {
        isChecked = true;
        SwapSprites();
        if (isBomb)
        {
            bombSprite.SetActive(true);
            OnGameOver();
        }
        else
        {
            SetCountSprite();
            OnOpenCells();
            if (countOfBombsAround == 0)
            {
                gameManager.OpenCellsRecurvely(this);
            }
            onWin();
        }
    }

    public void SwapSprites()
    {
        unclickedObject.SetActive(false);
        objectAfterClicking.SetActive(true);
    }

    public void SetBombSpriteActive()
    {
        bombSprite.SetActive(true);
    }
}
