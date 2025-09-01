using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaddleMovement : MonoBehaviour
{
    [Header("PaddleMovement")]
    [SerializeField] private float movementSpeed = 10f; // Velocidad de movimiento vertical
    [SerializeField] private KeyCode moveUpKey = KeyCode.W; // Tecla para mover arriba
    [SerializeField] private KeyCode moveDownKey = KeyCode.S; // Tecla para mover abajo

    [Header("Rotate")]
    [SerializeField] private float rotationSpeed = 10.0f; // Velocidad de rotación
    [SerializeField] private KeyCode rotateToRight = KeyCode.D; // Tecla rotar derecha
    [SerializeField] private KeyCode rotateToLeft = KeyCode.A; // Tecla rotar izquierda

    [Header("Colors")]
    [SerializeField] private KeyCode paddleColor = KeyCode.R; // Tecla para cambiar color

    private SpriteRenderer spriteRenderer; // Cambia el color del paddle
    private Rigidbody2D _rigidbody2D;      // Mueve el paddle   

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Rotate();      // Rotación con teclas
        ChangeColor(); // Cambia color con tecla
    }

    private void FixedUpdate()
    {
        Move(); // Movimiento vertical
    }

    // Movimiento vertical según teclas y velocidad
    private void Move()
    {
        Vector2 pos = _rigidbody2D.position;
        if (Input.GetKey(moveUpKey))
            pos.y += movementSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(moveDownKey))
            pos.y -= movementSpeed * Time.fixedDeltaTime;
        _rigidbody2D.MovePosition(pos);
    }

    // Rotación según teclas
    private void Rotate()
    {
        float rotation = 0f;
        if (Input.GetKey(rotateToRight))
            rotation += rotationSpeed;
        if (Input.GetKey(rotateToLeft))
            rotation -= rotationSpeed;
        if (rotation != 0f)
            transform.Rotate(0, 0, rotation);
    }

    // Cambia el color a uno aleatorio al soltar la tecla
    private void ChangeColor()
    {
        if (Input.GetKeyUp(paddleColor))
            spriteRenderer.color = new Color(Random.value, Random.value, Random.value);
    }

    // Cambia el color desde UI
    public void SetColor(Color newColor)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = newColor;
    }

    // Cambia el largo del paddle desde UI
    public void SetHeight(float value)
    {
        var t = transform.localScale;
        t.y = value;
        transform.localScale = t;
    }

    // Cambia la velocidad desde UI
    public void SetSpeed(float newSpeed)
    {
        movementSpeed = newSpeed;
    }
}



