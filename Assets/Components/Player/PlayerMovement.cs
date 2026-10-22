using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Rigidbody2D rb;

    public Vector2 moveInput;
    public float moveSpeed = 7f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        
    }

    public void Move(InputAction.CallbackContext context)
    {
       moveInput = context.ReadValue<Vector2>();
    }
}
