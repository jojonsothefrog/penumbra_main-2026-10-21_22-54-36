
using UnityEngine;
using TMPro;

public class SanitySystem : MonoBehaviour
{
    [Header("Sanidade")]
    public float maxSanity = 100f;
    public float currentSanity = 100f;

    [Header("Balanceamento")]
    public float sanityLossPerSecond = 5f;
    public float sanityRecoveryPerSecond = 1f;

    [Header("Iluminação")]
    public GameObject globalLight;

    [Header("Interface")]
    public TMP_Text sanityText;
    public GameObject loseUI;

    private bool hasLost = false;

    void Start()
    {
        currentSanity = maxSanity;

        if (loseUI != null)
        {
            loseUI.SetActive(false);
        }

        UpdateSanityUI();
    }

    void Update()
    {
        if (hasLost)
        {
            return;
        }

        if (globalLight == null)
        {
            return;
        }

        // Perde sanidade enquanto as luzes estão apagadas
        if (!globalLight.activeSelf)
        {
            currentSanity -= sanityLossPerSecond * Time.deltaTime;
        }
        // Recupera sanidade enquanto as luzes estão acesas
        else
        {
            currentSanity += sanityRecoveryPerSecond * Time.deltaTime;
        }

        // Mantém a sanidade entre 0 e o máximo
        currentSanity = Mathf.Clamp(
            currentSanity,
            0f,
            maxSanity
        );

        UpdateSanityUI();

        // Verifica se o jogador perdeu
        if (currentSanity <= 0f)
        {
            GameOver();
        }
    }

    void UpdateSanityUI()
    {
        if (sanityText != null)
        {
            sanityText.text =
                "Sanidade: " +
                Mathf.CeilToInt(currentSanity) + "%";
        }
    }

    public void TakeDamage(float amount)
    {
        if (hasLost)
        {
            return;
        }

        currentSanity -= amount;

        currentSanity = Mathf.Clamp(
            currentSanity,
            0f,
            maxSanity
        );

        UpdateSanityUI();

        if (currentSanity <= 0f)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        if (hasLost)
        {
            return;
        }

        hasLost = true;

        Debug.Log("O jogador perdeu toda a sanidade.");

        Time.timeScale = 0f;

        if (loseUI != null)
        {
            loseUI.SetActive(true);
        }
    }
}
