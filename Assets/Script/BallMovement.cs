using UnityEngine;

/// <summary>
/// Controla el movimiento, lanzamiento y la velocidad de la pelota.
/// </summary>
public class BallMovement : MonoBehaviour
{
    // --- Variables de la Pelota ---
    [Header("Fuerza y Velocidad")]
    [SerializeField] private float initialForce = 500f; // La fuerza con la que se lanza la pelota al principio.
    [SerializeField] private float speedIncreaseOverTime = 0.5f; // Cuánto más rápida se vuelve cada segundo.
    [SerializeField] private float maxSpeed = 25f; // La velocidad máxima que puede alcanzar.

    // --- Referencias ---
    private Rigidbody2D rb; // El componente de físicas de la pelota.
    
    // Una propiedad pública para que otros scripts puedan saber quién tocó la pelota.
    // "public" para que se pueda leer desde fuera.
    // "private set" para que solo este script pueda cambiar su valor.
    public PaddleMovement LastTouchedBy { get; private set; }

    // Awake se usa para obtener los componentes.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start se usa para iniciar la lógica del juego.
    private void Start()
    {
        // Esperamos 1 segundo antes de lanzar la pelota por primera vez.
        Invoke(nameof(LaunchBall), 1f);
    }

    // FixedUpdate es para la física.
    private void FixedUpdate()
    {
        // Si la pelota está en movimiento y no ha llegado a su velocidad máxima...
        if (rb.velocity.magnitude > 0 && rb.velocity.magnitude < maxSpeed)
        {
            // ...la aceleramos un poquito.
            rb.velocity += rb.velocity.normalized * speedIncreaseOverTime * Time.fixedDeltaTime;
        }
    }

    // Esta función se llama cuando la pelota choca con algo.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si chocamos con un objeto que tiene el tag "Player"...
        if (collision.gameObject.CompareTag("Player"))
        {
            // ...guardamos la referencia de ese jugador.
            LastTouchedBy = collision.gameObject.GetComponent<PaddleMovement>();
        }
    }

    // Hacemos esta función pública para que el GameManager pueda llamarla.
    public void LaunchBall()
    {
        transform.position = Vector2.zero;
        rb.velocity = Vector2.zero;
        LastTouchedBy = null;

        float angle = Random.Range(0, 4) * 90f + Random.Range(30f, 60f);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        
        rb.AddForce(direction * initialForce);
    }
}