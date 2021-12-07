using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class GridObject : MonoBehaviour
{
    [SerializeField] private string _cellIdentifier;

    private SpriteRenderer _spriteRenderer;

    public string CellIdentifier => _cellIdentifier;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetLayer(GridLayer layer)
    {
        _spriteRenderer.sortingOrder = (int)layer;
    }

    public Vector3 GetScale() => transform.localScale;

    public void SetScale(float cellSize)
    {
        if (cellSize < 0)
            throw new ArgumentOutOfRangeException(nameof(cellSize));

        transform.localScale *= cellSize;
    }
}
