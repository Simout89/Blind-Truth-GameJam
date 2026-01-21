using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public enum FadeMode
    {
        StartGame,
        ExitGame
    }

    [Header("UI References")]
    [SerializeField] private Image fadeImage;

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private Ease fadeEase = Ease.InOutSine;

    [Header("Sound Settings")]
    [SerializeField] private EventReference fadeSound;

    [Header("Music Event")]
    [SerializeField] private EventReference musicEvent; // Event, который играет музыку

    [Header("Fade Options")]
    [SerializeField] private FadeMode mode = FadeMode.StartGame;
    [SerializeField] private string sceneToLoad;

    private EventInstance musicInstance;

    private void Awake()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
            var color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;
        }

        // Создаём экземпляр музыки и запускаем его
        if (!musicEvent.IsNull)
        {
            musicInstance = RuntimeManager.CreateInstance(musicEvent);
            musicInstance.start();
        }
    }

    /// <summary>
    /// Запустить фейд
    /// </summary>
    public void StartFade()
    {
        if (fadeImage == null) return;

        // Останавливаем музыку с fadeout
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
        }

        fadeImage.gameObject.SetActive(true);

        float targetAlpha = 1f;

        if (fadeSound.IsNull == false)
            RuntimeManager.PlayOneShot(fadeSound);

        fadeImage.DOFade(targetAlpha, fadeDuration)
                 .SetEase(fadeEase)
                 .OnComplete(OnFadeFinished);
    }

    private void OnFadeFinished()
    {
        switch (mode)
        {
            case FadeMode.StartGame:
                if (!string.IsNullOrEmpty(sceneToLoad))
                    SceneManager.LoadScene(sceneToLoad);
                else
                    Debug.LogWarning("Не указана сцена для загрузки!");
                break;

            case FadeMode.ExitGame:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }

    public void FadeStartGame(string sceneName)
    {
        mode = FadeMode.StartGame;
        sceneToLoad = sceneName;
        StartFade();
    }

    public void FadeExitGame()
    {
        mode = FadeMode.ExitGame;
        StartFade();
    }
}
