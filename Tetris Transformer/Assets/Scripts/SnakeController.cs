using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class SnakeController : MonoBehaviour
{ 
    [SerializeField] private GameObject _bodyPrefab;
    [SerializeField] private Transform _bodyParent;

    [SerializeField] private BaitSpawner _baitSpawner;

    [SerializeField] private float _delayBetweenMoves;
    [SerializeField] private float _delayBetweenNextSpawn;

    [SerializeField] private int bodyLength = 4;

    private Grid _grid;
    private GridMatrix _gridMatrix;

    private List<Vector2Int> _bodiesGridPosition = new List<Vector2Int>();

    private Vector2Int _nextDirection = new Vector2Int(0, -1);
    private Vector2Int _currentDirection = new Vector2Int(0, -1);
    private Vector2Int _spawnMatrixPos = new Vector2Int(0, 0);

    public void ChangeDirection(Vector2Int newDirection) => _nextDirection = -newDirection != _currentDirection ? newDirection : _currentDirection;

    public void StartSpawn()
    {
        bodyLength = PlayerPrefs.GetInt("BodyLength");
        _delayBetweenMoves = (6f - PlayerPrefs.GetInt("BodySpeed")) / 10f;

        _grid = GetComponentInParent<Grid>();
        _gridMatrix = _grid.GetMatrix();

        SpawnHead();
    }

    private void SpawnHead()
    {
        _bodiesGridPosition.Clear();

        _spawnMatrixPos = new Vector2Int(Random.Range(0, _grid.width), _grid.height - 1);

        if (_gridMatrix.GetObject(_spawnMatrixPos) != null)
            return;

        SpawnBody();

        StartCoroutine(Move());
    }

    private void SpawnBody()
    {
        var obj = Instantiate(_bodyPrefab, _gridMatrix.GetPosition(_spawnMatrixPos), Quaternion.identity, _bodyParent);

        _grid.MultiplayScale(obj);
        _gridMatrix.SetObject(_spawnMatrixPos, obj);

        _bodiesGridPosition.Add(_spawnMatrixPos);
    }

    private void SpawnNewHead()
    {
        StartFalling();

        _gridMatrix.DestroyFilledLines();

        _nextDirection = new Vector2Int(0, -1);

        SpawnHead();
    }


    private IEnumerator Move()
    {
        while(true)
        {
            yield return new WaitForSeconds(_delayBetweenMoves);

            _currentDirection = _nextDirection;

            if (!_grid.OnGrid(_bodiesGridPosition[0] + _currentDirection) || _gridMatrix.GetElement(_bodiesGridPosition[0] + _currentDirection).obj != null)
            {
                yield return new WaitForSeconds(_delayBetweenNextSpawn);
                SpawnNewHead();
                break;
            }

            //Переміщаю об'єкти
            _gridMatrix.MoveElement(_bodiesGridPosition[0], _currentDirection);

            for (int i = 1; i < _bodiesGridPosition.Count; i++)
                _gridMatrix.MoveElement(_bodiesGridPosition[i], _bodiesGridPosition[i - 1] - _bodiesGridPosition[i]);     

            //Переміщаю позиції
            for (int i = _bodiesGridPosition.Count - 1; i >= 1 ; i--)
                _bodiesGridPosition[i] = _bodiesGridPosition[i - 1];     

            _bodiesGridPosition[0] += _currentDirection;
   

            if (_bodiesGridPosition.Count < bodyLength)
                SpawnBody();

            if (_baitSpawner.IsBait(_bodiesGridPosition[0]))
            {
                yield return new WaitForSeconds(_delayBetweenNextSpawn);
                SpawnNewHead();
                break;
            }
        }
    }

    private void StartFalling()
    {
        List<Vector2Int> bodiesGridPosition = new List<Vector2Int>();

        for (int i = 0; i < _grid.height; i++)
            foreach (var gridPosition in _bodiesGridPosition)
                if (i == gridPosition.y)
                    bodiesGridPosition.Add(gridPosition);   

        while(true)
        {
            for (int i = 0; i < bodiesGridPosition.Count; i++)
            {
                var newGridPosition = bodiesGridPosition[i] + new Vector2Int(0, -1);

                if (!_grid.OnGrid(newGridPosition) || _gridMatrix.GetObject(newGridPosition) != null && !bodiesGridPosition.Contains(newGridPosition))
                    return;
            }

            for (int i = 0; i < bodiesGridPosition.Count; i++)
            {
                _gridMatrix.MoveElement(bodiesGridPosition[i], new Vector2Int(0, -1));
                bodiesGridPosition[i] += new Vector2Int(0, -1);
            }
        }
    }  
}
