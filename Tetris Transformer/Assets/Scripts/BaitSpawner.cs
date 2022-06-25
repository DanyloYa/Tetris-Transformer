using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _baitPrefab;
    [SerializeField] private Transform _baitParent;

    private Grid _grid;

    private GridMatrix _gridMatrix;

    private List<GameObject> _baits;

    private List<Vector2Int> _baitsGridPosition;

    private int _baitCount = 1;

    public void StartSpawn()
    {
        _baitCount = PlayerPrefs.GetInt("BaitCount");

        _baits = new List<GameObject>();
        _baitsGridPosition = new List<Vector2Int>();

        _grid = GetComponentInParent<Grid>();
        _gridMatrix = _grid.GetMatrix();
    
        SpawnBait();
    }

    public void SpawnBait()
    {
        for (int i = 0; i < _baitCount; i++)
        {
            var baitGridPosition =  new Vector2Int(Random.Range(0, _grid.width), _grid.height / 2 + Random.Range(0, 2));

            var bait = Instantiate(_baitPrefab, _gridMatrix.GetPosition(baitGridPosition), Quaternion.identity, _baitParent);

            _grid.MultiplayScale(bait);

            _baits.Add(bait);
            _baitsGridPosition.Add(baitGridPosition);
        }
    } 

    public bool IsBait(Vector2Int point)
    {
        foreach(var i in _baitsGridPosition)
        {
            if (point == i)
            {
                PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 2);

                foreach (var j in _baits)
                    Destroy(j);

                _baits.Clear();
                _baitsGridPosition.Clear();

                SpawnBait();
            
                return true;
            }
        }

        return false;
    }
}
