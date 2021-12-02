using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridGenerator))]

public class LevelsGenerator : MonoBehaviour
{
    [SerializeField] private float _timeToNextLevel;
    [SerializeField] private TaskHandler _taskHandler;
    [SerializeField] private List<Level> _levelsList;

    private GridGenerator _gridGenerator;
    private int _levelIndex = 0;

    public event UnityAction<GridGenerator> FirstLevelGenerated;
    public event UnityAction GameFinished;

    private void OnValidate()
    {
        if (_timeToNextLevel < 3)
            _timeToNextLevel = 3;
    }

    private void OnEnable()
    {
        _taskHandler.SolutionChosen += OnSolutionChosen;
    }

    private void OnDisable()
    {
        _taskHandler.SolutionChosen -= OnSolutionChosen;
    }

    private void Start()
    {
        _gridGenerator = GetComponent<GridGenerator>();
        TryGenerateNextLevel();
    }

    private void OnSolutionChosen(bool isTaskSolved, GridObject selectedGridObject)
    {
        if (isTaskSolved)
            Invoke(nameof(TryGenerateNextLevel), _timeToNextLevel);
    }

    private void TryGenerateNextLevel()
    {
        if (_levelIndex < _levelsList.Count)
        {
            _gridGenerator.ClearGrid();
            _gridGenerator.Init(_levelsList[_levelIndex].NumberGridColumns,
                _levelsList[_levelIndex].NumberGridRows, _levelsList[_levelIndex].CellSize);

            if (_levelIndex == 0)
                FirstLevelGenerated?.Invoke(_gridGenerator);

            _levelIndex++;
        }
        else
            GameFinished?.Invoke();
    }
}
