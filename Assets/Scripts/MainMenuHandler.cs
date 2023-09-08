using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{


    //Save
    SaveGame saveGame;
    public MenuStats menuStats;

    //Main Menu
    GameObject menuPanel;
    GameObject startButton;
    GameObject exitButton;
    GameObject optionButton;

    //Options
    GameObject optionMenu;
    GameObject inputPanel;
    GameObject inputSystem1;
    GameObject inputSystem2;
    GameObject inputSystem3;

    // Start is called before the first frame update
    void Start()
    {
        saveGame = GetComponent<SaveGame>();
        saveGame.LoadMenuStats();
        menuPanel = GameObject.Find("MainMenuPanel");
        startButton = GameObject.Find("StartButton");
        exitButton = GameObject.Find("ExitButton");
        optionButton = GameObject.Find("OptionButton");
        

        //Options
        optionMenu = GameObject.Find("OptionPanel");
        inputPanel = optionMenu.transform.Find("InputPanel").gameObject;
        inputSystem1 = inputPanel.transform.Find("JoyStickInput").gameObject;
        inputSystem2 = inputPanel.transform.Find("SideInput").gameObject;
        inputSystem3 = inputPanel.transform.Find("TwoSidedInput").gameObject;
        
        optionMenu.SetActive(false);
        menuPanel.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        /* Überprüfe, ob es mindestens einen Touch-Input auf dem Bildschirm gibt.
        if (Input.touchCount > 0)
        {
            // Schleife durch alle aktiven Touch-Inputs.
            foreach (Touch touch in Input.touches)
            {
                // Überprüfe, ob der Touch-Input über einem Button liegt.
                if (IsTouchOverButton(touch.position, startButton))
                {
                    SceneManager.LoadScene(1);
                    Debug.Log("Game Started");
                }
                if (IsTouchOverButton(touch.position, exitButton))
                {
                    Application.Quit(); 
                    Debug.Log("Quit Touched");
                }
                if (IsTouchOverButton(touch.position, optionButton))
                {
                    menuPanel.SetActive(false);
                    optionMenu.SetActive(true);
                    Debug.Log("Option Touch");
                }
                else if (IsTouchOverButton(touch.position, inputSystem1))
                {
                    menuStats.inputSystem = 1;
                    saveGame.SaveMenuStats();
                }
                else if (IsTouchOverButton(touch.position, inputSystem2))
                {
                    menuStats.inputSystem = 2;
                    saveGame.SaveMenuStats();
                }
                else if (IsTouchOverButton(touch.position, inputSystem3))
                {
                    menuStats.inputSystem = 3;
                    saveGame.SaveMenuStats();
                }
            }
        } */
    } 

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(false);
        optionMenu.SetActive(true);
    }

    //JoyStick
    public void Control1()
    {
       menuStats.inputSystem = 1;

    }

    //Left Right Side
    public void Control2()
    {
        menuStats.inputSystem = 2;
    }

    // Left Side Walk, Right Side Jump
    public void Control3()
    {
        menuStats.inputSystem = 3;
    }

    public void OptionBackButton()
    {
        menuPanel.SetActive(true);
        optionMenu.SetActive(false);
        saveGame.SaveMenuStats();
    }

    private bool IsTouchOverButton(Vector2 touchPosition, GameObject button)
    {
        // Überprüfe, ob der Touch-Input innerhalb der Begrenzungen des Buttons liegt.
        Collider2D buttonCollider = button.GetComponent<Collider2D>();
        return buttonCollider.bounds.Contains(touchPosition);
    }


}
