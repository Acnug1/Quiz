using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class GridObject : MonoBehaviour
{
    [SerializeField] private GridLayer _layer;

    private SpriteRenderer _spriteRenderer;

    public GridLayer Layer => _layer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingOrder = (int)_layer;
    }
}
