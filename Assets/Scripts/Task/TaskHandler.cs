using UnityEngine;
using UnityEngine.Events;

public class TaskHandler : MonoBehaviour
{
    [SerializeField] private TaskGenerator _taskGenerator;
    [SerializeField] private GridGenerator _gridGenerator;

    private const string EventName = "CellSelected";
    private const string ErrorMessage = "Random Grid Object For Task is null";
    private GridObject _randomGridObjectForTask;

    public event UnityAction<bool, GridObject> SolutionChosen;

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

    private void OnCellSelected(GridObject selectedGridObject)
    {
        if (_randomGridObjectForTask == null)
            Debug.LogError(ErrorMessage);
        else
        {
            bool isTaskSolved = CheckAccuracyTaskSolution(selectedGridObject);

            if (isTaskSolved)
                _gridGenerator.DisableCollisionDetectionForGrid();

            SolutionChosen?.Invoke(isTaskSolved, selectedGridObject);
        }
    }

    private bool CheckAccuracyTaskSolution(GridObject selectedGridObject) =>
        _randomGridObjectForTask.CellIdentifier == selectedGridObject.CellIdentifier;
}
