using UnityEngine;

public class GoalZone : MonoBehaviour
{
    [Header("Configuración de la Portería")]
    [SerializeField] private int scoringPlayerNumber; // Indica qué jugador anota si la pelota entra aquí (1 o 2).

    // Hacemos una referencia estática al GameManager para encontrarlo fácilmente.
    private static GameManager gameManager;

    private void Awake()
    {
        // Si aún no hemos encontrado el GameManager, lo buscamos en la escena.
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que entra es la pelota...
        if (other.CompareTag("Ball"))
        {
            // ...le decimos al GameManager que el jugador correspondiente ha anotado.
            gameManager.PlayerScores(scoringPlayerNumber);
        }
    }
}