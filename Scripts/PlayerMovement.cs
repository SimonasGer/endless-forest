using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public InputAction inputAction;
    public float moveSpeed = 5f;
    public Transform cameraObject;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void RotatePlayer()
    {
        Vector2 mouseAt = Mouse.current.position.ReadValue();
        Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseAt);
        Vector2 direction = mouse - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }


    void Update()
    {
        cameraObject.position = new Vector3(transform.position.x, transform.position.y, -10);
        moveInput = inputAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        RotatePlayer();
        Vector2 input = moveInput.normalized;

        if (input != Vector2.zero)
            rb.linearVelocity = input * moveSpeed;
        else
            rb.linearVelocity = Vector2.zero;
        }
}