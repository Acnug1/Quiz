using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private CellBundleData[] _bundles;

    private Camera _camera;
    private float _cellSize;
    private int _numberCellsOnAxisX;
    private int _numberCellsOnAxisY;
    private HashSet<GridObject> _gridObjects = new HashSet<GridObject>();

    public event UnityAction<List<GridObject>> CopyCollectionGot;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void ClearGrid()
    {
        if (_gridObjects.Count == 0)
            return;

        foreach (GridObject gridObject in _gridObjects)
        {
            Destroy(gridObject.gameObject);
        }

        _gridObjects.Clear();
    }

    public void Init(int numberGridColumns, int numberGridRows, float cellSize)
    {
        CellBundleData gridBundleData;
        CellBundleData onGridBundleData;

        gridBundleData = GetRandomCellBundleData(GridLayer.Grid);
        onGridBundleData = GetRandomCellBundleData(GridLayer.OnGrid);

        _numberCellsOnAxisX = numberGridColumns;
        _numberCellsOnAxisY = numberGridRows;
        _cellSize = cellSize;

        Vector2 startSpawnPosition = GetStartSpawnPosition();

        FillGrid(startSpawnPosition, _numberCellsOnAxisX, _numberCellsOnAxisY, gridBundleData, onGridBundleData);
    }

    public void EnableCollisionDetectionForGrid()
    {
        CollisionDetection(true);
    }

    public void DisableCollisionDetectionForGrid()
    {
        CollisionDetection(false);
    }

    private void CollisionDetection(bool isEnableDetection)
    {
        CellCollisionHandler[] cellsWithColliders = GetComponentsInChildren<CellCollisionHandler>();

        foreach (CellCollisionHandler cellWithCollider in cellsWithColliders)
        {
            if (isEnableDetection)
                cellWithCollider.EnableCollisionDetection();
            else
                cellWithCollider.DisableCollisionDetection();
        }
    }

    private CellBundleData GetRandomCellBundleData(GridLayer layer)
    {
        CellBundleData[] suitableBundles = _bundles.Where(_bundle => _bundle.Layer == layer).ToArray();

        return suitableBundles[Random.Range(0, suitableBundles.Length)];
    }

    private Vector2 GetStartSpawnPosition() =>
        _camera.transform.position - new Vector3(_numberCellsOnAxisX * _cellSize / 2f, _numberCellsOnAxisY * _cellSize / 2f, 0);

    private void FillGrid(Vector2 startSpawnPosition, int numberCellsOnAxisX, int numberCellsOnAxisY,
        CellBundleData gridBundleData, CellBundleData onGridBundleData)
    {
        Vector2Int startFillArea = WorldToGridPosition(startSpawnPosition);

        GridObject[] gridObjects = GetRandomGridObjects(gridBundleData);
        GridObject[] onGridObjects = GetRandomGridObjects(onGridBundleData);

        int objectIndex = 0;

        for (int x = 0; x < numberCellsOnAxisX; x++)
        {
            for (int y = 0; y < numberCellsOnAxisY; y++)
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

        if (collectionCopy != null)
            collectionCopy = null;

        if (bundle.Layer == GridLayer.OnGrid)
            CopyCollectionGot?.Invoke(result.ToList());

        return result;
    }

    private List<GridObject> GetSuitableCollectionCopyAsList(CellBundleData bundle) =>
        bundle.GetSuitableCollectionCopy(GetTotalCells()).ToList();

    private void CreateObjectOnLayer(GridLayer layer, Vector2Int gridPosition, GridObject gridObject)
    {
        Vector2 position = GridToWorldPosition(gridPosition);

        GridObject newObject = Instantiate(gridObject, position, Quaternion.identity, transform);
        newObject.SetLayer(layer);
        newObject.SetScale(_cellSize);
        _gridObjects.Add(newObject);
    }

    private int GetTotalCells()
    {
        return _numberCellsOnAxisX * _numberCellsOnAxisY;
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
