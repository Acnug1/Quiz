using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

public class SceneVisualization : MonoBehaviour
{
    [SerializeField] private ParticleSystem _starsVFX;
    [SerializeField] private TaskHandler _taskHandler;
    [SerializeField] private TaskViewer _taskViewer;
    [SerializeField] private GameMenu _gameMenu;

    private Coroutine _easeOutBounceAnimation;
    private Coroutine _fadeInAnimation;
    private Tween _bounceTween;
    private float _bounceDuration = 1;
    private float _fadeDuration = 1;

    public event UnityAction FadeInOver;

    private void OnEnable()
    {
        _taskHandler.SolutionChosen += OnSolutionChosen;
        _taskViewer.TaskTextIsSet += OnTaskTextIsSet;
        _gameMenu.FirstLevelLoad += OnFirstLevelLoad;
        _gameMenu.FadeScreenEnabled += OnFadeScreenEnabled;
        _gameMenu.RestartButtonClick += OnRestartButtonClick;
    }

    private void OnDisable()
    {
        _taskHandler.SolutionChosen -= OnSolutionChosen;
        _taskViewer.TaskTextIsSet -= OnTaskTextIsSet;
        _gameMenu.FirstLevelLoad -= OnFirstLevelLoad;
        _gameMenu.FadeScreenEnabled -= OnFadeScreenEnabled;
        _gameMenu.RestartButtonClick -= OnRestartButtonClick;
    }

    private void OnFirstLevelLoad(GridGenerator gridGenerator, Image loadScreen, bool isShouldPlayLoadingAnimation)
    {
        if (isShouldPlayLoadingAnimation)
            FadeOutImage(loadScreen);

        if (_easeOutBounceAnimation != null)
            StopCoroutine(_easeOutBounceAnimation);

        _easeOutBounceAnimation = StartCoroutine(EaseOutBounceAnimation(gridGenerator, _bounceDuration));
    }

    private IEnumerator EaseOutBounceAnimation(GridGenerator gridGenerator, float bounceDuration)
    {
        gridGenerator.DisableCollisionDetectionForGrid();
        BounceForGrid(gridGenerator);

        WaitForSeconds waitForSeconds = new WaitForSeconds(bounceDuration);
        yield return waitForSeconds;

        gridGenerator.EnableCollisionDetectionForGrid();
    }

    private void BounceForGrid(GridGenerator gridGenerator)
    {
        GridObject[] gridObjects = gridGenerator.GetComponentsInChildren<GridObject>();

        foreach (GridObject gridObject in gridObjects)
        {
            EaseOutBounce(gridObject, false);
        }
    }

    private void OnSolutionChosen(bool isTaskSolved, GridObject selectedGridObject)
    {
        if (isTaskSolved)
        {
            PlayVFX(_starsVFX);
            EaseOutBounce(selectedGridObject, true);
        }
        else
            EaseInBounce(selectedGridObject, true);
    }

    private void PlayVFX(ParticleSystem starsVFX)
    {
        if (starsVFX)
            Instantiate(starsVFX, transform.position, Quaternion.identity, transform);
    }

    private void EaseOutBounce(GridObject gridObject, bool isLoop)
    {
        Vector3 currentScale = gridObject.GetScale();
        gridObject.SetScale(0);
        Bounce(gridObject, currentScale, _bounceDuration, Ease.OutBounce, isLoop);
    }

    private void EaseInBounce(GridObject gridObject, bool isLoop)
    {
        _bounceTween?.Kill(true);
        Bounce(gridObject, Vector3.zero, _bounceDuration, Ease.InBounce, isLoop);
    }

    private void Bounce(GridObject gridObject, Vector3 value, float duration, Ease ease, bool isLoop)
    {
        _bounceTween = gridObject.transform.DOScale(value, duration).SetEase(ease);

        if (isLoop)
            _bounceTween.SetLoops(2, LoopType.Yoyo);
    }

    private void OnTaskTextIsSet(Text taskText)
    {
        FadeInText(taskText, 1);
    }

    private void FadeInText(Text text, float value)
    {
        Fade(text, 0, 0);
        Fade(text, value, _fadeDuration);
    }

    private void OnFadeScreenEnabled(Image fadeScreen)
    {
        FadeInImage(fadeScreen, 0.5f);
    }

    private void OnRestartButtonClick(Image loadScreen)
    {
        if (_fadeInAnimation != null)
            StopCoroutine(_fadeInAnimation);

        _fadeInAnimation = StartCoroutine(FadeInAnimation(loadScreen, _fadeDuration));
    }

    private IEnumerator FadeInAnimation(Image loadScreen, float fadeDuration)
    {
        FadeInImage(loadScreen, 1);

        WaitForSeconds waitForSeconds = new WaitForSeconds(fadeDuration);
        yield return waitForSeconds;

        FadeInOver?.Invoke();
    }

    private void FadeInImage(Image image, float value)
    {
        Fade(image, 0, 0);
        Fade(image, value, _fadeDuration);
    }

    private void FadeOutImage(Image image)
    {
        Fade(image, 0, _fadeDuration);
    }

    private void Fade(Text text, float value, float duration)
    {
        text.DOFade(value, duration);
    }

    private void Fade(Image image, float value, float duration)
    {
        image.DOFade(value, duration);
    }
}
