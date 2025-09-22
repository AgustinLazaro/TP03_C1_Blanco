//librerias importadas
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

/* 
 * todo este documento me sirve a modo de guia y anotaciones para entender el proyecto
 * =================================================================================================
 * ANOTACIONES Y GUÍA DEL PROYECTO - PONG CON MEJORAS
 * =================================================================================================
 * 
 * Este documento sirve como una guía central para entender la estructura y el funcionamiento
 * de los diferentes scripts que componen este proyecto de Pong.
 *
 *
 * ESTRUCTURA GENERAL DE SCRIPTS
 * -------------------------------------------------------------------------------------------------
 * 
 * 1. GameManager.cs:
 *    - Propósito: Orquesta el estado general del juego (inicio, puntuación, fin de la partida).
 *    - Funcionalidades clave:
 *      - `UpdateScore()`: Actualiza el marcador y comprueba si un jugador ha ganado.
 *      - `ResetGame()`: Reinicia la posición de la bola y los jugadores.
 *      - `EndGame()`: Muestra el panel de fin de partida y detiene el juego.
 * 
 * 2. BallMovement.cs:
 *    - Propósito: Controla el movimiento y la física de la bola.
 *    - Funcionalidades clave:
 *      - `StartMoving()`: Inicia el movimiento de la bola en una dirección aleatoria.
 *      - `OnCollisionEnter2D()`: Gestiona los rebotes con los paddles y los muros. Aumenta
 *        la velocidad de la bola con cada golpe para incrementar la dificultad.
 * 
 * 3. PaddleMovement.cs:
 *    - Propósito: Gestiona el movimiento de los paddles de los jugadores.
 *    - Funcionalidades clave:
 *      - `HandleMovementWithForce()`: Aplica fuerzas al Rigidbody2D para un movimiento suave
 *        y basado en físicas.
 *      - `SetColor()`, `SetHeight()`, `SetSpeed()`: Métodos públicos para que otros scripts
 *        (como UIMenu o los Power-Ups) puedan modificar las propiedades del paddle.
 * 
 * 4. GoalZone.cs:
 *    - Propósito: Detecta cuándo la bola entra en la zona de gol de un jugador.
 *    - Funcionalidades clave:
 *      - `OnTriggerEnter2D()`: Llama al `GameManager` para que actualice la puntuación y
 *        reinicie la ronda.
 * 
 * 5. UIMenu.cs:
 *    - Propósito: Controla el menú de pausa, el panel de opciones y el de créditos.
 *    - Funcionalidades clave:
 *      - `TogglePause()`: Pausa y reanuda el juego, mostrando u ocultando el menú.
 *      - `ShowOptionsPanel()`: Muestra el panel de opciones.
 *      - `ShowCreditsPanel()`: Muestra el panel de créditos.
 *      - `ExitToMainMenu()`: Carga la escena del menú principal.
 *      - `ChangePaddlesColor()`, `ChangePaddlesHeight()`, `ChangePaddlesSpeed()`: Modifican
 *        las propiedades de los paddles según los valores seleccionados en el menú de opciones.
 * 
 * 6. PowerUpSpawnSystem.cs y ObstacleSpawnSystem.cs:
 *    - Propósito: Gestionan la aparición aleatoria de Power-Ups y obstáculos en el campo.
 *    - Funcionalidades clave:
 *      - `StartSpawning()`: Inicia una corrutina que instancia prefabs cada cierto intervalo
 *        de tiempo en posiciones aleatorias.
 * 
 * 7. Scripts de Power-Ups (HeightPowerUp.cs, etc.):
 *    - Propósito: Definen el comportamiento de un Power-Up específico.
 *    - Funcionalidades clave:
 *      - `OnTriggerEnter2D()`: Cuando un paddle toca el Power-Up, aplica el efecto
 *        correspondiente (ej. aumentar altura) y luego se autodestruye.
 * 
 * -------------------------------------------------------------------------------------------------
 * NUEVAS IMPLEMENTACIONES Y CAMBIOS RECIENTES
 * -------------------------------------------------------------------------------------------------
 * 
 * - Panel de Créditos (UIMenu.cs):
 *   - Se ha añadido un `GameObject` llamado `panelCredits` para mostrar los créditos del juego.
 *   - Se ha creado la función `ShowCreditsPanel()` que se activa con el `buttonCredits`.
 *   - Esta función oculta el panel de pausa y muestra el de créditos.
 *   - Se ha actualizado la lógica de `TogglePause()`, `Start()` y `ShowPausePanel()` para
 *     gestionar correctamente la visibilidad del nuevo panel, asegurando que no se solape
 *     con otros menús.
 * 
 * 
 * =================================================================================================
 */

