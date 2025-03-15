using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void LoadNormalMode()
    {
        PlayerPrefs.SetString("GameMode", "Normal");
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadReloadedMode()
    {
        PlayerPrefs.SetString("GameMode", "Reloaded");
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR // C�digo espec�fico para el editor de Unity 
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // C�digo para la build del juego
        Application.Quit();
#endif
    }
}
