using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]

public class TaskViewer : MonoBehaviour
{
    [SerializeField] private TaskGenerator _taskGenerator;

    private const string TemplateFormatText = "Find {0}";
    private const string StandartText = "";
    private TMP_Text _taskText;

    private void Awake()
    {
        _taskText = GetComponent<TMP_Text>();
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
    }
}