// --- ANOTACIONES DEL PROYECTO ---

// Este script no hace nada en el juego, solo sirve como un bloc de notas para estudiar.
public class Anotaciones : MonoBehaviour
{
    // =================================================================================
    // 1. CONCEPTOS BÁSICOS DE UN SCRIPT EN UNITY
    // =================================================================================

    // 'using UnityEngine;'
    // Esto es como decirle a nuestro script: "Vamos a usar herramientas de la caja de Unity".
    // Nos da acceso a funciones como 'GameObject', 'Transform', 'Vector2', 'Debug.Log', etc.

    //using System.Collections; 
    // Nos permite usar colecciones como 'IEnumerator' para corrutinas.

    //using System.Collections.Generic;
    // Nos permite usar listas genéricas como 'List<T>'.

    //using UnityEngine.UI;
    // Nos permite usar componentes de UI como 'Button', 'Text', 'Image', etc.

    //using TMPro;
    // Nos permite usar TextMeshPro, una herramienta avanzada para manejar texto en Unity.

    // 'public class Anotaciones : MonoBehaviour'
    // - 'public class Anotaciones': Define una nueva "plantilla" o "molde" para un componente llamado 'Anotaciones'.
    // - ': MonoBehaviour': Significa que nuestra clase "hereda" todas las capacidades de un componente de Unity.
    //   Gracias a esto, podemos añadir el script a un objeto en la escena y usar funciones como Awake, Start y Update.


    // =================================================================================
    // 2. VARIABLES Y TIPOS DE DATOS
    // =================================================================================

    // Una variable es como una caja donde guardamos información.

    // --- Variables que se configuran desde el Inspector de Unity ---

    // '[SerializeField]'
    // Este "atributo" hace que una variable privada (que normalmente está oculta) aparezca en el Inspector de Unity.
    // Es una buena práctica porque mantiene la variable protegida de otros scripts, pero nos deja ajustarla fácilmente.
    [SerializeField] private float ejemploDeSerializeField;

    // '[Header("Texto")]'
    // Crea un título en el Inspector. Sirve para organizar las variables y que todo sea más fácil de leer.
    [Header("Ejemplo de Título en el Inspector")]
    [SerializeField] private int otroEjemplo;


    // --- Tipos de Variables Comunes Usadas en el Proyecto ---

    // 'GameObject': Representa cualquier objeto en la escena de Unity (un jugador, un obstáculo, una luz, etc.).
    private GameObject ejemploGameObject;

    // 'Rigidbody2D': El componente de físicas para objetos 2D. Lo usamos para mover cosas con fuerzas (AddForce).
    private Rigidbody2D ejemploRigidbody;

    // 'SpriteRenderer': El componente que dibuja la imagen (sprite) de un objeto 2D. Lo usamos para cambiar el color.
    private SpriteRenderer ejemploSpriteRenderer;

    // 'Collider2D': Define la forma física de un objeto para detectar colisiones o triggers.
    private Collider2D ejemploCollider;

    // 'float': Un número con decimales. Se usa para la velocidad, el tiempo, la fuerza, etc.
    private float velocidad = 10.5f;

    // 'int': Un número entero, sin decimales. Se usa para contar puntos, vidas, o para el índice de un dropdown.
    private int puntuacion = 100;

    // 'bool': Una variable que solo puede ser verdadera ('true') o falsa ('false'). Perfecta para saber si el juego está en pausa.
    private bool estaEnPausa = false;

    // 'Vector2': Representa una dirección o una posición en un espacio 2D. Tiene dos valores: X e Y.
    private Vector2 posicion = new Vector2(0, 0);

    // 'List<GameObject>': Una lista o colección de objetos. Es como un array pero más flexible.
    private List<GameObject> listaDeObjetos = new List<GameObject>();


    // =================================================================================
    // 3. FUNCIONES PRINCIPALES DE UNITY (CICLO DE VIDA)
    // =================================================================================

    // 'Awake()': Se llama una sola vez, justo cuando el objeto se crea. Es lo primero que se ejecuta.
    private void Awake() { }

    // 'Start()': Se llama una sola vez, después de que todos los 'Awake' se hayan ejecutado.
    private void Start() { }

    // 'Update()': Se llama en cada fotograma (frame) del juego.
    private void Update() { }

    // 'FixedUpdate()': Se llama a un ritmo fijo. Es el lugar para la física.
    private void FixedUpdate() { }

    // 'OnTriggerEnter2D(Collider2D other)': Se llama cuando algo entra en un collider marcado como "Is Trigger".
    private void OnTriggerEnter2D(Collider2D other) { }

