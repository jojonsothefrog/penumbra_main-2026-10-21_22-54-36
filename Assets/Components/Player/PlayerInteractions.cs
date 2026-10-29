
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    public Rigidbody2D rb;

    public Light2D Flash;
    public Camera mainCamera;

    public Vector2 moveInput;
    public Vector2 mousePosition;
    public Vector2 gamepadPosition;

    public float moveSpeed = 7f;

    public bool flashOn = false;

    public bool IsFlashlightOn
    {
        get { return flashOn && Flash != null && Flash.enabled; }
    }

    [Header("Bateria da Lanterna")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float drainPerSecond = 5f;

    [Header("Interface")]
    public TMP_Text batteryText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Flash = transform.GetChild(0).GetComponent<Light2D>();

        mainCamera = Camera.main;

        currentBattery = maxBattery;
        flashOn = false;

        UpdateBatteryText();
    }

    void Update()
    {
        // Movimentação
        rb.linearVelocity = moveInput * moveSpeed;

        // Sistema de bateria
        if (flashOn)
        {
            currentBattery -= drainPerSecond * Time.deltaTime;

            currentBattery = Mathf.Clamp(
                currentBattery,
                0f,
                maxBattery
            );

            // Desliga a lanterna quando a bateria acaba
            if (currentBattery <= 0f)
            {
                flashOn = false;
            }
        }

        // Sistema da lanterna
        if (flashOn)
        {
            Flash.enabled = true;

            // Rotação usando o gamepad
            if (gamepadPosition.magnitude > 0.2f)
            {
                float angle = Mathf.Atan2(
                    gamepadPosition.y,
                    gamepadPosition.x
                ) * Mathf.Rad2Deg;

                Flash.transform.rotation = Quaternion.Euler(
                    new Vector3(0f, 0f, angle - 90)
                );
            }
            else
            {
                // Rotação usando o mouse
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(
                    new Vector3(mousePosition.x, mousePosition.y, 0f)
                );

                Vector3 rotateDirection =
                    (worldPosition - Flash.transform.position).normalized;

                rotateDirection.z = 0;

                float angle = Mathf.Atan2(
                    rotateDirection.y,
                    rotateDirection.x
                ) * Mathf.Rad2Deg;

                Flash.transform.rotation = Quaternion.Euler(
                    new Vector3(0f, 0f, angle - 90)
                );
            }
        }
        else
        {
            Flash.enabled = false;
        }

        UpdateBatteryText();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Flashlight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Não permite ligar a lanterna sem bateria
            if (!flashOn && currentBattery <= 0f)
            {
                return;
            }

            flashOn = !flashOn;
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (flashOn)
        {
            mousePosition = context.ReadValue<Vector2>();
        }
    }

    public void LookGamepad(InputAction.CallbackContext context)
    {
        if (flashOn)
        {
            gamepadPosition = context.ReadValue<Vector2>();
        }
    }

    // Chamado pela estação de recarga
    public void RechargeBattery()
    {
        currentBattery = maxBattery;

        UpdateBatteryText();
    }

    void UpdateBatteryText()
    {
        if (batteryText != null)
        {
            batteryText.text =
                "Bateria: " + Mathf.CeilToInt(currentBattery) + "%";
        }
    }
}
