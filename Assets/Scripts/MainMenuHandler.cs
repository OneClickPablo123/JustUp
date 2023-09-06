using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{

    public GameObject startButton;
    public GameObject exitButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton = GameObject.Find("StartButton");
        exitButton = GameObject.Find("ExitButton");
    }

    // Update is called once per frame
    void Update()
    {
        // Überprüfe, ob es mindestens einen Touch-Input auf dem Bildschirm gibt.
        if (Input.touchCount > 0)
        {
            // Schleife durch alle aktiven Touch-Inputs.
            foreach (Touch touch in Input.touches)
            {
                // Überprüfe, ob der Touch-Input über einem Button liegt.
                if (IsTouchOverButton(touch.position, startButton))
                {
                    SceneManager.LoadScene(1);
                }
                else if (IsTouchOverButton(touch.position, exitButton))
                {
                    Application.Quit();
                }
            }
        }
    }

public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private bool IsTouchOverButton(Vector2 touchPosition, GameObject button)
    {
        // Überprüfe, ob der Touch-Input innerhalb der Begrenzungen des Buttons liegt.
        Collider2D buttonCollider = button.GetComponent<Collider2D>();
        return buttonCollider.bounds.Contains(touchPosition);
    }


}
