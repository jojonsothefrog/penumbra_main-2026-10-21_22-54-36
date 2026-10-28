
using UnityEngine;
using TMPro;

public class LightSwitch : MonoBehaviour
{
    [Header("Iluminação")]
    public GameObject globalLight;

    [Header("Interação")]
    public float interactionCooldown = 5f;
    public TMP_Text interactionText;

    private bool playerInRange = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Atualiza o tempo de espera
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Se o jogador estiver próximo
        if (playerInRange)
        {
            if (cooldownTimer <= 0)
            {
                if (interactionText != null)
                {
                    interactionText.text = "Pressione E para interagir";
                }

                // Interação com a tecla E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ToggleLights();
                }
            }
            else
            {
                if (interactionText != null)
                {
                    interactionText.text =
                        "Aguarde " + Mathf.Ceil(cooldownTimer) + "s";
                }
            }
        }
    }

    void ToggleLights()
    {
        if (globalLight == null)
        {
            Debug.LogWarning("A iluminação global não foi atribuída!");
            return;
        }

        // Alterna o estado da iluminação
        globalLight.SetActive(!globalLight.activeSelf);

        // Inicia o tempo de espera
        cooldownTimer = interactionCooldown;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
}
