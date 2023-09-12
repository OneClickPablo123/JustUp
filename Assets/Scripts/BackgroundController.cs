using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    public Transform layer; // Die Hintergrundebene
    public Vector2 parallaxFactors; // Die Parallax-Faktoren für jede Ebene (horizontal und vertikal)
}

public class BackgroundController : MonoBehaviour
{
    public ParallaxLayer[] layers; // Eine Liste der Hintergrundebenen
    public float maxHeight = 10f; // Die maximale Höhe, bis zu der der Parallax-Effekt voll wirkt
    public float minY = 0f; // Die minimale Y-Position, unter die die Ebene nicht fallen darf

    private Vector3 previousCameraPosition; // Die vorherige Position der Kamera

    void Start()
    {
        previousCameraPosition = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        Vector3 cameraMovement = Camera.main.transform.position - previousCameraPosition;
        Vector2 currentCameraPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        for (int i = 0; i < layers.Length; i++)
        {
            ParallaxLayer layer = layers[i];

            // Berücksichtige die maximale Höhe und schwäche den Parallax-Effekt ab, wenn sie überschritten wird
            float parallaxX = (currentCameraPosition.y <= maxHeight) ? cameraMovement.x * layer.parallaxFactors.x : 0f;
            float parallaxY = (currentCameraPosition.y <= maxHeight) ? cameraMovement.y * layer.parallaxFactors.y : 0f;

            Vector3 layerPosition = layer.layer.position;
            layerPosition.x += parallaxX;
            layerPosition.y += parallaxY;

            // Stelle sicher, dass die Ebene nicht unter minY fällt
            layerPosition.y = Mathf.Max(layerPosition.y, minY);

            layer.layer.position = layerPosition;
        }

        previousCameraPosition = Camera.main.transform.position;
    }
}