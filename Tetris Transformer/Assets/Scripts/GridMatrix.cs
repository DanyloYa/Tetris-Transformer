using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMatrix : MonoBehaviour
{
    public int width = 0;
    public int height = 0;

    private List<List<GridElement>> _elements;

    public GridMatrix(int width, int height)
    {
        _elements = new List<List<GridElement>>();

        this.width = width;
        this.height = height;
        
        Resize();
    }

    private void Resize()
    {
        _elements.Clear();

        for (int i = 0; i < width; i++)
        {
            _elements.Add(new List<GridElement>());

            for (int j = 0; j < height; j++)
                _elements[i].Add(new GridElement());
        }
    }

    public void MoveElement(Vector2Int gridPosition, Vector2Int direction)
    {
        var elementObject = GetObject(gridPosition);
        var newGridPosition = gridPosition + direction;
      
        SetObject(newGridPosition, elementObject);
        ClearObject(gridPosition);

        if (elementObject)
            elementObject.transform.position = GetPosition(newGridPosition);
    }

    public void DestroyFilledLines()
    {
        for (int i = 0; i < height; i++)
        {
            bool filledLines = true;

            for (int j = 0; j < width; j++)
                if (GetElement(new Vector2Int(j, i)).obj == null)
                    filledLines = false;

            if (filledLines)
            {
                DestroyLine(i);
                break;
            }
        }
    }

    private void DestroyLine(int line)
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 10);

        for (int i = 0; i < width; i++)
        {
            Destroy(GetObject(new Vector2Int(i, line)));
            GetElement(new Vector2Int(i, line)).obj = null;
        }

        for (int i = line + 1; i < height; i++)
            for (int j = 0; j < width; j++)
                MoveElement(new Vector2Int(j, i), new Vector2Int(0, -1));

        DestroyFilledLines();
    }

    public GridElement GetElement(Vector2Int point) => _elements[point.x][point.y];
    public void SetElement(int x, int y, Vector2 position, GameObject obj = null) => _elements[x][y] = new GridElement(position, obj);

    public Vector2 GetPosition(Vector2Int point) => _elements[point.x][point.y].position;

    public void SetObject(Vector2Int point, GameObject obj) => _elements[point.x][point.y].obj = obj;
    public GameObject GetObject(Vector2Int point) => _elements[point.x][point.y].obj;
    public void ClearObject(Vector2Int point) => _elements[point.x][point.y].obj = null;
}
