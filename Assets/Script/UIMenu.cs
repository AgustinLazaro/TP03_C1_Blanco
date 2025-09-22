using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Necesario si usas TextMeshPro para el Dropdown.

// Este script controla todo el menú de pausa y las opciones.
public class UIMenu : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelOptions;
    [SerializeField] private GameObject panelCredits; // Panel para los créditos.

    [Header("Botones")]
    [SerializeField] private Button buttonResume; // Botón para reanudar.
    [SerializeField] private Button buttonOptions; // Botón para ir a opciones.
    [SerializeField] private Button buttonBack; // Botón para volver de opciones al menú de pausa.
    [SerializeField] private Button buttonCredits;// Botón para ver créditos.
    [SerializeField] private Button buttonExit; // Botón para salir al menú principal.

    [Header("Controles de Opciones")]
    [SerializeField] private TMP_Dropdown colorDropdown; // Dropdown para el color.
    [SerializeField] private Slider heightSlider; // Slider para la altura.
    [SerializeField] private Slider speedSlider; // Slider para la velocidad.

    [Header("Referencias a los Jugadores")]
    [SerializeField] private PaddleMovement player1;
    [SerializeField] private PaddleMovement player2;

    private bool isPaused = false; // Para saber si el juego está en pausa.

    // Awake es para configurar los listeners de los botones y controles.
    private void Awake()
    {
        // Comprobación de seguridad para evitar errores.
        if (panelPause == null || panelOptions == null || panelCredits == null || buttonResume == null)
        {
            Debug.LogError("¡FALTAN REFERENCIAS EN UIMenu! Asegúrate de asignar todos los paneles y botones en el Inspector.");
            // Desactivamos el script para evitar más errores.
            this.enabled = false;
            return;
        }

        // Conectamos cada botón a su función.
        buttonResume.onClick.AddListener(TogglePause);
        buttonOptions.onClick.AddListener(ShowOptionsPanel);
        buttonBack.onClick.AddListener(ShowPausePanel);
        buttonCredits.onClick.AddListener(ShowCreditsPanel);
        buttonExit.onClick.AddListener(ExitToMainMenu);

        // Conectamos los controles de opciones.
        colorDropdown.onValueChanged.AddListener(ChangePaddlesColor);
        heightSlider.onValueChanged.AddListener(ChangePaddlesHeight);
        speedSlider.onValueChanged.AddListener(ChangePaddlesSpeed);
    }

    // Start se usa para asegurar el estado inicial.
    private void Start()
    {
        // Al empezar, nos aseguramos de que el juego no esté pausado.
        panelPause.SetActive(false);
        panelOptions.SetActive(false);
        panelCredits.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Update para leer la tecla Escape.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Pausa o reanuda el juego.
    public void TogglePause()
    {
        isPaused = !isPaused; // Invertimos el estado de pausa.

        if (isPaused)
        {
            // Si pausamos, mostramos el menú y congelamos el tiempo.
            ShowPausePanel();
            Time.timeScale = 0f;
        }
        else
        {
            // Si reanudamos, ocultamos todo y reanudamos el tiempo.
            panelPause.SetActive(false);
            panelOptions.SetActive(false);
            panelCredits.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    // Muestra el panel de pausa y oculta los demás.
    private void ShowPausePanel()
    {
        panelPause.SetActive(true);
        panelOptions.SetActive(false);
        panelCredits.SetActive(false);
    }

    // Muestra el panel de opciones y oculta el de pausa.
    private void ShowOptionsPanel()
    {
        panelPause.SetActive(false);
        panelOptions.SetActive(true);
    }

    // Muestra el panel de créditos.
    private void ShowCreditsPanel()
    {
        panelPause.SetActive(false);
        panelCredits.SetActive(true);
    }

    // Carga la escena del menú principal.
    private void ExitToMainMenu()
    {
        Time.timeScale = 1f; // ¡Importante! Restaurar el tiempo antes de cambiar de escena.
        SceneManager.LoadScene("MainMenu"); // Asegúrate de que tu escena se llame "MainMenu".
    }

    // --- Funciones para los Controles de Opciones ---

    private void ChangePaddlesColor(int colorIndex)
    {
        Color newColor = Color.white;
        switch (colorIndex)
        {
            case 0: newColor = Color.white; break;
            case 1: newColor = Color.red; break;
            case 2: newColor = Color.green; break;
            case 3: newColor = Color.blue; break;
        }
        player1.SetColor(newColor);
        player2.SetColor(newColor);
    }

    private void ChangePaddlesHeight(float newHeight)
    {
        player1.SetHeight(newHeight);
        player2.SetHeight(newHeight);
    }

    private void ChangePaddlesSpeed(float newSpeed)
    {
        player1.SetSpeed(newSpeed);
        player2.SetSpeed(newSpeed);
    }
}

