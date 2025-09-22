using UnityEngine;

// Controla todo lo que hace el paddle.
public class PaddleMovement : MonoBehaviour
{
    [Header("Fuerza de Movimiento")]
    [SerializeField] private float movementForce = 50f; // La FUERZA que aplicamos.
    [SerializeField] private float horizontalForce = 40f;

    [Header("Velocidad Máxima")]
    [SerializeField] private float maxVerticalSpeed = 10f; // Límite para que no acelere infinitamente.
    [SerializeField] private float maxHorizontalSpeed = 8f;

    [Header("Teclas de Control")]
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode moveDownKey = KeyCode.S;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;

    // --- Referencias ---
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void FixedUpdate()
    {
        HandleMovementWithForce();
    }

    // Movimiento basado en FUERZA, que respeta las colisiones.
    private void HandleMovementWithForce()
    {
        // --- Captura de Input ---
        float verticalInput = 0f;
        if (Input.GetKey(moveUpKey)) verticalInput = 1f;
        else if (Input.GetKey(moveDownKey)) verticalInput = -1f;

        float horizontalInput = 0f;
        if (Input.GetKey(moveRightKey)) horizontalInput = 1f;
        else if (Input.GetKey(moveLeftKey)) horizontalInput = -1f;

        // --- Aplicar Fuerza ---
        // Solo aplicamos fuerza si el jugador está presionando una tecla.
        if (verticalInput != 0)
        {
            rb.AddForce(Vector2.up * verticalInput * movementForce);
        }
        if (horizontalInput != 0)
        {
            rb.AddForce(Vector2.right * horizontalInput * horizontalForce);
        }

        // --- Limitar la Velocidad ---
        // Para evitar que el paddle acelere sin control, le ponemos un límite.
        float clampedY = Mathf.Clamp(rb.velocity.y, -maxVerticalSpeed, maxVerticalSpeed);
        float clampedX = Mathf.Clamp(rb.velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
        rb.velocity = new Vector2(clampedX, clampedY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            spriteRenderer.color = new Color(Random.value, Random.value, Random.value);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            spriteRenderer.color = Color.black;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            spriteRenderer.color = originalColor;
        }
    }

    // --- Funciones Públicas ---
    public void SetColor(Color newColor)
    {
        spriteRenderer.color = newColor;
        originalColor = newColor;
    }
    public void SetHeight(float newHeight)
    {
        transform.localScale = new Vector3(transform.localScale.x, newHeight, transform.localScale.z);
    }
    public void SetSpeed(float newSpeed)
    {
        // Ahora el slider controla la fuerza, no la velocidad directa.
        movementForce = newSpeed;
    }
}



