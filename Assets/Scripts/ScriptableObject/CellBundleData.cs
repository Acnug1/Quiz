using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New CellBundleData", menuName = "Cell Bundle Data/Create Bundle Data", order = 51)]
public class CellBundleData : ScriptableObject
{
    [SerializeField] private GridLayer _layer;
    [SerializeField] private GridObject[] _gridObjects;

    private const string ErrorMessage = "Fields Cell Identifier do not have a unique name in collection {0}";

    public GridLayer Layer => _layer;

    private void OnValidate()
    {
        Dictionary<string, int> dictionary = new Dictionary<string, int>();

        foreach (var gridObject in _gridObjects)
        {
            if (dictionary.ContainsKey(gridObject.CellIdentifier))
                dictionary[gridObject.CellIdentifier] += 1;
            else
                dictionary[gridObject.CellIdentifier] = 1;
        }

        Debug.Assert(dictionary.Values.Sum() == dictionary.Count, string.Format(ErrorMessage, this.name));
    }

    public GridObject[] GetSuitableCollectionCopy(int requiredCollectionLenght)
    {
        GridObject[] collectionCopy = new GridObject[GetObjectsNumber()];
        Array.Copy(_gridObjects, collectionCopy, GetObjectsNumber());

        collectionCopy = GetSuitableCollection(collectionCopy, requiredCollectionLenght);

        return collectionCopy;
    }

    private int GetObjectsNumber() => _gridObjects.Length;

    private GridObject[] GetSuitableCollection(GridObject[] gridObjects, int requiredCollectionLenght)
    {
        if (CheckCollectionToSuitable(gridObjects.Length, requiredCollectionLenght))
            return gridObjects;

        List<GridObject> gridObjectsList = gridObjects.ToList();

        while (gridObjectsList.Count < requiredCollectionLenght)
            gridObjectsList.AddRange(gridObjects);

        return gridObjectsList.ToArray();
    }

    private bool CheckCollectionToSuitable(int gridObjectsLength, int requiredCollectionLenght) => gridObjectsLength >= requiredCollectionLenght;
}
