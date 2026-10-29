
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public PlayerInteractions playerInteractions;
    public SanitySystem playerSanity;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Lights")]
    public GameObject globalLight;

    [Header("Flashlight")]
    public float flashlightAngle = 60f;

    [Header("Sanity Damage")]
    public float sanityDamageOnContact = 5f;
    public float contactDamageCooldown = 1f;

    private float damageTimer = 0f;

    private void Start()
    {
        // Encontra automaticamente o Player
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        // Encontra automaticamente o PlayerInteractions
        if (playerInteractions == null && player != null)
        {
            playerInteractions = player.GetComponent<PlayerInteractions>();
        }

        // Encontra automaticamente o SanitySystem
        if (playerSanity == null)
        {
            playerSanity = FindFirstObjectByType<SanitySystem>();
        }
    }

    private void Update()
    {
        // Atualiza o cooldown do dano
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }

        // Se as luzes globais estiverem acesas, destrói a sombra
        if (globalLight != null && globalLight.activeInHierarchy)
        {
            Destroy(gameObject);
            return;
        }

        // Se não houver player, não faz nada
        if (player == null)
        {
            return;
        }

        // Persegue o player
        Vector2 direction = (player.position - transform.position).normalized;

        transform.position +=
            (Vector3)(direction * moveSpeed * Time.deltaTime);

        // Verifica se a lanterna está iluminando a sombra
        if (playerInteractions != null &&
            playerInteractions.IsFlashlightOn &&
            playerInteractions.Flash != null)
        {
            Vector2 flashlightDirection =
                playerInteractions.Flash.transform.up;

            Vector2 directionToShadow =
                (transform.position - player.position).normalized;

            float angle = Vector2.Angle(
                flashlightDirection,
                directionToShadow
            );

            if (angle <= flashlightAngle / 2f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Verifica se a sombra está encostando no player
        if (other.CompareTag("Player") && damageTimer <= 0f)
        {
            if (playerSanity != null)
            {
                playerSanity.TakeDamage(sanityDamageOnContact);

                damageTimer = contactDamageCooldown;
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning(
                    "Shadow: SanitySystem do Player não foi encontrado!"
                );
            }
        }
    }
}
