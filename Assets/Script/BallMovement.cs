using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour 
{
    private Rigidbody2D rb; // Componente Rigidbody2D de la pelota

    [SerializeField]
    private float initialForce = 300f; // Fuerza inicial aplicada a la pelota
    private float speedMultiplier = 1f; // Multiplicador de velocidad

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchBall();
    }

    void FixedUpdate()
    {
        // Si la velocidad es muy baja, relanza la pelota
        if (rb.velocity.magnitude < 0.1f)
        {
            speedMultiplier = 1f; // Reinicia el multiplicador al relanzar
            LaunchBall();
        }
    }

    // Lanza la pelota en una dirección aleatoria
    private void LaunchBall()
    {
        rb.velocity = Vector2.zero;
        float angleDeg = Random.Range(0f, 360f);
        Vector2 forceDirection = Quaternion.Euler(0, 0, angleDeg) * Vector2.right;
        rb.AddForce(forceDirection * initialForce * speedMultiplier);
    }

    // Aumenta de la velocidad del paddle al colisionar con este
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            speedMultiplier += 0.5f;
            // Normalizacion de la direccion y ajuste de la velocidad   
            rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude + 0.5f);
        }
    }
}