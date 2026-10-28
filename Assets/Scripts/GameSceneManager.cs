using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManagement : MonoBehaviour
{
    public void LoadCurrentScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }
}
