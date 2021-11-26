using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(GridObject))]
[RequireComponent(typeof(BoxCollider2D))]

public class CellCollisionHandler : MonoBehaviour, IPointerClickHandler
{
    private const string EventName = "CellSelected";
    private GridObject _gridObject;
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        _gridObject = GetComponent<GridObject>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.size = new Vector2(1, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent(EventName, _gridObject);
    }
}
