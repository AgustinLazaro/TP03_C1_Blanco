using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script hace aparecer los Power-Ups.
public class PowerUpSpawnSystem : MonoBehaviour
{
    // --- Variables del Spawner ---
    [Header("Configuración de Power-Ups")]
    [SerializeField] private GameObject powerUpPrefab; // El objeto Power-Up que queremos que aparezca.
    [SerializeField] private int poolSize = 5; // Cuántos creamos de antemano.

    [Header("Área de Aparición")]
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(16, 8);

    [Header("Tiempos")]
    [SerializeField] private float minSpawnInterval = 10f;
    [SerializeField] private float maxSpawnInterval = 20f;
    [SerializeField] private float lifetime = 10f; // Cuánto tiempo dura el power-up en pantalla.

    // --- El Object Pool ---
    private List<GameObject> powerUpPool;

    private void Awake()
    {
        InitializePool();
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Prepara la lista de power-ups.
    private void InitializePool()
    {
        powerUpPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject powerUp = Instantiate(powerUpPrefab, transform);
            powerUp.SetActive(false);
            powerUpPool.Add(powerUp);
        }
    }

    // Busca un power-up que no esté activo.
    private GameObject GetPooledObject()
    {
        foreach (GameObject powerUp in powerUpPool)
        {
            if (!powerUp.activeInHierarchy)
            {
                return powerUp;
            }
        }
        return null;
    }

    // La rutina que los hace aparecer.
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            GameObject powerUp = GetPooledObject();
            if (powerUp != null)
            {
                float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
                float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
                powerUp.transform.position = new Vector2(randomX, randomY);
                powerUp.SetActive(true);

                // En lugar de destruirlo, lo desactivamos después de un tiempo.
                StartCoroutine(ReturnToPool(powerUp, lifetime));
            }
        }
    }

    // Desactiva el power-up para que pueda ser reutilizado.
    private IEnumerator ReturnToPool(GameObject powerUp, float delay)
    {
        yield return new WaitForSeconds(delay);
        powerUp.SetActive(false);
    }
}
