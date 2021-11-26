using UnityEngine;
using DG.Tweening;

public class SceneVisualization : MonoBehaviour
{
    [SerializeField] private TaskHandler _taskHandler;

    private void OnEnable()
    {
        _taskHandler.SolutionChosen += OnSolutionChosen;
    }

    private void OnDisable()
    {
        _taskHandler.SolutionChosen -= OnSolutionChosen;
    }

    private void OnSolutionChosen(bool isTaskSolved, GridObject selectedGridObject)
    { 
  //  if (isTaskSolved)
    }
}
