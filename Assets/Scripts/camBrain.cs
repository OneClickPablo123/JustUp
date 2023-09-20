using UnityEngine;

public class camBrain : MonoBehaviour
{
    public Transform target; // Der Spieler, dem die Kamera folgen soll
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Offsets für die Kamera-Position
    public float smoothSpeed = 5f; // Geschwindigkeit, mit der die Kamera dem Spieler folgt

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset; // Zielposition der Kamera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime); // Übergang zur Zielposition

            transform.position = smoothedPosition; // Aktualisieren Sie die Kamera-Position
        }
    }
}

