using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    //Save
    SaveGame saveGame;

    //Main Menu
    GameObject menuPanel;
    AudioSource audioSource;
    public TextMeshProUGUI startButtonText;
    
    //Option Panel
    public GameObject optionMenu;
    public GameObject inputPanel;

    //EasyMode - Options
    public GameObject easyModeCheckBox;
    Image buttonImage;
    public Sprite checkBoxChecked;
    public Sprite checkBoxUnchecked;
    private bool easyMode;

    //NewGame - Options
    public GameObject warningPanel;

    //Settings Tab
    public GameObject generalPanel;
    public GameObject soundPanel;
    public GameObject graphicPanel;

    //Graphic Settings
    public Image shadowCheckBox;

    //TouchControl Options
    public GameObject joyStickCheckBox;
    Image joyStickImage;
    public GameObject alternativeCheckBox;
    Image alternativeImage;
    public GameObject standardCheckBox;
    Image standardImage;

    //Sound Options
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectVolumeSlider;


    
    private void Awake()
    {
        saveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();   
    }

    void Start()
    {
        //Menu
        menuPanel = GameObject.Find("MainMenuPanel");  
        buttonImage = easyModeCheckBox.GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();

        //Check if the Player Already Played the Game. HAS TO BE PLAYERPREFS!!!
       if (PlayerPrefs.GetInt("firstPlayed") == 0)
        {
            Debug.Log("Create");
            saveGame.CreateMenuStats();
            saveGame.CreatePlayerStats();
            saveGame.playerStats.firstPlayed = 1;
        } 
        else
        {
            saveGame.LoadMenuStats();
            saveGame.LoadPlayerStats();
        }

        //Start Button Text
        if (saveGame.playerStats.firstPlayed == 1)
        {
            startButtonText.text = "New Game";
        } 
        else if (saveGame.playerStats.firstPlayed == 2)
        {
            startButtonText.text = "Resume Game";
        }


        if (saveGame.menuStats.shadowsEnabled == 1)
        {
            shadowCheckBox.sprite = checkBoxChecked;
        }
        else
        {
            shadowCheckBox.sprite = checkBoxUnchecked;
        }

        //Options
        optionMenu.SetActive(false);
        menuPanel.SetActive(true);
        generalPanel.SetActive(true);
        soundPanel.SetActive(false);
        warningPanel.SetActive(false);

        //TouchControls
        joyStickImage = joyStickCheckBox.GetComponent<Image>();
        alternativeImage = alternativeCheckBox.GetComponent<Image>();
        standardImage = standardCheckBox.GetComponent<Image>();

        joyStickCheckBox.SetActive(false);
        alternativeCheckBox.SetActive(false);
        standardCheckBox.SetActive(false);

        if(saveGame.menuStats.easyMode == 1)
        {
            easyMode = true;
            buttonImage.sprite = checkBoxChecked;
        } 
        else if (saveGame.menuStats.easyMode == 0)
        {
            easyMode = false;
            buttonImage.sprite = checkBoxUnchecked;
        }
      
        //Sound
        masterVolumeSlider.value = saveGame.menuStats.masterVolume;
        musicVolumeSlider.value = saveGame.menuStats.musicVolume;
        effectVolumeSlider.value = saveGame.menuStats.effectVolume;


    }

    // Update is called once per frame
    void Update()
    {
        TouchControlBox();
        MusicController();
    } 

    public void QuitGame()
    {
        saveGame.SaveMenuStats();
        saveGame.SavePlayerStats();
        Application.Quit();
    }

    public void StartGame()
    {
        saveGame.playerStats.firstPlayed = 2;
        saveGame.SavePlayerStats();
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
            buttonImage.sprite = checkBoxUnchecked;
            saveGame.menuStats.easyMode = 0;
        }
        else
        {
            easyMode = true;
            buttonImage.sprite = checkBoxChecked;
            saveGame.menuStats.easyMode = 1;
        }
    }
    
    public void TouchControlBox()
    {
        //JoyStick
        if (saveGame.menuStats.touchControls == 1)
        {
            joyStickCheckBox.SetActive(true);
            joyStickImage.sprite = checkBoxChecked;

        } else
        {
            joyStickCheckBox.SetActive(false);
        }
        //Alternative
        if (saveGame.menuStats.touchControls == 2)
        {
            alternativeCheckBox.SetActive(true);
            alternativeImage.sprite = checkBoxChecked;
        } else
        {
            alternativeCheckBox.SetActive(false);
        }
        //Standard
        if (saveGame.menuStats.touchControls == 3)
        {
            standardCheckBox.SetActive(true);
            standardImage.sprite = checkBoxChecked;
        } else
        {
            standardCheckBox.SetActive(false);
        }
    }
   
    public void MusicController()
    {
        audioSource.volume = masterVolumeSlider.value * musicVolumeSlider.value;
        saveGame.menuStats.masterVolume = masterVolumeSlider.value;
        saveGame.menuStats.musicVolume = musicVolumeSlider.value;
        saveGame.menuStats.effectVolume = effectVolumeSlider.value;
    }

    public void ShowWarningNewGame()
    {
        warningPanel.SetActive(true);
    }

    public void StartNewGame()
    {
        saveGame.CreatePlayerStats();
        saveGame.SavePlayerStats();
        warningPanel.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void CloseWarning()
    {
        warningPanel.SetActive(false);
    }

    public void ShowSoundSetting()
    {
        graphicPanel.SetActive(false);
        generalPanel.SetActive(false);
        soundPanel.SetActive(true);
    }

    public void ShowGraphicSettings()
    {
        graphicPanel.SetActive(true);
        generalPanel.SetActive(false);
        soundPanel.SetActive(false);
    }
   
    public void ShowGeneralSettings()
    {
        generalPanel.SetActive(true);
        soundPanel.SetActive(false);
        graphicPanel.SetActive(false);
    }

    public void ShadowSettings()
    {
        if (saveGame.menuStats.shadowsEnabled == 1)
        {
            saveGame.menuStats.shadowsEnabled = 0;
            shadowCheckBox.sprite = checkBoxUnchecked;
        } 
        else
        {
            saveGame.menuStats.shadowsEnabled = 1;
            shadowCheckBox.sprite = checkBoxChecked;
        }
    }

    private bool IsTouchOverButton(Vector2 touchPosition, GameObject button)
    {
        // Überprüfe, ob der Touch-Input innerhalb der Begrenzungen des Buttons liegt.
        Collider2D buttonCollider = button.GetComponent<Collider2D>();
        return buttonCollider.bounds.Contains(touchPosition);
    }


}
