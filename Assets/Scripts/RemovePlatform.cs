using UnityEngine;

public class RemovePlatform : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool playerOnPlatform;
    private float timeOnPlatform;
    private bool isDisappearing = false;

    public float disappearTime = 3f;
    public float appearTime = 1f;
    private float fadeSpeed;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        fadeSpeed = disappearTime;
    }

    private void Update()
    {
        if (playerOnPlatform && !isDisappearing)
        {
            timeOnPlatform += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timeOnPlatform / disappearTime);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            if (timeOnPlatform >= disappearTime)
            {
                isDisappearing = true;
                gameObject.SetActive(false);
                timeOnPlatform = 0f;
                Invoke("ReactivatePlatform", appearTime);
            }
        }
    }

    private void ReactivatePlatform()
    {
        isDisappearing = false;
        spriteRenderer.color = originalColor;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = false;
            timeOnPlatform = 0f;
        }
    }
}