using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private LevelsGenerator _levelsGenerator;
    [SerializeField] private Image _fadeScreen;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Image _loadScreen;
    [SerializeField] private SceneVisualization _sceneVisualization;

    private static bool _isShouldPlayLoadingAnimation = false;
    private AsyncOperation _asyncLoad;

    public event UnityAction<GridGenerator, Image, bool> FirstLevelLoad;
    public event UnityAction<Image> FadeScreenEnabled;
    public event UnityAction<Image> RestartButtonClick;

    private void Awake()
    {
        DisableMenuObject(_fadeScreen.gameObject);
        DisableMenuObject(_restartButton.gameObject);

        if (_isShouldPlayLoadingAnimation)
            SetLoadScreenAlpha(1);
        else
            SetLoadScreenAlpha(0);
    }

    private void DisableMenuObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void SetLoadScreenAlpha(float alpha)
    {
        _loadScreen.color = new Vector4(1, 1, 1, alpha);
    }

    private void OnEnable()
    {
        _levelsGenerator.FirstLevelGenerated += OnFirstLevelGenerated;
        _levelsGenerator.GameFinished += OnGameFinished;
        _sceneVisualization.FadeInOver += OnFadeInOver;
    }

    private void OnDisable()
    {
        _levelsGenerator.FirstLevelGenerated -= OnFirstLevelGenerated;
        _levelsGenerator.GameFinished -= OnGameFinished;
        _sceneVisualization.FadeInOver -= OnFadeInOver;
    }

    public void Restart()
    {
        RestartButtonClick?.Invoke(_loadScreen);

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        _asyncLoad = SceneManager.LoadSceneAsync(currentScene);
        _asyncLoad.allowSceneActivation = false;
    }

    private void OnFadeInOver()
    {
        _isShouldPlayLoadingAnimation = true;
        _asyncLoad.allowSceneActivation = true;
    }

    private void OnFirstLevelGenerated(GridGenerator gridGenerator)
    {
        FirstLevelLoad?.Invoke(gridGenerator, _loadScreen, _isShouldPlayLoadingAnimation);
    }

    private void OnGameFinished()
    {
        EnableMenuObject(_restartButton.gameObject);
        EnableMenuObject(_fadeScreen.gameObject);

        FadeScreenEnabled?.Invoke(_fadeScreen);
    }

    private void EnableMenuObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
}
