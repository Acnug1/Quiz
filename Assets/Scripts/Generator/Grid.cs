using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private CellBundleData[] _dataSet;
    [SerializeField] private GridObject _template;

    private HashSet<GridObject> _gridObjects = new HashSet<GridObject>();

    private int _index;

    private void Start()
    {
        foreach (var cellBundleData in _dataSet)
        {
            foreach (var cellData in cellBundleData.CellData)
            {
              //  _gridObjects[_index++].Init(cellData);
            }
        }
    }
}
