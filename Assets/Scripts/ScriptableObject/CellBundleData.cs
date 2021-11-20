using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New CellBundleData", menuName = "Cell Bundle Data/Create Bundle Data", order = 51)]
public class CellBundleData : ScriptableObject
{
    [SerializeField] private GridLayer _layer;
    [SerializeField] private GridObject[] _gridObject;

    public GridLayer Layer => _layer;
    public GridObject[] GridObject => _gridObject;

    public int GetObjectsNumber()
    {
        return _gridObject.Length;
    }

    public GridObject[] GetCollectionCopy()
    {
        GridObject[] collectionCopy = new GridObject[_gridObject.Length];
        Array.Copy(_gridObject, collectionCopy, _gridObject.Length);

        return collectionCopy;
    }
}
