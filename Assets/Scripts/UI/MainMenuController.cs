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
#if UNITY_EDITOR // Código específico para el editor de Unity 
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Código para la build del juego
        Application.Quit();
#endif
    }
}
