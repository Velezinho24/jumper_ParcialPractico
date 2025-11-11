using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Vector3 startPosition;

    void Start()
    {
        // Posición inicial del jugador
        startPosition = new Vector3(0f, 2f, 0f);
    }

    void Update()
    {
        // Si se cae al vacío
        if (transform.position.y < -10f)
        {
            Respawn();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto tiene el tag "Obstacle"
        if (collision.collider.CompareTag("Obstacle"))
        {
            // Revisa los puntos de contacto
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Si el contacto es lateral (normal apunta en X), el jugador muere
                if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    Respawn();
                    return;
                }
            }
        }
    }

    private void Respawn()
    {
        // Reinicia la posición
        transform.position = startPosition;

        // Detiene cualquier movimiento residual
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Debug.Log("💀 Jugador murió por colisión lateral o caída, reiniciando...");
    }
}

