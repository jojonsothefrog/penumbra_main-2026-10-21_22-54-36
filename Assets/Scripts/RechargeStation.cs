
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RechargeStation : MonoBehaviour
{
    [Header("Interface")]
    public TMP_Text interactionText;

    [Header("Cooldown")]
    public float rechargeCooldown = 10f;

    private PlayerInteractions player;
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
        // Atualiza o cooldown
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Max(cooldownTimer, 0f);
        }

        if (playerInRange && player != null)
        {
            // Atualiza o texto de interação
            if (interactionText != null)
            {
                if (cooldownTimer > 0f)
                {
                    interactionText.text =
                        "Recarga disponível em " +
                        Mathf.CeilToInt(cooldownTimer) + "s";
                }
                else
                {
                    interactionText.text =
                        "Pressione E para recarregar";
                }
            }

            // Interação com E
            if (Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                Recharge();
            }
        }
    }

    void Recharge()
    {
        // Impede recargas durante o cooldown
        if (cooldownTimer > 0f)
        {
            return;
        }

        if (player.currentBattery >= player.maxBattery)
        {
            return;
        }

        // Recarrega a bateria
        player.RechargeBattery();

        // Inicia o cooldown
        cooldownTimer = rechargeCooldown;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerInteractions>();
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
            player = null;

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
}
