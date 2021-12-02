using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class TaskViewer : MonoBehaviour
{
    [SerializeField] private bool _isShouldPlayAnimation;
    [SerializeField] private TaskGenerator _taskGenerator;

    private const string TemplateFormatText = "Find {0}";
    private const string StandartText = "";
    private Text _taskText;

    public event UnityAction<Text> TaskTextIsSet;

    private void Awake()
    {
        _taskText = GetComponent<Text>();
        _taskText.text = string.Format(TemplateFormatText, StandartText);
    }

    private void OnEnable()
    {
        _taskGenerator.RandomGridObjectSelected += OnRandomGridObjectSelected;
    }

    private void OnDisable()
    {
        _taskGenerator.RandomGridObjectSelected -= OnRandomGridObjectSelected;
    }

    private void OnRandomGridObjectSelected(GridObject randomGridObject)
    {
        SetTaskText(randomGridObject);
    }

    private void SetTaskText(GridObject randomGridObject)
    {
        if (randomGridObject != null)
            _taskText.text = string.Format(TemplateFormatText, randomGridObject.CellIdentifier);
        else
            _taskText.text = string.Format(TemplateFormatText, StandartText);

        if (_isShouldPlayAnimation)
        {
            _isShouldPlayAnimation = false;
            TaskTextIsSet?.Invoke(_taskText);
        }
    }
}
