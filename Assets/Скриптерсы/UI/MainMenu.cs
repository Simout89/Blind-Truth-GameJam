using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Скриптерсы;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _linkURL;

    [Inject] private SaveRepository _saveRepository;

    private void Awake()
    {
        _saveRepository.PlayerSave = null;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void OpenLink()
    {
        if (!string.IsNullOrEmpty(_linkURL))
        {
            Application.OpenURL(_linkURL);
        }
    }
}