using UnityEngine.Rendering.Universal;
using UnityEngine;
using System.Collections;

public class FireFlicker : MonoBehaviour
{
    public float minIntensity = 1f;
    public float maxIntensity = 2f;
    public float flickerSpeed = 1f;
    public Color minColor = Color.red; // Mindestfarbe des Lichts (rot)
    public Color maxColor = Color.yellow; // Maximalfarbe des Lichts (orange)

    private Light2D fireLight;
    private float originalIntensity;
    private Color originalColor;

    void Start()
    {
        fireLight = GetComponent<Light2D>();
        originalIntensity = fireLight.intensity;
        originalColor = fireLight.color;

        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            float randomIntensity = Random.Range(minIntensity, maxIntensity);
            Color randomColor = Color.Lerp(minColor, maxColor, Random.Range(0f, 1f));

            fireLight.intensity = randomIntensity;
            fireLight.color = randomColor;

            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) / flickerSpeed);

            fireLight.intensity = originalIntensity;
            fireLight.color = originalColor;

            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f) / flickerSpeed);
        }
    }
}