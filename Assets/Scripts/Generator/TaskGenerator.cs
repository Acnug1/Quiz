using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridGenerator))]

public class TaskGenerator : MonoBehaviour
{
    private GridGenerator _gridGenerator;
    private List<GridObject> _gridObjectsForTasks = new List<GridObject>();

    public event UnityAction<GridObject> RandomGridObjectSelected;

    private void Awake()
    {
        _gridGenerator = GetComponent<GridGenerator>();
    }

    private void OnEnable()
    {
        _gridGenerator.CopyCollectionGot += OnCopyCollectionGot;
    }

    private void OnDisable()
    {
        _gridGenerator.CopyCollectionGot -= OnCopyCollectionGot;
    }

    private void OnCopyCollectionGot(List<GridObject> collectionCopy)
    {
        GridObject randomGridObject = TryGetRandomGridObject(collectionCopy);
        RandomGridObjectSelected?.Invoke(randomGridObject);
    }

    private GridObject TryGetRandomGridObject(List<GridObject> collectionCopy)
    {
        GridObject randomGridObject;
        int iterationsNumber = 0;

        do
        {
            iterationsNumber++;
            randomGridObject = collectionCopy[Random.Range(0, collectionCopy.Count)];

            if (iterationsNumber > collectionCopy.Count)
                return null;
        }
        while (_gridObjectsForTasks.Contains(randomGridObject));

        _gridObjectsForTasks.Add(randomGridObject);

        return randomGridObject;
    }
}
