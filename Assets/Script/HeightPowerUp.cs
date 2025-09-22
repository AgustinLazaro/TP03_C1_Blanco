using System.Collections;
using UnityEngine;

/// <summary>
/// Gestiona el efecto de un power-up que aumenta el tamaño del paddle temporalmente.
/// </summary>
// Este script va en el objeto del Power-Up.
// [RequireComponent] asegura que este objeto siempre tenga un Collider2D.
[RequireComponent(typeof(Collider2D))]
public class HeightPowerUp : MonoBehaviour
{
    // --- Variables del Power-Up ---
    [Header("Configuración del Power-Up")]
    [SerializeField] private float _heightMultiplier = 1.5f; // Cuánto crece el paddle (1.5 = 50% más grande).
    [SerializeField] private float _duration = 8f; // Cuántos segundos dura el efecto.

    // Ahora usamos OnCollisionEnter2D para que el paddle choque y lo recoja.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprueba si el objeto que colisiona es un jugador.
        if (collision.gameObject.CompareTag("Player"))
        {
            PaddleMovement paddle = collision.gameObject.GetComponent<PaddleMovement>();
            if (paddle != null)
            {
                StartCoroutine(ApplyEffect(paddle));
            }
        }
    }

    // Esta corrutina aplica el efecto de tamaño.
    private IEnumerator ApplyEffect(PaddleMovement paddle)
    {
        // 1. Hacemos invisible el power-up para que no se pueda coger dos veces.
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // 2. Guardamos la altura original del paddle.
        float originalHeight = paddle.transform.localScale.y;
        // Aplicamos la nueva altura.
        paddle.SetHeight(originalHeight * _heightMultiplier);

        // 3. Esperamos a que pase el tiempo del efecto.
        yield return new WaitForSeconds(_duration);

        // 4. Devolvemos el paddle a su altura original.
        paddle.SetHeight(originalHeight);

        // 5. Desactivamos el objeto del power-up para que vuelva a la "piscina".
        gameObject.SetActive(false);
    }

    // Esta función se llama cada vez que el objeto se activa.
    // La usamos para asegurarnos de que el power-up sea visible de nuevo cuando se reutilice.
    private void OnEnable()
    {
        // Al reutilizarlo, lo volvemos a hacer visible y sólido.
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}