
using UnityEngine;
using UnityEngine.InputSystem;
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
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Max(cooldownTimer, 0f);
        }

        // Se o jogador estiver próximo
        if (playerInRange)
        {
            if (cooldownTimer <= 0f)
            {
                if (interactionText != null)
                {
                    interactionText.text =
                        "Pressione E / A para interagir";
                }

                // Interação com teclado ou controle Xbox
                if (InteractionPressed())
                {
                    ToggleLights();
                }
            }
            else
            {
                if (interactionText != null)
                {
                    interactionText.text =
                        "Aguarde " +
                        Mathf.CeilToInt(cooldownTimer) + "s";
                }
            }
        }
    }

    bool InteractionPressed()
    {
        bool keyboardPressed =
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame;

        bool gamepadPressed =
            Gamepad.current != null &&
            Gamepad.current.buttonSouth.wasPressedThisFrame;

        return keyboardPressed || gamepadPressed;
    }

    void ToggleLights()
    {
        if (globalLight == null)
        {
            Debug.LogWarning(
                "A iluminação global não foi atribuída!"
            );

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
