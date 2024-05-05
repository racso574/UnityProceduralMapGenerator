using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento del jugador

    void Update()
    {
        // Obtener la entrada del teclado
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calcular el vector de movimiento
        Vector3 movement = new Vector3(verticalInput, 0f, -horizontalInput) * speed * Time.deltaTime;

        // Rotar el vector de movimiento en -45 grados para compensar la rotación del juego
        movement = Quaternion.Euler(0, -45, 0) * movement;

        // Mover el jugador utilizando Transform.position
        transform.position += movement;
    }
}



