using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _linkURL; 

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