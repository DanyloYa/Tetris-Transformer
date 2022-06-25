using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement
{
    public Vector2 position = new Vector2(0, 0);

    public GameObject obj = null;

    public GridElement(Vector2 position, GameObject obj) 
    {
        this.position = position;
        this.obj = obj;
    }

    public GridElement() {}
}
