using UnityEngine;
using UnityEngine.Events;

public class TaskHandler : MonoBehaviour
{
    [SerializeField] private TaskGenerator _taskGenerator;

    private const string EventName = "CellSelected";
    private const string ErrorMessage = "Random Grid Object For Task is null";
    private GridObject _randomGridObjectForTask;

    public event UnityAction<bool> SolutionChosen;

    private void OnEnable()
    {
        _taskGenerator.RandomGridObjectSelected += OnRandomGridObjectSelected;
        EventManager.StartListening(EventName, OnCellSelected);
    }

    private void OnDisable()
    {
        _taskGenerator.RandomGridObjectSelected -= OnRandomGridObjectSelected;
        EventManager.StopListening(EventName, OnCellSelected);
    }

    private void OnRandomGridObjectSelected(GridObject randomGridObjectForTask)
    {
        _randomGridObjectForTask = randomGridObjectForTask;
    }

    private void OnCellSelected(string selectedCellIdentifier)
    {
        if (_randomGridObjectForTask == null)
            Debug.LogError(ErrorMessage);
        else
        {
            bool isTaskSolved = CheckAccuracyTaskSolution(selectedCellIdentifier);
            SolutionChosen?.Invoke(isTaskSolved);
        }
    }

    private bool CheckAccuracyTaskSolution(string selectedCellIdentifier) => _randomGridObjectForTask.CellIdentifier == selectedCellIdentifier;
}
