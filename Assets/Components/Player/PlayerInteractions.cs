using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityPipeline.Microsoft.CodeAnalysis.CSharp.Syntax;

public class PlayerInteractions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Rigidbody2D rb;

    private Light2D Flash;
    public Camera mainCamera;

    public Vector2 moveInput;

    public Vector2 mousePosition;
    public Vector2 gamepadPosition;
    private float moveSpeed = 7f;


    public bool flashOn = false;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Flash = transform.GetChild(0).GetComponent<Light2D>(); // Armazenando a luz da lanterna que é child do player

        mainCamera = Camera.main; //Atribuindo camera principal ao player
    }

    // Update is called once per frame
    void Update()
    {
        //Movimentação
        rb.linearVelocity = moveInput * moveSpeed;

        // Sistema de ligar e desligar lanterna
        if (flashOn)
        {
            Flash.enabled = true;

            //  Sistema de rotação de lanterna
            if (gamepadPosition.magnitude > 0.2f) 
            {
                float angle = Mathf.Atan2(gamepadPosition.y, gamepadPosition.x) * Mathf.Rad2Deg;

                Flash.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));
            } else
            {
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

                Vector3 rotateDirection = (worldPosition - Flash.transform.position).normalized;
                rotateDirection.z = 0;

                float angle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;


                Flash.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));

            }
        }
        else
        {
            Flash.enabled = false;
        } 
    }

    public void Move(InputAction.CallbackContext context)
    {
       moveInput = context.ReadValue<Vector2>();
    }

    public void Flashlight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
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
}
