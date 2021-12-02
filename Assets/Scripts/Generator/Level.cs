using System;
using UnityEngine;

[Serializable]

public class Level
{
    [SerializeField] private int _numberGridColumns;
    [SerializeField] private int _numberGridRows;
    [SerializeField] private float _cellSize;

    public int NumberGridColumns => _numberGridColumns;
    public int NumberGridRows => _numberGridRows;
    public float CellSize => _cellSize;

    private void OnValidate()
    {
        if (_numberGridColumns < 1)
            _numberGridColumns = 1;

        if (_numberGridRows < 1)
            _numberGridRows = 1;

        if (_cellSize <= 0)
            _cellSize = 1;
    }
}
