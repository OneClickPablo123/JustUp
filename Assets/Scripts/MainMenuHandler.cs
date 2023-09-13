using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    //Save
    SaveGame saveGame;

    //Main Menu
    GameObject menuPanel;
    //Options
    GameObject optionMenu;
    GameObject inputPanel;

    //EasyMode - Options
    public GameObject easyModeCheckBox;
    Image buttonImage;
    public Sprite easyModeChecked;
    public Sprite easyModeUnchecked;
    private bool easyMode;

    // Start is called before the first frame update
    void Start()
    {
        saveGame = GetComponent<SaveGame>();
        saveGame.LoadMenuStats();
        saveGame.LoadPlayerStats();
        menuPanel = GameObject.Find("MainMenuPanel");  
        buttonImage = easyModeCheckBox.GetComponent<Image>();
        
        if(saveGame.menuStats.easyMode == 1)
        {
            easyMode = true;
            buttonImage.sprite = easyModeChecked;
        } 
        else if (saveGame.menuStats.easyMode == 0)
        {
            easyMode = false;
            buttonImage.sprite = easyModeUnchecked;
        }

        //Options
        optionMenu = GameObject.Find("OptionPanel");
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
        saveGame.SaveMenuStats();
        saveGame.SavePlayerStats();
        Application.Quit();
    }

    public void StartGame()
    {
        Debug.Log(PlayerPrefs.GetInt("touchControls"));
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
        saveGame.menuStats.touchControls = 1;
    }

    //Left Right Side
    public void Control2()
    {
        saveGame.menuStats.touchControls = 2;
    }

    // Left Side Walk, Right Side Jump
    public void Control3()
    {
        saveGame.menuStats.touchControls = 3;
    }

    public void OptionBackButton()
    {
        saveGame.SaveMenuStats();
        menuPanel.SetActive(true);
        optionMenu.SetActive(false);
       
    }

    public void EasyMode()
    {
        if (easyMode)
        {
            easyMode = false;
            buttonImage.sprite = easyModeUnchecked;
            saveGame.menuStats.easyMode = 0;
        }
        else
        {
            easyMode = true;
            buttonImage.sprite = easyModeChecked;
            saveGame.menuStats.easyMode = 1;
        }
    }

    private bool IsTouchOverButton(Vector2 touchPosition, GameObject button)
    {
        // Überprüfe, ob der Touch-Input innerhalb der Begrenzungen des Buttons liegt.
        Collider2D buttonCollider = button.GetComponent<Collider2D>();
        return buttonCollider.bounds.Contains(touchPosition);
    }


}
