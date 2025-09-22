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

    // Uso OnTriggerEnter2D para que el power-up se active cuando algo lo atraviesa.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprueba si el objeto que colisiona es un jugador.
        if (other.CompareTag("Player"))
        {
            PaddleMovement paddle = other.GetComponent<PaddleMovement>();
            if (paddle != null)
            {
                StartCoroutine(ApplyEffect(paddle));
            }
        }
    }

    // Esta corrutina aplica el efecto de tamaño.
    private IEnumerator ApplyEffect(PaddleMovement paddle)
    {
        // 1. Hago invisible el power-up para que no se pueda coger dos veces.
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // 2. Guardo la altura original del paddle.
        float originalHeight = paddle.transform.localScale.y;
        // Aplico la nueva altura.
        paddle.SetHeight(originalHeight * _heightMultiplier);

        // 3. Espero a que pase el tiempo del efecto.
        yield return new WaitForSeconds(_duration);

        // 4. Devuelvo el paddle a su altura original.
        paddle.SetHeight(originalHeight);

        // 5. Desactivo el objeto del power-up para que vuelva a la "piscina".
        gameObject.SetActive(false);
    }

    // Esta función se llama cada vez que el objeto se activa.
    // La uso para asegurarme de que el power-up sea visible de nuevo cuando se reutilice.
    private void OnEnable()
    {
        // Al reutilizarlo, lo vuelvo a hacer visible y sólido.
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}