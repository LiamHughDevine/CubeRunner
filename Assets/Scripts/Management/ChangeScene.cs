using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public static void ChangeToScene(int scene)
    {
        PauseMenu.Paused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
}