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
}
