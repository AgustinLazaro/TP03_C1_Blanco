using UnityEngine;

// [CreateAssetMenu]  permite crear instancias de este objeto desde el menú de Unity.
[CreateAssetMenu(fileName = "GameSettings", menuName = "Pong/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Reglas del Juego")]
    public int pointsToWin = 3; // Puntos necesarios para ganar (Mejor de 5).

    [Header("Temporizador")]
    public float shotClockDuration = 20f; // Duración del reloj de posesión.
}