using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Define la clase PlayerController que implementa la interfaz IMove_Player_PCActions
public class PlayerController : MonoBehaviour, Player_Input_Action.IMove_Player_PCActions
{
    // Variable para almacenar las acciones de input
    private Player_Input_Action playerInputActions;
    // Variable para almacenar el input de movimiento
    private Vector2 moveInput;
    // Variable para almacenar el componente Rigidbody del jugador
    private Rigidbody rb;
    // Velocidad de movimiento del jugador
    public float moveSpeed = 5f;
    // Velocidad de carrera del jugador
    public float runSpeed = 10f;
    // Fuerza del salto del jugador
    public float jumpForce = 7f;
    // Velocidad de vuelo del jugador
    public float flySpeed = 10f;
    // Variable para determinar si el jugador est� en el suelo
    private bool isGrounded;
    // Variable para determinar si el jugador est� defendiendo
    private bool isDefending;
    // Variable para determinar si el jugador est� corriendo
    private bool isRunning;

    // M�todo llamado cuando el script de instancia est� siendo cargado
    private void Awake()
    {
        // Inicializa las acciones de input
        playerInputActions = new Player_Input_Action();
        // Obtiene el componente Rigidbody del jugador
        rb = GetComponent<Rigidbody>();
    }

    // M�todo llamado cuando el objeto se habilita
    private void OnEnable()
    {
        // Configura los callbacks para las acciones de Move_Player_PC
        playerInputActions.Move_Player_PC.SetCallbacks(this);
        // Habilita las acciones de Move_Player_PC
        playerInputActions.Move_Player_PC.Enable();
    }

    // M�todo llamado cuando el objeto se deshabilita
    private void OnDisable()
    {
        // Deshabilita las acciones de Move_Player_PC
        playerInputActions.Move_Player_PC.Disable();
        // Elimina los callbacks para las acciones de Move_Player_PC
        playerInputActions.Move_Player_PC.SetCallbacks(null);
    }

    // M�todo que se llama cuando se detecta un input de movimiento
    public void OnMove(InputAction.CallbackContext context)
    {
        // Lee el valor del input de movimiento y lo asigna a moveInput
        moveInput = context.ReadValue<Vector2>();
    }

    // M�todo que se llama cuando se detecta un input de salto
    public void OnJump(InputAction.CallbackContext context)
    {
        // Verifica si el jugador est� en el suelo
        if (isGrounded)
        {
            // Aplica una fuerza de salto al Rigidbody del jugador
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // M�todo que se llama cuando se detecta un input de vuelo
    public void OnFly(InputAction.CallbackContext context)
    {
        // Verifica si el input de vuelo est� activo
        if (context.performed)
        {
            // Desactiva la gravedad para el jugador
            rb.useGravity = false;
            // Asigna una nueva velocidad al jugador para volar
            rb.velocity = new Vector3(rb.velocity.x, flySpeed, rb.velocity.z);
        }
        // Verifica si el input de vuelo ha sido cancelado
        else if (context.canceled)
        {
            // Reactiva la gravedad para el jugador
            rb.useGravity = true;
        }
    }

    // M�todo que se llama cuando se detecta un input de ataque
    public void OnAttack(InputAction.CallbackContext context)
    {
        // Implementa la l�gica de ataque aqu�
        Debug.Log("Attack performed");
    }

    // M�todo que se llama cuando se detecta un input de defensa
    public void OnDefend(InputAction.CallbackContext context)
    {
        // Verifica si el input de defensa est� activo
        if (context.performed)
        {
            // Establece isDefending a true para indicar que el jugador est� defendiendo
            isDefending = true;
            // Implementa la l�gica de defensa aqu�
            Debug.Log("Defending");
        }
        // Verifica si el input de defensa ha sido cancelado
        else if (context.canceled)
        {
            // Establece isDefending a false para indicar que el jugador ha dejado de defender
            isDefending = false;
            // Implementa la l�gica para detener la defensa
            Debug.Log("Stopped Defending");
        }
    }

    // M�todo que se llama cuando se detecta un input de correr
    public void OnRun(InputAction.CallbackContext context)
    {
        // Verifica si el input de correr est� activo
        if (context.performed)
        {
            // Establece isRunning a true para indicar que el jugador est� corriendo
            isRunning = true;
        }
        // Verifica si el input de correr ha sido cancelado
        else if (context.canceled)
        {
            // Establece isRunning a false para indicar que el jugador ha dejado de correr
            isRunning = false;
        }
    }

    // M�todo llamado una vez por frame para actualizar la l�gica del juego
    private void Update()
    {
        // Asigna la velocidad de movimiento en funci�n de si el jugador est� corriendo o caminando
        float speed = isRunning ? runSpeed : moveSpeed;
        // Calcula el vector de movimiento basado en el input y la velocidad
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * speed * Time.deltaTime;
        // Mueve el jugador en el espacio del mundo
        transform.Translate(move, Space.World);
    }

    // M�todo llamado cuando el jugador colisiona con otro objeto
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el jugador ha colisionado con el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Establece isGrounded a true para indicar que el jugador est� en el suelo
            isGrounded = true;
        }
    }

    // M�todo llamado cuando el jugador deja de colisionar con otro objeto
    private void OnCollisionExit(Collision collision)
    {
        // Verifica si el jugador ha dejado de colisionar con el suelo
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Establece isGrounded a false para indicar que el jugador ha dejado de estar en el suelo
            isGrounded = false;
        }
    }
}
