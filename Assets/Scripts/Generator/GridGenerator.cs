using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GridObject[] _templates;
    [SerializeField] private CellBundleData[] _dataSet;
    [SerializeField] private float _gridColumnsSize;
    [SerializeField] private float _gridRowsSize;
    [SerializeField] private float _cellSize;

    private Camera _camera;
    private CellBundleData _cellBundleData;
    private HashSet<GridObject> _gridObjects = new HashSet<GridObject>();
    private HashSet<GridObject> _randomVariants = new HashSet<GridObject>();
    private int _cellCountAxisX;
    private int _cellCountAxisY;

    private void OnValidate()
    {
        if (_gridColumnsSize <= 0)
            _gridColumnsSize = 1;

        if (_gridRowsSize <= 0)
            _gridRowsSize = 1;

        if (_cellSize <= 0)
            _cellSize = 1;
    }

    private void Awake()
    {
        _cellBundleData = TryGetRandomCellBundleData();

        _templates = _templates.Distinct().ToArray();
    }

    private CellBundleData TryGetRandomCellBundleData()
    {
        foreach (var cellBundleData in _dataSet)
        {
            if (cellBundleData.BundleIdentifier != null)
                return _dataSet[Random.Range(0, _dataSet.Length)]; 
        }

        return null;
    }

    private void Start()
    {
        _camera = Camera.main;
        _cellCountAxisX = ReturnCellCountAxis(_gridColumnsSize, _cellSize);
        _cellCountAxisY = ReturnCellCountAxis(_gridRowsSize, _cellSize);

        Vector2 startSpawnPosition = _camera.transform.position - new Vector3(_cellCountAxisX / 2f, _cellCountAxisY / 2f, 0);

        FillGrid(startSpawnPosition, _cellCountAxisX, _cellCountAxisY);
    }

    private int ReturnCellCountAxis(float axisSize, float cellSize)
    {
        return (int)(axisSize / cellSize);
    }

    private void FillGrid(Vector2 startSpawnPosition, int cellCountAxisX, int cellCountAxisY)
    {
        var startFillArea = WorldToGridPosition(startSpawnPosition);

        for (int x = 0; x < cellCountAxisX; x++)
        {
            for (int y = 0; y < cellCountAxisY; y++)
            {
                TryCreateRandomObjectOnLayer(GridLayer.Grid, startFillArea + new Vector2Int(x, y));
                TryCreateRandomObjectOnLayer(GridLayer.OnGrid, startFillArea + new Vector2Int(x, y));
            }
        }
    }

    private void TryCreateRandomObjectOnLayer(GridLayer layer, Vector2Int gridPosition)
    {
        GridObject template = TryGetRandomTemplate(layer);

        if (template == null)
            return;

        Vector2 position = GridToWorldPosition(gridPosition);

        GridObject gridObject = Instantiate(template, position, Quaternion.identity, transform);
        _gridObjects.Add(gridObject);
    }

    private GridObject TryGetRandomTemplate(GridLayer layer)
    {
        GridObject randomVariant;

        var variants = _templates.Where(template => template.Layer == layer).ToArray();

        if (variants.Length == 1)
            return variants[0];
        else
        if (variants.Length > 1 && variants.Length >= _cellCountAxisX * _cellCountAxisY)
        {
            do
            {
                randomVariant = variants[Random.Range(0, variants.Length)];
            }
            while (_randomVariants.Contains(randomVariant));

            _randomVariants.Add(randomVariant);

            return randomVariant;
        }
        else
            return null;
    }

    private void ClearGrid()
    {
        foreach (GridObject gridObject in _gridObjects)
        {
            Destroy(gridObject.gameObject);
        }

        _gridObjects = null;
        _randomVariants = null;
    }

    private Vector2 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector2(
            gridPosition.x * _cellSize,
            gridPosition.y * _cellSize);
    }

    private Vector2Int WorldToGridPosition(Vector2 worldPosition)
    {
        return new Vector2Int(
            (int)(worldPosition.x / _cellSize),
            (int)(worldPosition.y / _cellSize));
    }
}
