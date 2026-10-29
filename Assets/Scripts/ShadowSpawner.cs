
using UnityEngine;
using System.Collections.Generic;

public class ShadowSpawner : MonoBehaviour
{
    [Header("Referências")]
    public Timer gameTimer;
    public GameObject globalLight;
    public GameObject shadowPrefab;
    public Transform player;
    public PlayerInteractions playerInteractions;

    [Header("Pontos de Spawn")]
    public Transform[] spawnPoints;

    [Header("Spawn")]
    public float checkInterval = 3f;
    public int maxShadows = 3;

    [Header("Chance de Spawn")]
    [Range(0f, 1f)]
    public float earlySpawnChance = 0.5f;

    [Range(0f, 1f)]
    public float lateSpawnChance = 0.9f;

    public int earlyHour = 1;
    public int lateHour = 6;

    private float spawnTimer = 0f;

    private List<Shadow> activeShadows = new List<Shadow>();

    void Update()
    {
        if (gameTimer == null || globalLight == null)
        {
            return;
        }

        // Quando as luzes estão acesas,
        // todas as sombras desaparecem.
        if (globalLight.activeSelf)
        {
            ClearShadows();
            return;
        }

        // Não cria sombras depois das 07h.
        int currentHour = Mathf.FloorToInt(
            gameTimer.time / gameTimer.secondsPerHour
        );

        if (currentHour >= 7)
        {
            ClearShadows();
            return;
        }

        spawnTimer -= Time.deltaTime;

        if (spawnTimer > 0f)
        {
            return;
        }

        spawnTimer = checkInterval;

        // Remove referências de sombras que já foram destruídas.
        activeShadows.RemoveAll(shadow => shadow == null);

        // Limita a quantidade de sombras simultâneas.
        if (activeShadows.Count >= maxShadows)
        {
            return;
        }

        // Calcula a chance de spawn baseada no horário.
        float progress = Mathf.InverseLerp(
            earlyHour,
            lateHour,
            currentHour
        );

        float spawnChance = Mathf.Lerp(
            earlySpawnChance,
            lateSpawnChance,
            progress
        );

        // Sorteia se uma sombra vai aparecer.
        if (Random.value <= spawnChance)
        {
            SpawnShadow();
        }
    }

    void SpawnShadow()
    {
        if (shadowPrefab == null || spawnPoints.Length == 0)
        {
            return;
        }

        // Escolhe um ponto de spawn aleatório.
        Transform spawnPoint = spawnPoints[
            Random.Range(0, spawnPoints.Length)
        ];

        GameObject shadowObject = Instantiate(
            shadowPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        Shadow shadow = shadowObject.GetComponent<Shadow>();

        if (shadow != null)
        {
            shadow.player = player;
            shadow.playerInteractions = playerInteractions;
            shadow.globalLight = globalLight;

            activeShadows.Add(shadow);
        }
    }

    void ClearShadows()
    {
        foreach (Shadow shadow in activeShadows)
        {
            if (shadow != null)
            {
                Destroy(shadow.gameObject);
            }
        }

        activeShadows.Clear();
    }
}
