using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour, Player_Input_Action.IMove_Player_MobileActions
{
    // Declarar variables
    private Player_Input_Action playerInputActions; // Maneja las acciones de input
    private Rigidbody rb; // Componente Rigidbody del jugador

    public MobileJoystick joystick; // Script del joystick (para joystick virtual)
    public Button jumpButton; // Botón de salto
    public Button flyButton; // Botón de vuelo
    public Button attackButton; // Botón de ataque
    public Button defendButton; // Botón de defensa
    public Button runButton; // Botón de correr

    private Vector2 moveInput; // Almacena el input de movimiento
    private bool isRunning; // Estado de correr
    private bool isJumping; // Estado de salto
    private bool isFlying; // Estado de vuelo
    private bool isAttacking; // Estado de ataque
    private bool isDefending; // Estado de defensa

    public float moveSpeed = 5f; // Velocidad de movimiento
    public float runSpeed = 10f; // Velocidad de correr
    public float jumpForce = 7f; // Fuerza de salto
    public float flySpeed = 10f; // Velocidad de vuelo

    private void Awake()
    {
        // Inicializar las acciones de input y obtener el componente Rigidbody
        playerInputActions = new Player_Input_Action();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Configurar y habilitar los callbacks de las acciones de input
        playerInputActions.Move_Player_Mobile.SetCallbacks(this);
        playerInputActions.Move_Player_Mobile.Enable();

        // Agregar listeners a los botones
        jumpButton.onClick.AddListener(OnJumpButtonPressed);
        flyButton.onClick.AddListener(OnFlyButtonPressed);
        attackButton.onClick.AddListener(OnAttackButtonPressed);
        defendButton.onClick.AddListener(OnDefendButtonPressed);
        runButton.onClick.AddListener(OnRunButtonPressed);
    }

    private void OnDisable()
    {
        // Deshabilitar los callbacks de las acciones de input
        playerInputActions.Move_Player_Mobile.Disable();
        playerInputActions.Move_Player_Mobile.SetCallbacks(null);

        // Eliminar listeners de los botones
        jumpButton.onClick.RemoveListener(OnJumpButtonPressed);
        flyButton.onClick.RemoveListener(OnFlyButtonPressed);
        attackButton.onClick.RemoveListener(OnAttackButtonPressed);
        defendButton.onClick.RemoveListener(OnDefendButtonPressed);
        runButton.onClick.RemoveListener(OnRunButtonPressed);
    }

    private void Update()
    {
        // Obtener el input de movimiento del joystick
        moveInput = new Vector2(joystick.Horizontal(), joystick.Vertical());
        float speed = isRunning ? runSpeed : moveSpeed; // Determinar la velocidad basada en el estado de correr

        // Calcular el vector de movimiento
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * speed * Time.deltaTime;

        // Mover al jugador
        transform.Translate(move, Space.World);

        // Manejar el salto
        if (isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }

        // Manejar el vuelo
        if (isFlying)
        {
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, flySpeed, rb.velocity.z);
            isFlying = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    // Métodos vacíos para implementar la interfaz IMove_Player_MobileActions
    public void OnMove(InputAction.CallbackContext context) { }
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnFly(InputAction.CallbackContext context) { }
    public void OnAttack(InputAction.CallbackContext context) { }
    public void OnDefend(InputAction.CallbackContext context) { }
    public void OnRun(InputAction.CallbackContext context) { }

    // Métodos para manejar los eventos de los botones
    private void OnJumpButtonPressed()
    {
        isJumping = true;
    }

    private void OnFlyButtonPressed()
    {
        isFlying = !isFlying;
    }

    private void OnAttackButtonPressed()
    {
        isAttacking = true;
        Debug.Log("Attack performed");
    }

    private void OnDefendButtonPressed()
    {
        isDefending = true;
        Debug.Log("Defending");
    }

    private void OnRunButtonPressed()
    {
        isRunning = !isRunning;
    }

    // Método para manejar la colisión con el suelo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}

