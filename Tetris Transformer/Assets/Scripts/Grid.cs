using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private GameObject _gridBackground;
    [SerializeField] private Transform _backgroundParent;
    [SerializeField] public int height;
    [SerializeField] public int width;

    [Header("Adaptive(in %)")]
    [SerializeField] private Vector2 _leftBottomPoint;
    [SerializeField] private Vector2 _rightTopPoint;

    [Header("Gameplay")]
    [SerializeField] private BaitSpawner _baitSpawner;
    [SerializeField] private SnakeController _snakeController;

    private Vector2 _scaleMultiplayer;
    private GridMatrix _matrix;


    public GridMatrix GetMatrix() => _matrix;

    private void Start()
    {
        width = PlayerPrefs.GetInt("FieldWidth");
        height = PlayerPrefs.GetInt("FieldHeight");

        _matrix = new GridMatrix(width, height);

        GenerateGrid();   

        _baitSpawner.StartSpawn();
        _snakeController.StartSpawn();
    }

    private void GenerateGrid()
    {
        var camera = Camera.main;

        if (camera.pixelHeight / camera.pixelWidth >= 2.0f)
        {
            _leftBottomPoint.x = 0.05f;
            _rightTopPoint.x = 0.05f;
        }

        _leftBottomPoint = camera.ScreenToWorldPoint(new Vector2(camera.pixelWidth * _leftBottomPoint.x, camera.pixelHeight * _leftBottomPoint.y));
        _rightTopPoint = camera.ScreenToWorldPoint(new Vector2(camera.pixelWidth - camera.pixelWidth * _rightTopPoint.x, camera.pixelHeight - camera.pixelHeight * _rightTopPoint.y));

        var offset = _scaleMultiplayer = new Vector2((_rightTopPoint.x - _leftBottomPoint.x) / width, (_rightTopPoint.x - _leftBottomPoint.x) / width);        
        var spawnPosition = _leftBottomPoint;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _matrix.SetElement(i, j, spawnPosition);

                MultiplayScale(Instantiate(_gridBackground, spawnPosition, Quaternion.identity, _backgroundParent));

                spawnPosition.y += offset.y;
            }

            spawnPosition = new Vector2(spawnPosition.x + offset.x, _leftBottomPoint.y);
        }
    }

    public void MultiplayScale(GameObject obj) => obj.transform.localScale = new Vector3(obj.transform.localScale.x * _scaleMultiplayer.x, obj.transform.localScale.y * _scaleMultiplayer.y, 1);

    public bool OnGrid(Vector2Int gridPosition)
    {
        if (gridPosition.x > width - 1 || gridPosition.x < 0 || gridPosition.y > height - 1 || gridPosition.y < 0)
            return false;

        return true;
    }
}
