using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject panelPause;      // Panel de pausa
    [SerializeField] private GameObject panelCredits;    // Panel de cr�ditos
    [SerializeField] private GameObject panelOptions;    // Panel de opciones

    [Header("Botones")]
    [SerializeField] private Button buttonPlay;          // Bot�n para reanudar el juego
    [SerializeField] private Button buttonOptions;       // Bot�n para abrir opciones
    [SerializeField] private Button buttonBackToMenu;    // Bot�n para volver de opciones a pausa
    [SerializeField] private Button buttonCredits;       // Bot�n para abrir cr�ditos
    [SerializeField] private Button buttonExit;          // Bot�n para salir al men� principal
    [SerializeField] private Button buttonCreditsBack;   // Bot�n para volver de cr�ditos

    [Header("Opciones de Juego")]
    [SerializeField] private Dropdown colorDropdown;     // Dropdown para seleccionar color del paddle
    [SerializeField] private Slider heightSlider;        // Slider para cambiar el tama�o del paddle
    [SerializeField] private Slider speedSlider;         // Slider para cambiar la velocidad del paddle

    [Header("Referencias a Jugadores")]
    [SerializeField] private PaddleMovement player1;     // Referencia al paddle del jugador 1
    [SerializeField] private PaddleMovement player2;     // Referencia al paddle del jugador 2

    private bool isPaused = true; // Indica si el juego est� en pausa

    // Asigna los listeners a los botones y sliders/dropdown al iniciar
    private void Awake()
    {
        if (buttonPlay != null)
            buttonPlay.onClick.AddListener(() => panelPause.SetActive(false)); // Reanuda el juego
        if (buttonOptions != null)
            buttonOptions.onClick.AddListener(OnOptionsClicked); // Abre opciones
        if (buttonCredits != null)
            buttonCredits.onClick.AddListener(() => panelCredits.SetActive(true)); // Abre cr�ditos
        if (buttonExit != null)
            buttonExit.onClick.AddListener(OnExitClicked); // Sale al men� principal
        if (buttonCreditsBack != null)
            buttonCreditsBack.onClick.AddListener(() => panelCredits.SetActive(false)); // Cierra cr�ditos
        if (buttonBackToMenu != null)
            buttonBackToMenu.onClick.AddListener(() =>
            {
                panelOptions.SetActive(false); // Cierra opciones
                panelPause.SetActive(true);    // Abre pausa
            });

        // Listeners para sliders y dropdown
        if (speedSlider != null) speedSlider.onValueChanged.AddListener(OnSpeedChanged);   // Cambia velocidad
        if (heightSlider != null) heightSlider.onValueChanged.AddListener(OnHeightChanged); // Cambia tama�o
        if (colorDropdown != null) colorDropdown.onValueChanged.AddListener(OnColorChanged); // Cambia color
    }

    // Permite pausar y reanudar el juego con la tecla Escape
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            panelPause.SetActive(isPaused);
            Time.timeScale = isPaused ? 0 : 1;
        }
    }

    // Elimina todos los listeners al destruir el objeto
    private void OnDestroy()
    {
        if (buttonPlay != null) buttonPlay.onClick.RemoveAllListeners();
        if (buttonOptions != null) buttonOptions.onClick.RemoveAllListeners();
        if (buttonCredits != null) buttonCredits.onClick.RemoveAllListeners();
        if (buttonExit != null) buttonExit.onClick.RemoveAllListeners();
        if (buttonCreditsBack != null) buttonCreditsBack.onClick.RemoveAllListeners();
        if (buttonBackToMenu != null) buttonBackToMenu.onClick.RemoveAllListeners();

        if (speedSlider != null) speedSlider.onValueChanged.RemoveAllListeners();
        if (heightSlider != null) heightSlider.onValueChanged.RemoveAllListeners();
        if (colorDropdown != null) colorDropdown.onValueChanged.RemoveAllListeners();
    }

    // Muestra el panel de opciones y oculta el de pausa
    private void OnOptionsClicked()
    {
        panelOptions.SetActive(true);
        panelPause.SetActive(false);
    }

    // Cambia a la escena del men� principal
    private void OnExitClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Cambia la velocidad de ambos paddles seg�n el valor del slider
    private void OnSpeedChanged(float value)
    {
        if (player1 != null) player1.SetSpeed(value);
        if (player2 != null) player2.SetSpeed(value);
    }

    // Cambia el tama�o (largo) de ambos paddles seg�n el valor del slider
    private void OnHeightChanged(float value)
    {
        if (player1 != null) player1.SetHeight(value);
        if (player2 != null) player2.SetHeight(value);
    }

    // Cambia el color de ambos paddles seg�n la opci�n seleccionada en el dropdown
    private void OnColorChanged(int index)
    {
        Color newColor = Color.white;
        switch (index)
        {
            case 0: newColor = Color.white; break;
            case 1: newColor = Color.red; break;
            case 2: newColor = Color.blue; break;
            case 3: newColor = Color.green; break;
        }
        if (player1 != null) player1.SetColor(newColor);
        if (player2 != null) player2.SetColor(newColor);
    }
}

