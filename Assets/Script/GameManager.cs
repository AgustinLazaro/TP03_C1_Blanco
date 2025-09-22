using UnityEngine;
using TMPro; // Necesario para el texto de la UI.

public class GameManager : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameSettings settings; // Arrastra aquí tu asset de GameSettings.

    [Header("Referencias de Escena")]
    [SerializeField] private BallMovement ball;
    [SerializeField] private TextMeshProUGUI scoreText; // Texto para la puntuación (Ej: "0 - 0").
    [SerializeField] private TextMeshProUGUI timerText; // Texto para el temporizador.
    [SerializeField] private GameObject gameOverPanel; // Panel que se muestra al final.
    [SerializeField] private TextMeshProUGUI winText; // Texto que dice quién ganó.

    // --- Variables de Estado ---
    private int scorePlayer1 = 0;
    private int scorePlayer2 = 0;
    private float currentTimer;
    private bool isGameOver = false;

    private void Start()
    {
        // Ocultamos el panel de fin de juego y reiniciamos todo.
        gameOverPanel.SetActive(false);
        currentTimer = settings.shotClockDuration;
        UpdateScoreUI();
    }

    private void Update()
    {
        if (isGameOver) return; // Si el juego ha terminado, no hacemos nada.

        HandleShotClock();
    }

    // Gestiona el reloj de posesión.
    private void HandleShotClock()
    {
        currentTimer -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(currentTimer).ToString();

        if (currentTimer <= 0)
        {
            // Si se acaba el tiempo, vemos de qué lado está la pelota.
            if (ball.transform.position.x < 0)
            {
                // La pelota está del lado del Jugador 1, así que el Jugador 2 anota.
                PlayerScores(2);
            }
            else
            {
                // La pelota está del lado del Jugador 2, así que el Jugador 1 anota.
                PlayerScores(1);
            }
        }
    }

    // Esta función se llama desde fuera (por ejemplo, desde las porterías) cuando un jugador anota.
    public void PlayerScores(int playerNumber)
    {
        if (isGameOver) return;

        if (playerNumber == 1)
        {
            scorePlayer1++;
        }
        else
        {
            scorePlayer2++;
        }

        UpdateScoreUI();
        CheckForWinner();

        // Si nadie ha ganado todavía, reiniciamos la pelota y el temporizador.
        if (!isGameOver)
        {
            ball.LaunchBall(); // Necesitamos hacer público este método en BallMovement.
            currentTimer = settings.shotClockDuration;
        }
    }

    // Actualiza el texto de la puntuación en la pantalla.
    private void UpdateScoreUI()
    {
        scoreText.text = $"{scorePlayer1} - {scorePlayer2}";
    }

    // Comprueba si algún jugador ha llegado a los puntos para ganar.
    private void CheckForWinner()
    {
        if (scorePlayer1 >= settings.pointsToWin)
        {
            EndGame("Jugador 1 Gana");
        }
        else if (scorePlayer2 >= settings.pointsToWin)
        {
            EndGame("Jugador 2 Gana");
        }
    }

    // Termina el juego.
    private void EndGame(string winnerMessage)
    {
        isGameOver = true;
        winText.text = winnerMessage;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pausamos el juego.
    }
}