using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject PauseMenuUI;
    public GameObject ControlMenuUI;
    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        ControlMenuUI.SetActive(true);
        Time.timeScale = 1f;
        Paused = false;
    }

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        ControlMenuUI.SetActive(false);
        Time.timeScale = 0f;
        Paused = true;
    }
}