    // 'OnCollisionEnter2D(Collision2D collision)': Se llama cuando dos colliders chocan físicamente.
    private void OnCollisionEnter2D(Collision2D collision) { }


    // =================================================================================
    // 4. CONCEPTOS AVANZADOS USADOS EN EL PROYECTO
    // =================================================================================

    // --- Corrutinas (Coroutines) ---

    // Una corrutina es como una función que puede hacer pausas.
    // Son perfectas para secuencias de tiempo, como "espera 3 segundos y luego haz algo".
    // Se declaran con 'IEnumerator' y usan 'yield return' para pausar.

    // 'IEnumerator MiRutina()'
    // Así se declara una corrutina.
    private IEnumerator EjemploDeCorrutina()
    {
        // 'yield return new WaitForSeconds(3f);'
        // Esta línea pausa la ejecución de la corrutina durante 3 segundos.
        yield return new WaitForSeconds(3f);

        // Después de la pausa, el código continúa desde aquí.
        Debug.Log("Han pasado 3 segundos.");

    }

    // 'StartCoroutine(MiRutina());'
    // Así es como se inicia una corrutina. No se puede llamar como una función normal.


    // --- Object Pooling (Patrón de Diseño) ---

    // En lugar de crear ('Instantiate') y destruir ('Destroy') objetos todo el tiempo (lo cual es lento y costoso para el juego),
    // el Object Pool consiste en:
    // 1. Crear una "piscina" de objetos al principio del juego y dejarlos desactivados.
    // 2. Cuando necesitamos un objeto, lo cogemos de la piscina y lo activamos.
    // 3. Cuando ya no lo necesitamos, en lugar de destruirlo, lo desactivamos y lo devolvemos a la piscina.
    // Es como tener una caja de piezas de Lego reutilizables. ¡Mucho más eficiente!
    // Lo usamos en 'ObstacleSpawnSystem' y 'PowerUpSpawnSystem'.

    // --- Patrón Singleton ---

    // El Singleton es un patrón de diseño que garantiza que solo exista UNA instancia de una clase en todo el juego.
    // Es perfecto para clases "manager" como GameManager, que necesitan ser accesibles desde cualquier otro script
    // sin necesidad de buscarlo o pasarlo como referencia.
    //
    // public static GameManager instance;
    //
    // void Awake() {
    //   if (instance == null) {
    //     instance = this;
    //   } else {
    //     Destroy(gameObject);
    //   }
    // }
    // Este código en el Awake() asegura que solo haya un GameManager. Si se intenta crear otro, se destruye.

    // --- Eventos de UI y Listeners ---

    // En Unity, la UI (Interfaz de Usuario) funciona con eventos. Por ejemplo, un botón tiene un evento `onClick`.
    // Podemos "escuchar" (listen) estos eventos para ejecutar una función cuando ocurran.
    //
    // button.onClick.AddListener(MiFuncion);
    //
    // Esto le dice al botón: "Cuando alguien haga clic en ti, llama a MiFuncion".
    // Lo usamos en UIMenu.cs para conectar los botones (Resume, Options, etc.) y los controles (sliders, dropdowns)
    // a sus respectivas funciones en el código. Es una forma limpia y desacoplada de manejar la interacción del usuario.

    // --- Gestión de Escenas (Scene Management) ---

    // Unity permite dividir el juego en diferentes "escenas" (por ejemplo, Menú Principal, Nivel 1, Game Over).
    // Para cambiar de una escena a otra, usamos el SceneManager.
    //
    // using UnityEngine.SceneManagement; // ¡No olvides importar la librería!
    //
    // SceneManager.LoadScene("NombreDeLaEscena");
    //
    // Esta línea carga la escena que le indiquemos por su nombre. En UIMenu.cs, lo usamos para volver al menú principal.
    // Es importante recordar que las escenas deben estar añadidas en las Build Settings del proyecto para que funcione.

    // --- Time.timeScale ---

    // Es una propiedad estática que controla la velocidad a la que pasa el tiempo en el juego.
    // - `Time.timeScale = 1f;`: El tiempo transcurre a velocidad normal.
    // - `Time.timeScale = 0f;`: El tiempo se detiene por completo. Esto congela todas las animaciones y físicas.
    // - `Time.timeScale = 0.5f;`: El juego iría a cámara lenta (mitad de velocidad).
    //
    // Es la herramienta que usamos en `UIMenu.cs` para implementar la pausa. Al poner `timeScale` a 0, el juego se congela,
    // y al volver a ponerlo a 1, todo continúa como si nada.

}
