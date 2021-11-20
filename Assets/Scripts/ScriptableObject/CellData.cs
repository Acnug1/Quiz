using System;
using UnityEngine;

[Serializable]
public class CellData
{
    [SerializeField] private string _cellIdentifier;
    [SerializeField] private GridObject _gridObject;

    public string CellIdentifier => _cellIdentifier;
    public GridObject GridObject => _gridObject;
}
