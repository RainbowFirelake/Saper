using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private GameObject[,] cells;

    public GameObject[,] GenerateGrid(GameObject cellPrefab, int width, int height)
    {
        float deltaX = 0;
        float deltaY = 0;
        cells = new GameObject[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                cells[i, j] = Instantiate
                (
                    cellPrefab,
                    new Vector2(GetComponent<Transform>().position.x + deltaX,
                    GetComponent<Transform>().position.y + deltaY), 
                    Quaternion.identity, this.transform
                );
                
                deltaX += cellPrefab.GetComponent<Transform>().localScale.x;
            }
            deltaY -= cellPrefab.GetComponent<Transform>().localScale.y;
            deltaX = 0;
        }
        return cells;
    }
}
