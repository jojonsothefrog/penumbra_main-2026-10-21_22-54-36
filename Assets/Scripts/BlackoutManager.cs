
using UnityEngine;

public class BlackoutManager : MonoBehaviour
{
    [Header("Referências")]
    public Timer gameTimer;
    public GameObject globalLight;

    [Header("Primeiro apagão")]
    public int firstBlackoutHour = 1;

    [Header("Intervalos de apagão (segundos reais)")]
    public float earlyMinInterval = 50f;
    public float earlyMaxInterval = 70f;

    public float lateMinInterval = 8f;
    public float lateMaxInterval = 20f;

    [Header("Progressão")]
    public int earlyHour = 1;
    public int lateHour = 6;

    private bool firstBlackoutTriggered = false;

    private float blackoutTimer = 0f;

    void Update()
    {
        if (gameTimer == null || globalLight == null)
        {
            return;
        }

        int currentHour = Mathf.FloorToInt(
            gameTimer.time / gameTimer.secondsPerHour
        );

        // Não realiza apagões depois das 07h
        if (currentHour >= 7)
        {
            return;
        }

        // Primeiro apagão: acontece às 01h
        if (!firstBlackoutTriggered)
        {
            if (currentHour >= firstBlackoutHour &&
                globalLight.activeSelf)
            {
                TriggerBlackout();

                firstBlackoutTriggered = true;

                blackoutTimer = GetRandomInterval(currentHour);
            }

            return;
        }

        // O intervalo só passa enquanto as luzes estão ligadas
        if (globalLight.activeSelf)
        {
            blackoutTimer -= Time.deltaTime;

            if (blackoutTimer <= 0f)
            {
                TriggerBlackout();

                blackoutTimer = GetRandomInterval(currentHour);
            }
        }
    }

    void TriggerBlackout()
    {
        globalLight.SetActive(false);

        Debug.Log("Apagão! As luzes foram desligadas.");
    }

    float GetRandomInterval(int currentHour)
    {
        // Progresso de 0 a 1 entre as horas definidas
        float progress = Mathf.InverseLerp(
            earlyHour,
            lateHour,
            currentHour
        );

        // Diminui o intervalo conforme o jogo avança
        float minInterval = Mathf.Lerp(
            earlyMinInterval,
            lateMinInterval,
            progress
        );

        float maxInterval = Mathf.Lerp(
            earlyMaxInterval,
            lateMaxInterval,
            progress
        );

        return Random.Range(minInterval, maxInterval);
    }
}
