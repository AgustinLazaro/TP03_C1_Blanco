using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script se encarga de hacer aparecer obstáculos.
public class ObstacleSpawnSystem : MonoBehaviour
{
    // --- Variables del Spawner ---
    [Header("Configuración de Obstáculos")]
    [SerializeField] private GameObject obstaclePrefab; // El objeto que queremos que aparezca.
    [SerializeField] private int poolSize = 10; // Cuántos obstáculos creamos de antemano.

    [Header("Área de Aparición")]
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(10, 8); // El tamaño del rectángulo donde pueden aparecer.

    [Header("Tiempos")]
    [SerializeField] private float minSpawnInterval = 4f; // El tiempo mínimo que esperamos para crear uno nuevo.
    [SerializeField] private float maxSpawnInterval = 8f; // El tiempo máximo.

    // --- El Object Pool ---
    // Una lista para guardar todos los obstáculos que hemos creado.
    private List<GameObject> obstaclePool;

    // Awake es para preparar todo.
    private void Awake()
    {
        // Creamos la "piscina" de obstáculos.
        InitializePool();
    }

    // Start es para empezar la acción.
    private void Start()
    {
        // Empezamos la rutina que crea obstáculos sin parar.
        StartCoroutine(SpawnRoutine());
    }

    // Prepara la lista de obstáculos al principio del juego.
    private void InitializePool()
    {
        obstaclePool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, transform);
            obstacle.SetActive(false); // Lo creamos, pero lo dejamos desactivado.
            obstaclePool.Add(obstacle);
        }
    }

    // Busca en la lista un obstáculo que no esté activo.
    private GameObject GetPooledObject()
    {
        foreach (GameObject obstacle in obstaclePool)
        {
            if (!obstacle.activeInHierarchy)
            {
                return obstacle; // ¡Encontramos uno! Lo devolvemos.
            }
        }
        return null; // No hay ninguno disponible.
    }

    // Una Corrutina es como una función que puede hacer pausas.
    private IEnumerator SpawnRoutine()
    {
        // Este bucle se ejecutará para siempre.
        while (true)
        {
            // 1. Esperamos un tiempo aleatorio.
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            // 2. Buscamos un obstáculo disponible en nuestra "piscina".
            GameObject obstacle = GetPooledObject();

            // 3. Si encontramos uno...
            if (obstacle != null)
            {
                // ...lo colocamos en una posición aleatoria y lo activamos.
                float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
                float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
                obstacle.transform.position = new Vector2(randomX, randomY);
                obstacle.SetActive(true);

                // 4. Le decimos que se destruya (desactive) después de un tiempo.
                float lifetime = Random.Range(3f, 7f);
                StartCoroutine(ReturnToPool(obstacle, lifetime));
            }
        }
    }

    // Esta corrutina espera un tiempo y luego desactiva el objeto.
    private IEnumerator ReturnToPool(GameObject obstacle, float delay)
    {
        yield return new WaitForSeconds(delay);
        obstacle.SetActive(false); // Lo desactivamos para poder reutilizarlo.
    }
}