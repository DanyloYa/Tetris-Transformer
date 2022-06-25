using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeInput : MonoBehaviour
{
    [SerializeField] private SnakeController _snakeController;
    [SerializeField] private float _minDragDistance;
    [SerializeField] private float _maxDragDistance;

    private Vector2 _startTouch = new Vector2(0, 0);

    private bool _isDrag = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           _startTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           _isDrag = true;
        }

        else if (Input.GetMouseButtonUp(0))
            ChangeDirection();

        else if (_isDrag)
        {
            var currentDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Mathf.Abs(currentDragPos.x - _startTouch.x) > _maxDragDistance || Mathf.Abs(currentDragPos.y - _startTouch.y) > _maxDragDistance)          
                ChangeDirection();         
        }
    }

    private void ChangeDirection()
    {
        var direction = GenerateDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (direction != new Vector2(0, 0))
            _snakeController.ChangeDirection(direction);

        _isDrag = false;
    }

    private Vector2Int GenerateDirection(Vector2 endTouch)
    {
        var verticalDragDistance = endTouch.y - _startTouch.y;
        var horizontalDragDistance = endTouch.x - _startTouch.x;

        if (Mathf.Abs(verticalDragDistance) < _minDragDistance && Mathf.Abs(horizontalDragDistance) < _minDragDistance)
            return new Vector2Int(0, 0);

        if (verticalDragDistance > Mathf.Abs(horizontalDragDistance))
            return new Vector2Int(0, 1);
        
        if (Mathf.Abs(verticalDragDistance) > Mathf.Abs(horizontalDragDistance))
            return new Vector2Int(0, -1);

        if (horizontalDragDistance > Mathf.Abs(verticalDragDistance))
            return new Vector2Int(1, 0);
        
        if (Mathf.Abs(horizontalDragDistance) > Mathf.Abs(verticalDragDistance))
            return new Vector2Int(-1, 0);

        return new Vector2Int(0, 0);
    }
}
