using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private CellBundleData[] _bundles;
    [SerializeField] private float _gridColumnsSize;
    [SerializeField] private float _gridRowsSize;
    [SerializeField] private float _cellSize;

    private Camera _camera;
    private CellBundleData _onGridBundleData;
    private CellBundleData _gridBundleData;
    private HashSet<GridObject> _gridObjects = new HashSet<GridObject>();
    private int _cellCountAxisX;
    private int _cellCountAxisY;

    public event UnityAction<List<GridObject>> CopyCollectionGot;

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
        _gridBundleData = GetRandomCellBundleData(GridLayer.Grid);
        _onGridBundleData = GetRandomCellBundleData(GridLayer.OnGrid);
    }

    private CellBundleData GetRandomCellBundleData(GridLayer layer)
    {
        CellBundleData[] suitableBundles = _bundles.Where(_bundle => _bundle.Layer == layer).ToArray();

        return suitableBundles[Random.Range(0, suitableBundles.Length)];
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
        GridObject[] gridObjects = GetRandomGridObjects(_gridBundleData);
        GridObject[] onGridObjects = GetRandomGridObjects(_onGridBundleData);
        int objectIndex = 0;

        for (int x = 0; x < cellCountAxisX; x++)
        {
            for (int y = 0; y < cellCountAxisY; y++)
            {
                CreateObjectOnLayer(GridLayer.Grid, startFillArea + new Vector2Int(x, y), gridObjects[objectIndex]);
                CreateObjectOnLayer(GridLayer.OnGrid, startFillArea + new Vector2Int(x, y), onGridObjects[objectIndex++]);
            }
        }
    }

    private GridObject[] GetRandomGridObjects(CellBundleData bundle)
    {
        List<GridObject> collectionCopy = GetSuitableCollectionCopyAsList(bundle);

        GridObject[] result = new GridObject[GetTotalCells()];

        for (int i = 0; i < result.Length; i++)
        {
            GridObject cell = collectionCopy[Random.Range(0, collectionCopy.Count)];
            result[i] = cell;
            collectionCopy.Remove(cell);
        }

        if (bundle.Layer == GridLayer.OnGrid)
            CopyCollectionGot?.Invoke(result.ToList());

        return result;
    }

    private List<GridObject> GetSuitableCollectionCopyAsList(CellBundleData bundle)
    {
        return bundle.GetSuitableCollectionCopy(GetTotalCells()).ToList();
    }

    private void CreateObjectOnLayer(GridLayer layer, Vector2Int gridPosition, GridObject gridObject)
    {
        Vector2 position = GridToWorldPosition(gridPosition);

        GridObject newObject = Instantiate(gridObject, position, Quaternion.identity, transform);
        newObject.SetLayer(layer);
        _gridObjects.Add(newObject);
    }

    private int GetTotalCells()
    {
        return _cellCountAxisX * _cellCountAxisY;
    }

    private void ClearGrid()
    {
        foreach (GridObject gridObject in _gridObjects)
        {
            Destroy(gridObject.gameObject);
        }

        _gridObjects = null;
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
