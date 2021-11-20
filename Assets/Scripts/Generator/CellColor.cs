using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class CellColor : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float _alpha;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        float red = GetRandomColorValue();
        float green = GetRandomColorValue();
        float blue = GetRandomColorValue();

        _spriteRenderer.color = new Vector4(red, green, blue, _alpha);
    }

    private float GetRandomColorValue()
    {
        return Random.Range(0, 1f);
    }
}
