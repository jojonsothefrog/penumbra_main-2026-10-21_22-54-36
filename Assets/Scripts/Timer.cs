using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float time;
    public TextMeshProUGUI timerText;
    public GameObject winUI;

    public float secondsPerHour = 85f;

    private bool hasWon = false;

    void Update()
    {
        if (hasWon) { return; }

        time += Time.deltaTime;

        int currentHour = Mathf.FloorToInt(time / secondsPerHour);

        timerText.text = currentHour.ToString("00") + "h";

        if (currentHour >= 7)
        {
            hasWon = true;
            Victory();
        }
    }

    void Victory()
    {
        Debug.Log("O jogador sobreviveu até as 07h.");
        Time.timeScale = 0;
        winUI.SetActive(true);
    }
}
