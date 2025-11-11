using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1; // valor de la moneda
    public float rotationSpeed = 90f;

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el que tocó la moneda es el jugador
        if (other.CompareTag("Player"))
        {
            // Destruye la moneda (desaparece)
            Destroy(gameObject);
        }
    }
}
